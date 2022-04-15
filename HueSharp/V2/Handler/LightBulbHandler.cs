using HueSharp.Base;
using HueSharp.Command;
using HueSharp.V1;
using HueSharp.V2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HueSharp.V2.Handler
{
    public class LightBulbHandler : BaseHandler
    {
        private string _baseURL;
        private HueClientV1 _oldClient;
        private JsonSerializerSettings _serializationSettings;

        public LightBulbHandler(HttpClient client, string ipAddress, HueClientV1 clientV1) : base(client)
        {
            _baseURL = $"https://{ipAddress}/clip/v2/resource/light";
            _oldClient = clientV1;
            _serializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Command the bridge to open network for new light bulbs. Search takes 40 seconds.
        /// </summary>
        /// <returns></returns>
        public async Task SearchForNewLightBulbs()
        {
            await _oldClient.LightBulbHandler.SearchForNewLightBulbs();
        }

        /// <summary>
        /// Returns last added light bulbs to the bridge.
        /// </summary>
        /// <returns>List of LightBulb</returns>
        public async Task<IEnumerable<LightBulb>> GetLastAddedLightBulbs()
        {
            List<LightBulb> lightBulbs = new List<LightBulb>();
            var response = await _oldClient.LightBulbHandler.GetLastAddedLightBulbs();
            var allLightBulbs = await GetLightBulbsAsync();
            return allLightBulbs.Where(bulb => response.Contains(bulb.IDv1)).Select(bulb => bulb);
        }

        /// <summary>
        /// Returns all LightBulbs configured in the bridge.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LightBulb>> GetLightBulbsAsync()
        {
            var response = await GetCommandAsync(_baseURL);
            JObject dataObject = JsonConvert.DeserializeObject<JObject>(response);
            var dataElement = dataObject["data"];
            return JsonConvert.DeserializeObject<List<LightBulb>>(dataElement.ToString());
        }

        public async Task<bool> DeleteLightBulbAsync(string idV1)
        {
            return await _oldClient.LightBulbHandler.DeleteLightBulb(idV1);
        }

        /// <summary>
        /// Sends light bulb command to the bridge. Calls to more than 10 devices / 1 group are paged and delayed to not overload the bridge
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task SendLightBulbCommandAsync(LightBulbCommand command)
        {
            // 10 commands per second to device
            // 1 command per second to group
            if(command.Bulbs != null)
            {
                byte skip = 0;
                int size;
                do
                {
                    if(skip > 0)
                    {
                        await Task.Delay(1000);
                    }
                    var bulbIDs = command.Bulbs.Skip(skip).Take(10).ToList();
                    size = bulbIDs.Count;
                    foreach (var bulbID in bulbIDs)
                    {
                        LightBulb lightBulb = command;
                        var content = JsonConvert.SerializeObject(lightBulb, Formatting.None, _serializationSettings);
                        // TODO: handle answer
                        var response = await PutCommandAsync($"{_baseURL}/{bulbID}", content);
                    }
                } while (size == 10);
            }

            //TODO: implement group light
        }
    }
}

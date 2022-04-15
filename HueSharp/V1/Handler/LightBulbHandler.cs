using HueSharp.Base;
using HueSharp.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.V1.Handler
{
    public class LightBulbHandler : BaseHandler
    {
        private string _ipAddress;
        private string _baseURL;
        
        public LightBulbHandler(HttpClient client, string ipAddress, string applicationKey) : base(client)
        {
            _ipAddress = ipAddress;
            SetApplicationKey(applicationKey);
        }

        public void SetApplicationKey(string applicationKey)
        {
            _baseURL = $"https://{_ipAddress}/api/{applicationKey}/lights";
        }

        /// <summary>
        /// Command the bridge to open network for new light bulbs. Search takes 40 seconds.
        /// </summary>
        /// <returns></returns>
        public async Task SearchForNewLightBulbs()
        {
            await PostCommandAsync(_baseURL, "");
        }

        public async Task<IEnumerable<string>> GetLastAddedLightBulbs()
        {
            List<string> lightBulbIDs = new List<string>();
            var response = await GetCommandAsync($"{_baseURL}/new");
            try
            {
                JObject scanObject = JsonConvert.DeserializeObject<JObject>(response);
                foreach (var element in scanObject)
                {
                    if (element.Key != "lastscan")
                    {
                        lightBulbIDs.Add(element.Key);
                    }
                }
                return lightBulbIDs;
            }
            finally
            {
                HandleUnexpectedException(response);
            }
        }

        public async Task<bool> DeleteLightBulb(string id)
        {
            var response = await DeleteCommandAsync($"{_baseURL}/{id}");
            JArray dataArray = JsonConvert.DeserializeObject<JArray>(response);
            var dataElement = (JObject)dataArray.First();
            if (dataElement.TryGetValue("success", out JToken successToken))
            {
                return true;
            }
            else if (dataElement.TryGetValue("error", out JToken errorToken))
            {
                var description = errorToken["description"]?.Value<string>();
                if (description.Contains("not available"))
                {
                    throw new ResourceDoesNotExistException($"Resource with ID: {id} does not exist");
                }
            }
            throw new HueException($"Unexpected response: {response}");
        }

        protected void HandleUnexpectedException(string content)
        {
            try
            {
                JArray dataArray = JsonConvert.DeserializeObject<JArray>(content);
                var dataElement = (JObject)dataArray.First();
                if (dataElement.TryGetValue("error", out JToken errorToken))
                {
                    if (errorToken["description"]?.Value<string>() == "unauthorized user")
                    {
                        throw new NotAuthorizedException();
                    }
                    else
                    {
                        throw new HueException(errorToken["description"]?.Value<string>());
                    }
                }
            }
            finally
            {
                throw new HueException($"Unexpected content: {content}");
            }
        }
    }
}

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

namespace HueSharp.V2.Handler
{
    public class RegistrationHandler : BaseHandler
    {
        private string _registrationURL;

        public delegate void ApplicationRegisteredEventHandler(object source, ApplicationRegistrationEventArgs args);
        public event ApplicationRegisteredEventHandler ApplicationRegistered;

        public RegistrationHandler(HttpClient client, string ipAddress) : base(client)
        {
            _registrationURL = $"https://{ipAddress}/api";
        }

        /// <summary>
        /// Registers the application to the Hue bridge. Please do not forget to push the hardware button before executing this method.
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="deviceName"></param>
        /// <returns>T1: Appllication key (used for API calls) T2: Client key (used for streaming)</returns>
        public async Task<Tuple<string, string>> RegisterApplication(string applicationName, string deviceName)
        {
            var registerJson = $"{{\"devicetype\":\"{applicationName}#{deviceName}\", \"generateclientkey\":true}}";
            var response = await PostCommandAsync(_registrationURL, registerJson);

            JArray dataArray = JsonConvert.DeserializeObject<JArray>(response);
            var dataElement = (JObject)dataArray.First();

            if (dataElement.TryGetValue("error", out JToken errorToken))
            {
                if (errorToken["type"]?.Value<int>() == 101)
                {
                    throw new PhysicalButtonNotPressedException();
                }
                else
                {
                    throw new HueException(errorToken["description"]?.Value<string>());
                }
            }

            var applicationKey = dataElement["success"]["username"].Value<string>();
            var clientKey = dataElement["success"]["clientkey"].Value<string>();
            OnApplicationRegistered(applicationKey, clientKey);

            return new Tuple<string, string>(applicationKey, clientKey);
        }

        protected virtual void OnApplicationRegistered(string applicationKey, string clientKey)
        {
            ApplicationRegistered?.Invoke(this, new ApplicationRegistrationEventArgs(applicationKey, clientKey));
        }

    }
}

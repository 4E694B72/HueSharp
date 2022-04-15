using HueSharp.Base;
using HueSharp.V1;
using HueSharp.V2.Handler;

namespace HueSharp.V2
{
    public class HueClientV2 : BaseHueClient
    {
        public string StreamKey { get; set; }

        public RegistrationHandler RegistrationHandler { get; private set; }

        public LightBulbHandler LightBulbHandler { get; private set; }

        private HueClientV1 _oldClient;

        public HueClientV2(string ipAddress, string applicationKey = null, string streamingKey = null) : base(ipAddress, "./Resources/signify_huebridge_cacert.pem", applicationKey)
        {
            _oldClient = new HueClientV1(ipAddress, applicationKey);
            _httpClient.DefaultRequestHeaders.Add("hue-application-key", applicationKey);
            StreamKey = streamingKey;
            HandlerSetup();
        }

        private void HandlerSetup()
        {
            RegistrationHandler = new RegistrationHandler(_httpClient, IPAddress);
            RegistrationHandler.ApplicationRegistered += OnApplicationRegistered;

            LightBulbHandler = new LightBulbHandler(_httpClient, IPAddress, _oldClient);
        }

        protected void OnApplicationRegistered(object source, ApplicationRegistrationEventArgs args)
        {
            SetApplicationKey(args.ApplicationKey);
            StreamKey = args.ClientKey;
        }

        public void SetApplicationKey(string applicationKey)
        {
            AppKey = applicationKey;
            _oldClient.AppKey = applicationKey;
            _httpClient.DefaultRequestHeaders.Add("hue-application-key", applicationKey);
            _oldClient.SetApplicationKey(applicationKey);
        }
    }
}

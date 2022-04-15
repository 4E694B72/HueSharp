using HueSharp.Base;
using HueSharp.V1.Handler;

namespace HueSharp.V1
{
    public class HueClientV1 : BaseHueClient
    {
        public LightBulbHandler LightBulbHandler { get; private set; }

        public HueClientV1(string ipAddress, string applicationKey = null) : base(ipAddress, "./Resources/signify_huebridge_cacert.pem", applicationKey)
        {
            HandlerSetup();
        }

        private void HandlerSetup()
        {
            LightBulbHandler = new LightBulbHandler(_httpClient, IPAddress, AppKey);
        }

        public void SetApplicationKey(string applicationKey)
        {
            AppKey = applicationKey;
            LightBulbHandler.SetApplicationKey(applicationKey);
        }

        

    }
}

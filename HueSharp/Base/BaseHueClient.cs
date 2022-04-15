using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace HueSharp.Base
{
    public abstract class BaseHueClient
    {
        protected string IPAddress { get; set; }

        public string AppKey { get; set; }
        
        protected HttpClient _httpClient;

        private string _pathToHueRootCA;
        
        protected X509Certificate2 _hueBridgeRootCA;

        protected X509Chain _chain;

        public BaseHueClient(string ipAddress, string pathToHueRootCA, string applicationKey = null)
        {
            IPAddress = ipAddress;
            _pathToHueRootCA = pathToHueRootCA;
            _hueBridgeRootCA = GetHueBridgeRootCA();
            _chain = ConfigureX509Chain();
            _httpClient = new HttpClient(GetHttpClientHandler());
            AppKey = applicationKey;
        }

        // TODO: if Signify root CA is signed by a trusted root CA, remove this method
        protected X509Certificate2 GetHueBridgeRootCA()
        {
            string rootHueBridgeCA = File.ReadAllText(_pathToHueRootCA);
            rootHueBridgeCA = rootHueBridgeCA.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r\n", "");
            return new X509Certificate2(Convert.FromBase64String(rootHueBridgeCA));
        }

        // TODO: if Signify root CA is signed by a trusted root CA, remove this method
        protected X509Chain ConfigureX509Chain()
        {
            X509Chain chain = new X509Chain();
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.ExtraStore.Add(_hueBridgeRootCA);
            return chain;
        }

        protected HttpClientHandler GetHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, certChain, policyErrors) =>
                {
                    if (policyErrors == System.Net.Security.SslPolicyErrors.None)
                    {
                        return true;
                    }

                    // Handling Hue bridge certs is not ideal - can not connect properly since I always get RemoteCertificateNameMismatch
                    // This procedure checks if at least the chain contains the private Signify CA certificate for Hue bridges - if not, we do not trust the cert
                    // Signify private CA Certificate for Hue Bridges can be found here: https://developers.meethue.com/develop/application-design-guidance/using-https/
                    // Since the current implementation encapsules the root cert in this app, the chain will give an error on untrusted root CA. Configuring custom root ca in client handler is possible in .NET 5
                    if (!_chain.Build(_hueBridgeRootCA) && (!(_chain.ChainStatus.Length == 1 && _chain.ChainStatus.First().Status == X509ChainStatusFlags.UntrustedRoot)))
                    {
                        return false;
                    }
                    return _chain.ChainElements.Cast<X509ChainElement>().Any(x => x.Certificate.Thumbprint == _hueBridgeRootCA.Thumbprint);
                }
            };
            return handler;
        }
    }
}

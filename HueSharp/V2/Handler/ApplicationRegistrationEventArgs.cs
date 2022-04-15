using System;

namespace HueSharp.V2.Handler
{
    public class ApplicationRegistrationEventArgs : EventArgs
    {
        private readonly string _applicationKey;
        private readonly string _clientKey;

        public ApplicationRegistrationEventArgs(string applicationKey, string clientKey)
        {
            _applicationKey = applicationKey;
            _clientKey = clientKey;
        }

        public string ApplicationKey => _applicationKey;

        public string ClientKey => _clientKey;
    }
}

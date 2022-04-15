using HueSharp.Exceptions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Base
{
    public class BaseHandler
    {
        private HttpClient _httpClient;

        public BaseHandler(HttpClient client)
        {
            _httpClient = client;
        }

        protected async Task<string> PostCommandAsync(string url, string content)
        {
            var response = await _httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new NotAuthorizedException();
            }
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseAsString;
        }

        protected async Task<string> PutCommandAsync(string url, string content)
        {
            var response = await _httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new NotAuthorizedException();
            }
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseAsString;
        }

        protected async Task<string> GetCommandAsync(string url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new NotAuthorizedException();
            }
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseAsString;
        }

        protected async Task<string> DeleteCommandAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new NotAuthorizedException();
                }
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseAsString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

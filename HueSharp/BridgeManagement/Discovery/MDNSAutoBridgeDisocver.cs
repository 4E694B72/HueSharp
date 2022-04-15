using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueSharp.BridgeManagement.Discovery
{
    public class MDNSAutoBridgeDisocver : IAutoBridgeDiscover
    {
        protected string HUE_SERVICE_NAME = "_hue._tcp.local";
        protected int MDNS_TIMEOUT = 30000;

        /// <summary>
        /// This method discovers Hue bridges via mDNS by sending UDP packets on 224.0.0.251 / ff02::fb on port 5353. Search takes 30 seconds.
        /// </summary>
        /// <returns>A list of Hue Bridges with their IPs</returns>
        public async Task<List<string>> GetHuesInNetwork()
        {
            List<string> hueIPs = new List<string>();
            var mdns = new MulticastService();
            mdns.NetworkInterfaceDiscovered += (s, e) => mdns.SendQuery(HUE_SERVICE_NAME);
            mdns.AnswerReceived += (s, e) =>
            {
                string ip = e.RemoteEndPoint.Address.ToString();
                if(!hueIPs.Contains(ip))
                {
                    hueIPs.Add(ip);
                }
            };
            mdns.Start();
            await Task.WhenAny(Task.Delay(MDNS_TIMEOUT));
            mdns.Stop();

            return hueIPs;
        }
    }
}

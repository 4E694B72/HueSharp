using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HueSharp.BridgeManagement.Discovery
{
    [Obsolete("SSDP method is deprecated in favor of mDNS and will be disabled in Q2 2022 by Signify.")]
    public class SSDPAutoBridgeDiscover : IAutoBridgeDiscover
    {
        protected static int DISCOVERY_TIMEOUT_MILLISECONDS = 30000;

        /// <summary>
        /// This method discovers Hue bridges via SSDP by sending UDP packets over IPv4 the multicast address. Search takes 30 seconds.
        /// </summary>
        /// <returns>A list of IPs as string</returns>
        public async Task<List<string>> GetHuesInNetwork()
        {
            var hueIPs = new List<string>();

            // wake me up when IPv4 ends - Green Day, 2003
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                var multicastEndpoint = new IPEndPoint(IPAddress.Broadcast, 1900);
                string req = "M-SEARCH * HTTP/1.1\r\n" +
                           $"HOST: {multicastEndpoint}\r\n" +
                           "ST:upnp:rootdevice\r\n" +
                           "MAN:\"ssdp:discover\"\r\n" +
                           "MX:3\r\n\r\n";

                var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(req));
                for (var i = 0; i < 3; i++)
                {
                    await socket.SendToAsync(data, SocketFlags.None, multicastEndpoint);
                }
                var receiveTask = ReceiveAsync(socket, new ArraySegment<byte>(new byte[4096]), hueIPs);
                // keep in mind that a larger network might need a higher time out, possibly that this implementation needs to be changed when used in a lager setting
                await Task.WhenAny(receiveTask, Task.Delay(DISCOVERY_TIMEOUT_MILLISECONDS));
            }
            return hueIPs;
        }

        protected async Task ReceiveAsync(Socket socket, ArraySegment<byte> buffer, ICollection<string> responses)
        {
            while (true)
            {
                var i = await socket.ReceiveAsync(buffer, SocketFlags.None);
                if (i > 0)
                {
                    string response = Encoding.UTF8.GetString(buffer.Take(i).ToArray());
                    // to eliminate the need of downloading every description xml and check for modelName
                    if (response.IndexOf("hue-bridgeid:") > -1)
                    {
                        Regex rx = new Regex(@"http:\/\/(\d+.\d+.\d+.\d+)");
                        Match match = rx.Match(response);
                        responses.Add(match.Groups[1].Value);
                    }
                }
            }
        }
    }
}

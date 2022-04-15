using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueSharp.BridgeManagement.Discovery
{
    public interface IAutoBridgeDiscover
    {
        public Task<List<string>> GetHuesInNetwork();
    }
}

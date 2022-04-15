using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Owner
    {
        [JsonProperty("rid")]
        public string ID { get; set; }

        [JsonProperty("rowner")]
        public string Type { get; set; }
    }
}

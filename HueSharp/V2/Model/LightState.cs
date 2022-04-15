using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class LightState
    {
        [JsonProperty("on")]
        public bool On { get; set; }
    }
}

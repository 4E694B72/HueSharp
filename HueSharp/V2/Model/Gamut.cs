using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Gamut
    {
        [JsonProperty("red")]
        public XY Red { get; set; }

        [JsonProperty("green")]
        public XY Green { get; set; }

        [JsonProperty("blue")]
        public XY Blue { get; set; }

        [JsonProperty("gammut_type")]
        public string Type { get; set; }
    }
}

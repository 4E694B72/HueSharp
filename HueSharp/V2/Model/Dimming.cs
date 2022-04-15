using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Dimming
    {
        [JsonProperty("brightness")]
        public double? Brightness { get; set; }

        [JsonProperty("min_dim_level")]
        public double? MinDimLevel { get; set; }
    }
}

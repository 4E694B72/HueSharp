using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Color
    {
        [JsonProperty("xy")]
        public XY CIE_XY { get; set; }

        [JsonProperty("gamut")]
        public Gamut Gamut { get; set; }
    }
}

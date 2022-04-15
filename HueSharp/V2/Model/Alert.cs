using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Alert
    {
        [JsonProperty("action_values")]
        public string[] Values { get; set; }
    }
}

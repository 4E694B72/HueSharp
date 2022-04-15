using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Metadata
    {
        [JsonProperty("archetype")]
        public string ArcheType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

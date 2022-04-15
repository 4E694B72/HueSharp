using Newtonsoft.Json;

namespace HueSharp.V2.Model
{
    public class LightEffect
    {
        [JsonProperty("effect")]
        public string Effect { get; set; }
    }
}

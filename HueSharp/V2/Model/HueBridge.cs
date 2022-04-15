using Newtonsoft.Json;

namespace HueSharp.V2
{
    public class HueBridge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("id_v1")]
        public string IDv1 { get; set; }

        [JsonProperty("bridge_id")]
        public string BridgeID { get; set; }

    }
}

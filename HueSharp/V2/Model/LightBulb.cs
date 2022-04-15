using HueSharp.Command;
using Newtonsoft.Json;

namespace HueSharp.V2.Model
{
    public class LightBulb
    {
        private string _idV1;

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("id_v1")]
        public string IDv1
        {
            get
            {
                return _idV1;
            }
            set
            {
                _idV1 = value?.Replace("/lights/", "");
            }
        }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("on")]
        public LightState State { get; set; }

        [JsonProperty("dimming")]
        public Dimming Dimming { get; set; }

        [JsonProperty("color_temperature")]
        public ColorTemperature Temperature { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        [JsonProperty("dynamics")]
        public Dynamics Dynamics { get; set; }

        [JsonProperty("alert")]
        public Alert Alert { get; set; }

        [JsonProperty("effects")]
        public LightEffect Effect { get; set; }


        public static implicit operator LightBulb(LightBulbCommand command)
        {
            LightBulb bulb = new LightBulb();
            if(command.On != null)
            {
                bulb.State = new LightState()
                {
                    On = (bool)command.On
                };
            }
            if(command.Brightness != null)
            {
                bulb.Dimming = new Dimming()
                {

                    Brightness = command.Brightness <= 100 ? command.Brightness : 100
                };
            }
            if(command.Color != null)
            {
                bulb.Color = new Color()
                {
                    CIE_XY = new XY(command.Color[0], command.Color[1])
                };
            }
            if(command.ColorTemperature != null)
            {
                bulb.Temperature = new ColorTemperature()
                {
                    Mirek = (short)command.ColorTemperature
                };
            }
            if(command.TransitionTime != null)
            {
                bulb.Dynamics = new Dynamics()
                {
                    Speed = (double)command.TransitionTime
                };
            }
            if(command.Alert != null)
            {
                bulb.Alert = new Alert()
                {
                    Values = new string[] { command.Alert }
                };
            }
            if(command.Effect != null)
            {
                bulb.Effect = new LightEffect()
                {
                    Effect = command.Effect
                };
            }

            return bulb;
        }
    }
}

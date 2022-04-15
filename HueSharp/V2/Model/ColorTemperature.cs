using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class ColorTemperature
    {
        private short _mirek;

        [JsonProperty("mirek")]
        public short Mirek
        {
            get
            {
                return _mirek;
            }
            set
            {
                if(value < 153)
                {
                    _mirek = 153;
                }
                else if(value > 500)
                {
                    _mirek = 500;
                }
                else
                {
                    _mirek = value;
                }
            }
        }

        [JsonProperty("mirek_valid")]
        public bool Valid { get; set; }

        [JsonProperty("mirek_schema")]
        public MirekSchema MirekSchema { get; set; }
    }
}

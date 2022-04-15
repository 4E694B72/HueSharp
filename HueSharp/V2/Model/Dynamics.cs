using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class Dynamics
    {
        private double _speed;

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_values")]
        public string[] Possible_Values { get; set; }

        [JsonProperty("speed")]
        public double Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if(value > 1)
                {
                    _speed = 1;
                }
                else if(value < 0)
                {
                    _speed = 0;
                }
                else
                {
                    _speed = value;
                }
            }
        }

        [JsonProperty("speed_valid")]
        public bool Valid { get; set; }
    }
}

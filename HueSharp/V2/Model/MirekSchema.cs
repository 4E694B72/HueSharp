using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class MirekSchema
    {
        private short _minimum;
        private short _maximum;

        [JsonProperty("mirek_minimum")]
        public short Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                if(value < 153)
                {
                    _minimum = 153;
                }
                else if(value > 500)
                {
                    _minimum = 500;
                }
                else
                {
                    _minimum = value;
                }
            }
        }

        [JsonProperty("mirek_maximum")]
        public short Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                if(value < 153)
                {
                    _maximum = 153;
                }
                else if(value > 500)
                {
                    _maximum = 500;
                }
                else
                {
                    _maximum = value;
                }
            }
        }
    }
}

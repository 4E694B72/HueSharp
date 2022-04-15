using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueSharp.V2.Model
{
    public class XY
    {
        private double _x;
        private double _y;

        public XY()
        {

        }

        public XY(double x, double y)
        {
            _x = x;
            _y = y;
        }

        [JsonProperty("x")]
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if(value > 1)
                {
                    _x = 1;
                }
                else if(value < 0)
                {
                    _x = 0;
                }
                else
                {
                    _x = value;
                }
            }
        }

        [JsonProperty("y")]
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if(value > 1)
                {
                    _y = 1;
                }
                else if(value < 0)
                {
                    _y = 0;
                }
                else
                {
                    _y = value;
                }
            }
        }
    }
}

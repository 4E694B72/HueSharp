using System.Collections.Generic;

namespace HueSharp.Command
{
    public class LightBulbCommand
    {
        public bool? On { get; set; }

        public byte? Brightness { get; set; }

        // Based on CIE 1931 color coordinates
        public double[]? Color { get; set; }

        public short? ColorTemperature { get; set; }

        public byte? Saturation { get; set; }

        public double? TransitionTime { get; set; }

        public string Alert { get; set; }

        public string Effect { get; set; }

        public List<string> Bulbs { get; set; }

        public List<string> Groups { get; set; }

    }
}

using System;
using System.Drawing;

namespace HueSharp.Command
{
    public static class ColorExtension
    {
        public static LightBulbCommand SetColor(this LightBulbCommand command, float red, float green, float blue)
        {
            float[] normalizedCoordinate = new float[3];

            normalizedCoordinate[0] = red / 255;
            normalizedCoordinate[1] = green / 255;
            normalizedCoordinate[2] = blue / 255;

            float cred, cgreen, cblue;

            if (normalizedCoordinate[0] > 0.04045)
            {
                cred = (float)Math.Pow((normalizedCoordinate[0] + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                cred = (float)(normalizedCoordinate[0] / 12.92);
            }

            // Make green more vivid
            if (normalizedCoordinate[1] > 0.04045)
            {
                cgreen = (float)Math.Pow((normalizedCoordinate[1] + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                cgreen = (float)(normalizedCoordinate[1] / 12.92);
            }

            // Make blue more vivid
            if (normalizedCoordinate[2] > 0.04045)
            {
                cblue = (float)Math.Pow((normalizedCoordinate[2] + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                cblue = (float)(normalizedCoordinate[2] / 12.92);
            }

            float X = (float)(cred * 0.649926 + cgreen * 0.103455 + cblue * 0.197109);
            float Y = (float)(cred * 0.234327 + cgreen * 0.743075 + cblue * 0.022598);
            float Z = (float)(cred * 0.0000000 + cgreen * 0.053077 + cblue * 1.035763);

            float x = X / (X + Y + Z);
            float y = Y / (X + Y + Z);

            command.Color = new double[] { x, y };
            return command;
        }

        public static LightBulbCommand SetColorHSV(this LightBulbCommand command, float hue, float saturation, float brightness)
        {
            float sat = saturation / 100;
            float bri = brightness / 100;
            float C = sat * bri;
            float X = C * (1 - Math.Abs((hue / 60 % 2) - 1));
            float m = bri - C;
            float r, g, b;

            if (hue >= 0 && hue < 60)
            {
                r = C;
                g = X;
                b = 0;
            }
            else if (hue >= 60 && hue < 120)
            {
                r = X;
                g = C;
                b = 0;
            }
            else if (hue >= 120 && hue < 180)
            {
                r = 0;
                g = C;
                b = X;
            }
            else if (hue >= 180 && hue < 240)
            {
                r = 0;
                g = X;
                b = C;
            }
            else if (hue >= 240 && hue < 300)
            {
                r = X;
                g = 0;
                b = C;
            }
            else
            {
                r = C;
                g = 0;
                b = X;
            }

            float red = r + m;
            float green = g + m;
            float blue = b + m;

            SetColor(command, red, green, blue);

            return command;
        }

        public static LightBulbCommand SetColor(this LightBulbCommand command, Color color)
        {
            SetColor(command, color.R, color.G, color.B);
            return command;
        }

        public static LightBulbCommand SetColor(this LightBulbCommand command, string hex)
        {
            var color = ColorTranslator.FromHtml(hex);
            return SetColor(command, color);
        }

    }
}

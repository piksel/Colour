
using System;

namespace Piksel.Graphics.ColourSpaces
{

    /// <summary>
    /// Provides color conversion functionality.
    /// </summary>

    public static class ColourConverter
    {

        /// <summary>
        /// Converts a Color to RGB.
        /// </summary>
        /// <param name="color">A Color object representing the color that is
        /// to be converted to RGB.</param>
        /// <returns>A RGB equivalent.</returns>

        public static RGB ColourToRgb(Colour color)
        {
            return new RGB(color.Red, color.Green, color.Blue);
        }


        /// <summary>
        /// Converts a RGB color structure to a Color object.
        /// </summary>
        /// <param name="rgb">A RGB object representing the color that is to be
        /// converted.</param>
        /// <returns>A Color equivalent.</returns>

        public static Colour RgbToColour(RGB rgb)
        {
            return new Colour(rgb.Red, rgb.Green, rgb.Blue);
        }


        /// <summary>
        /// Converts RGB to HSB.
        /// </summary>
        /// <param name="rgb">A RGB object containing the RGB values that are to 
        /// be converted to HSB values.</param>
        /// <returns>A HSB equivalent.</returns>

        public static HSB RgbToHsb(RGB rgb)
        {

            var r = rgb.Red / 255d;
            var g = rgb.Green / 255d;
            var b = rgb.Blue / 255d;

            var minValue = GetMinimumValue(r, g, b);
            var maxValue = GetMaximumValue(r, g, b);
            var delta = maxValue - minValue;

            var hue = 0d;
            var saturation = 0d;
            var brightness = maxValue * 100;

            if (maxValue == 0 || delta == 0)
            {

                hue = 0;
                saturation = 0;

            }
            else
            {

                if (minValue == 0)
                {
                    saturation = 100;
                }
                else
                {
                    saturation = (delta / maxValue) * 100;
                }

                if (Math.Abs(r - maxValue) < double.Epsilon)
                {
                    hue = (g - b) / delta;
                }
                else if (Math.Abs(g - maxValue) < double.Epsilon)
                {
                    hue = 2 + (b - r) / delta;
                }
                else if (Math.Abs(b - maxValue) < double.Epsilon)
                {
                    hue = 4 + (r - g) / delta;
                }

            }

            hue *= 60;
            if (hue < 0)
            {
                hue += 360;
            }

            return new HSB(
                (ushort)Math.Round(hue),
                (byte)Math.Round(saturation),
                (byte)Math.Round(brightness));

        } // RgbToHsb


        /// <summary>
        /// Converts HSB to RGB.
        /// </summary>
        /// <param name="rgb">A HSB object containing the HSB values that are to 
        /// be converted to RGB values.</param>
        /// <returns>A RGB equivalent.</returns>

        public static RGB HsbToRgb(HSB hsb)
        {

            double h, s, b;
            double red = 0, green = 0, blue = 0;

            h = hsb.Hue % 360;
            s = ((double)hsb.Saturation) / 100;
            b = ((double)hsb.Brightness) / 100;

            if (s == 0)
            {

                red = b;
                green = b;
                blue = b;

            }
            else
            {

                double p, q, t;

                // the color wheel has six sectors.
                double fractionalSector;
                int sectorNumber;
                double sectorPosition;

                sectorPosition = h / 60;
                sectorNumber = (int)Math.Floor(sectorPosition);
                fractionalSector = sectorPosition - sectorNumber;

                p = b * (1 - s);
                q = b * (1 - (s * fractionalSector));
                t = b * (1 - (s * (1 - fractionalSector)));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        red = b;
                        green = t;
                        blue = p;
                        break;

                    case 1:
                        red = q;
                        green = b;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = b;
                        blue = t;
                        break;

                    case 3:
                        red = p;
                        green = q;
                        blue = b;
                        break;

                    case 4:
                        red = t;
                        green = p;
                        blue = b;
                        break;

                    case 5:
                        red = b;
                        green = p;
                        blue = q;
                        break;
                }

            }

            byte nRed = (byte)Math.Min(255, Math.Round(red * 255));
            byte nGreen = (byte)Math.Round(green * 255);
            byte nBlue = (byte)Math.Round(blue * 255);

            return new RGB(nRed, nGreen, nBlue);

        } // HsbToRgb


        /// <summary>
        /// Determines the maximum value of all of the numbers provided in the
        /// variable argument list.
        /// </summary>
        /// <param name="values">An array of doubles.</param>
        /// <returns>The maximum value.</returns>

        public static double GetMaximumValue(params double[] values)
        {

            double maxValue = values[0];

            if (values.Length >= 2)
            {

                double num;

                for (int i = 1; i < values.Length; i++)
                {

                    num = values[i];
                    maxValue = Math.Max(maxValue, num);

                }

            }

            return maxValue;

        } // GetMaximumValue


        /// <summary>
        /// Determines the minimum value of all of the numbers provided in the
        /// variable argument list.
        /// </summary>
        /// <param name="values">An array of doubles.</param>
        /// <returns>The minimum value.</returns>

        public static double GetMinimumValue(params double[] values)
        {

            double minValue = values[0];

            if (values.Length >= 2)
            {

                double num;

                for (int i = 1; i < values.Length; i++)
                {

                    num = values[i];
                    minValue = Math.Min(minValue, num);

                }

            }

            return minValue;

        } // GetMinimumValue




        /// <summary>
        /// Creates a RGB object based on the parameterized hexadecimal
        /// string value.
        /// </summary>
        /// <param name="hexValue">A string object representing the hexadecimal 
        /// value.</param>
        /// <returns>A Color equivalent.</returns>

        public static RGB HexToRgb(string hexValue)
        {

            var c = Colour.FromHex(hexValue);
            return new RGB(c.Red, c.Green, c.Blue);

        }

    }

}

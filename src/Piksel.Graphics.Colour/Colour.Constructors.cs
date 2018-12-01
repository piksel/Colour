using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Piksel.Graphics
{
    public partial struct Colour
    {
        private const string RgbaPattern = 
            @"((?:rgba)?)\(?([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)(?:\s*,\s*((?:0?\.)?[0-9]+|1\.0+))?\)";

        private static readonly NumberFormatInfo FloatFormat = new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        };

        public Colour(byte red, byte green, byte blue)
            => value = Opaque
            | ((uint)red << RedShift)
            | ((uint)green << GreenShift)
            | ((uint)blue << BlueShift);

        public Colour(byte red, byte green, byte blue, byte alpha)
            => value = ((uint)alpha << AlphaShift)
            | ((uint)red << RedShift)
            | ((uint)green << GreenShift)
            | ((uint)blue << BlueShift);

        private Colour(uint argb)
            => value = argb & ValueMask;

        public static Colour FromArgb(uint argb)
            => new Colour(argb);

        public static Colour FromArgb(byte alpha, byte red, byte green, byte blue)
            => new Colour(red, green, blue, alpha);

        public static Colour FromRgba(byte red, byte green, byte blue, byte alpha)
            => new Colour(red, green, blue, alpha);

        public static Colour FromRgba(uint rgba)
            => new Colour(((rgba >> 8) & ValueMask) | (rgba << AlphaShift));

        /// <summary>
        /// Parses a colour in the rgba(RRR, GGG, BBB, A.A) format. Prefix and parantheses may be omitted. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="strict">If not set this will allow number outside the byte range, clamping them to 0-255</param>
        /// <exception cref="ArgumentException">Thrown if the input string does not match the correct syntax</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="strict"/> is set and the channel bytes are outside the byte range (0-255)</exception>
        /// <returns></returns>
        public static Colour FromRgbaString(string input, bool strict = false)
        {
            var match = Regex.Match(input, RgbaPattern, RegexOptions.IgnoreCase);
            if (!match.Success) throw new ArgumentException($"Invalid colour string \"{input}\"");

            var red = match.Groups[2].Value;
            if (!ParseByte(red, out byte r) && strict)
                throw new ArgumentOutOfRangeException(nameof(red));

            var green = match.Groups[3].Value;
            if (!ParseByte(green, out byte g) && strict)
                throw new ArgumentOutOfRangeException(nameof(green));

            var blue = match.Groups[4].Value;
            if (!ParseByte(blue, out byte b) && strict)
                throw new ArgumentOutOfRangeException(nameof(blue));

            var alpha = match.Groups[5].Value;

            if (string.IsNullOrEmpty(alpha))
            {
                return new Colour(r, g, b);
            }
            else
            {
                byte a;
                if (alpha.IndexOf('.') >= 0)
                {
                    a = (byte)Math.Round(float.Parse(alpha, NumberStyles.Float, FloatFormat) * 255, 0);
                }
                else {
                    if (!ParseByte(alpha, out a) && strict)
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                }
                return new Colour(r, g, b, a);
            }
        }

        /// <summary>
        /// Parses a base 10 number string as a byte, clamping it to 0 - 255
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns>Returns whether the value was in the valid range (0 - 255)</returns>
        private static bool ParseByte(string input, out byte output)
        {
            var val = ushort.Parse(input);
            if (val > 255)
            {
                output = 255;
                return false;
            }
            else {
                output = (byte)val;
                return true;
            }
        }

        public static Colour FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentNullException(nameof(hex));

            var length = hex.Length;
            var pos = 0;

            if (hex[0] == '#')
                length -= ++pos;
            else if (hex[0] == '0' || hex[1] == 'x')
                length -= (pos += 2);

            byte r, g, b, a;

            switch (length)
            {
                case 2:
                    b = (byte)(Convert.ToByte(hex.Substring(pos, 1), 16) * 0x11);
                    return new Colour(b, b, b);

                case 3:
                case 6:
                case 8:
                    var size = length == 3 ? 1 : 2;
                    r = Convert.ToByte(hex.Substring(pos, size), 16);
                    g = Convert.ToByte(hex.Substring(pos += size, size), 16);
                    b = Convert.ToByte(hex.Substring(pos += size, size), 16);
                    if (length < 8)
                        return new Colour(r, g, b);
                    a = Convert.ToByte(hex.Substring(pos += size, size), 16);
                    return new Colour(r, g, b, a);

                default:
                    throw new ArgumentException("Invalid string length, only 2, 3, 6 or 8 hex characters are supported");
            }
        }
    }
}

using System;
using System.Drawing;
using System.Text;

namespace Piksel.Graphics
{
    public struct Colour
    {
        const uint Opaque = 0xff000000;
        const uint Transparent = 0x000000;

        const uint ValueMask = 0xffffffff;
        const uint RedMask = 0xff00ffff;
        const uint GreenMask = 0xffff00ff;
        const uint BlueMask = 0xffffff00;
        const uint AlphaMask = 0x00ffffff;

        const int AlphaShift = 24;
        const int RedShift = 16;
        const int GreenShift = 8;
        const int BlueShift = 0;

        uint value;

        public byte Red => (byte)(value >> RedShift);
        public byte Green => (byte)(value >> GreenShift);
        public byte Blue => (byte)(value >> BlueShift);
        public byte Alpha => (byte)(value >> AlphaShift);

        public Colour WithRed(byte red)
            => new Colour((value & RedMask) | ((uint)red << RedShift));

        public Colour WithGreen(byte red)
            => new Colour((value & GreenMask) | ((uint)red << GreenShift));

        public Colour WithBlue(byte blue)
            => new Colour((value & BlueMask) | ((uint)blue << BlueShift));

        public Colour WithAlpha(byte alpha)
            => new Colour((value & AlphaMask) | ((uint)alpha << AlphaShift));

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

        public uint ToArgb()
            => value;

        public uint ToRgba()
            => (value >> AlphaShift) | (value << 8);

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
            
            switch (length) {
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

        public string ToHex(HexPrefix prefix = HexPrefix.Hash, HexFormatAlpha alpha = HexFormatAlpha.Auto)
        {
            var sb = new StringBuilder(10);

            if (prefix == HexPrefix.Hash) sb.Append('#');
            else if (prefix == HexPrefix.Ox) sb.Append("0x");

            sb.Append(Convert.ToString(value & AlphaMask, 16));

            if (alpha == HexFormatAlpha.Always 
                || (alpha == HexFormatAlpha.Auto && (value & Opaque) != Opaque))
                sb.Append(Convert.ToString(value >> AlphaShift, 16));

            return sb.ToString();
        }

        public override string ToString()
            => ToHex(HexPrefix.Hash, HexFormatAlpha.Auto);

        public static implicit operator Color(Colour colour)
            => Color.FromArgb(unchecked((int)colour.value));

        public static implicit operator Colour(Color color)
            => Colour.FromArgb(unchecked((uint)color.ToArgb()));
    }
}

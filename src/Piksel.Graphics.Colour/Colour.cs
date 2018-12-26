using System;
using System.Text;

namespace Piksel.Graphics
{
    public partial struct Colour
    {
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



        public uint ToArgb()
            => value;

        public uint ToRgba()
            => (value >> AlphaShift) | (value << 8);

        public static Colour FromString(string input)
            => input[0] == '#' ? FromHex(input) : FromRgbaString(input);



        public string ToHex(HexPrefix prefix = HexPrefix.Hash, HexFormatAlpha alpha = HexFormatAlpha.Auto)
        {
            var sb = new StringBuilder(10);

            if (prefix == HexPrefix.Hash) sb.Append('#');
            else if (prefix == HexPrefix.Ox) sb.Append("0x");

            sb.Append(Convert.ToString(value & AlphaMask, 16).PadLeft(6, '0'));

            if (alpha == HexFormatAlpha.Always
                || (alpha == HexFormatAlpha.Auto && (value & Opaque) != Opaque))
                sb.Append(Convert.ToString(value >> AlphaShift, 16));

            return sb.ToString();
        }

        public override string ToString()
            => ToHex(HexPrefix.Hash, HexFormatAlpha.Auto);

    }
}

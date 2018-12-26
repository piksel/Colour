using System;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    public static class StaticFunctions
    {
        public static void Clamp<T>(ref T value, T min, T max) where T : IComparable
        {
            value = value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : value);
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
            => value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : value);

        public static byte ClampByte(this int value)
            => (byte)Math.Max(0, Math.Min(byte.MaxValue, value));

        public static byte ClampByte(this double value)
            => (byte)Math.Max(0, Math.Min(byte.MaxValue, value));
    }
}

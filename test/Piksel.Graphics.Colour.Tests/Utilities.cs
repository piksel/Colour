using NUnit.Framework;
using System;
using Piksel.Graphics;
using System.Drawing;

namespace Piksel.Graphics.Tests
{
    public static class Utilities
    {
        public static void AssertColour(Colour colour, byte red, byte green, byte blue, byte alpha, string message = "Colour component {0}")
        {
            Assert.AreEqual(red, colour.Red, message, nameof(red));
            Assert.AreEqual(green, colour.Green, message, nameof(green));
            Assert.AreEqual(blue, colour.Blue, message, nameof(blue));
            Assert.AreEqual(alpha, colour.Alpha, message, nameof(alpha));
        }

        public static void AssertColor(Color color, byte red, byte green, byte blue, byte alpha, string message = "Color component {0}")
        {
            Assert.AreEqual(red, color.R, message, nameof(red));
            Assert.AreEqual(green, color.G, message, nameof(green));
            Assert.AreEqual(blue, color.B, message, nameof(blue));
            Assert.AreEqual(alpha, color.A, message, nameof(alpha));
        }
    }
}

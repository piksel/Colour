using NUnit.Framework;
using System;
using System.Drawing;

namespace Piksel.Graphics.Tests
{
    using static Utilities;

    [TestFixture]
    public class Compability
    {


        [Test]
        public void ColorCompability()
        {
            byte r = 255, b = 64, g = 32, a = 128;
            var co = Color.FromArgb(a, r, g, b);
            var cu = new Colour(r, g, b, a);

            AssertColour(cu, co.R, co.G, co.B, co.A);
        }

        [Test]
        public void CastingToColor()
        {
            var cu = new Colour(0xff, 0x00, 0x00);
            var cuco = (Color)cu;

            AssertColor(cuco, cu.Red, cu.Green, cu.Blue, cu.Alpha);
        }

    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Piksel.Graphics.Tests
{
    using static Utilities;


    [TestFixture]
    [Category("RgbaStrings")]
    [Category("Deserialization")]
    public class RgbaStringDeserialization
    {
        [Test]
        public void RgbaWithNoAlpha() => AssertColour(
            Colour.FromRgbaString("rgba(255,32,128)"), 255, 32, 128, 255);

        [Test]
        public void RgbaWithByteAlpha() => AssertColour(
            Colour.FromRgbaString("rgba(255,32,128,64)"), 255, 32, 128, 64);

        [Test]
        public void RgbaWithFloatAlpha() => AssertColour(
            Colour.FromRgbaString("rgba(255,0,128,0.5)"), 255, 0, 128, 128);
    }
}

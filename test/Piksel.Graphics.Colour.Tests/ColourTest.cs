using NUnit.Framework;
using System;
using System.Drawing;

namespace Piksel.Graphics.Tests
{
    [TestFixture]
    public class ColourTest
    {

        [Test]
        public void ColorCompability()
        {
            var co = Color.Red;
            var cu = new Colour(0xff, 0x00, 0x00);

            Assert.AreEqual(co.R, cu.Red);
            Assert.AreEqual(co.G, cu.Green);
            Assert.AreEqual(co.B, cu.Blue);

            var cuco = (Color)cu;

            Assert.AreEqual(co.R, cuco.R);
            Assert.AreEqual(co.G, cuco.G);
            Assert.AreEqual(co.B, cuco.B);
        }

        [Test]
        public void HexDeserialization()
        {
  
            Assert.AreEqual("deadbeef", Colour.FromHex("0xdeadbeef").ToRgba().ToString("x"),
                "0x prefix with alpha");

            Assert.AreEqual("deadbeff", Colour.FromHex("0xdeadbe").ToRgba().ToString("x"),
                "0x prefix without alpha");

            Assert.AreEqual("98765432", Colour.FromHex("#98765432").ToRgba().ToString("x"),
                "Hash prefix with alpha");

            Assert.AreEqual("987654ff", Colour.FromHex("#987654").ToRgba().ToString("x"),
                "Hash prefix without alpha");

        }

        [Test]
        public void HexSerialization()
        {
            Assert.AreEqual("#12345678", new Colour(0x12, 0x34, 0x56, 0x78).ToString(), 
                "Default .ToString should return a hash followed by 8 lower case hex characters if the colour is not opaque");

            Assert.AreEqual("#123456", new Colour(0x12, 0x34, 0x56).ToString(),
                "Default .ToString should return a hash followed by 6 lower case hex characters if the colour is opaque");
        }
    }
}

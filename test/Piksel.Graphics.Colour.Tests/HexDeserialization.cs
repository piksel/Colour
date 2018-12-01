using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Piksel.Graphics.Tests
{
    using static Utilities;

    [TestFixture]
    [Category("HexStrings")]
    [Category("Deserialization")]
    public class HexDeserialization
    {

        [Test]
        [Description("")]
        public void OxFormatWithAlpha() => 
            Assert.AreEqual("deadbeef", Colour.FromHex("0xdeadbeef").ToRgba().ToString("x"),
                "0x prefix with alpha");

        [Test]
        [Description("")]
        public void OxFormatWithoutAlpha() => 
            Assert.AreEqual("deadbeff", Colour.FromHex("0xdeadbe").ToRgba().ToString("x"),
                "0x prefix without alpha");

        [Test]
        [Description("")]
        public void HashFormatWithAlpha() => 
            Assert.AreEqual("98765432", Colour.FromHex("#98765432").ToRgba().ToString("x"),
                "Hash prefix with alpha");
        [Test]
        [Description("")]
        public void HashFormatWithoutAlpha() => 
            Assert.AreEqual("987654ff", Colour.FromHex("#987654").ToRgba().ToString("x"),
                "Hash prefix without alpha");

    }
}

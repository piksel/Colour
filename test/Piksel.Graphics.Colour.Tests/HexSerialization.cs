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
    public class HexSerialization
    {
        [Test]
        [Description("Default .ToString should return a hash followed by " +
            "8 lower case hex characters if the colour is not opaque")]
        public void DefaultToStringNonOpaque() =>
            Assert.AreEqual("#12345678", new Colour(0x12, 0x34, 0x56, 0x78).ToString());

        [Test]
        [Description("Default .ToString should return a hash followed by " +
            "6 lower case hex characters if the colour is opaque")]
        public void DefaultToStringOpaque() =>
             Assert.AreEqual("#123456", new Colour(0x12, 0x34, 0x56).ToString());

    }
}

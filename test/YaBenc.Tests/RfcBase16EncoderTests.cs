using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaBenc.Tests
{
    [TestFixture]
    public class RfcBase16EncoderTests
    {
        private readonly RfcBase16Encoder encoder = new RfcBase16Encoder();

        [Test]
        public void Encodes_Test_Vectors()
        {
            Assert.AreEqual(encoder.Encode(""), "");
            Assert.AreEqual(encoder.Encode("f"), "66");
            Assert.AreEqual(encoder.Encode("fo"), "666F");
            Assert.AreEqual(encoder.Encode("foo"), "666F6F");
            Assert.AreEqual(encoder.Encode("foob"), "666F6F62");
            Assert.AreEqual(encoder.Encode("fooba"), "666F6F6261");
            Assert.AreEqual(encoder.Encode("foobar"), "666F6F626172");
        }

        [Test]
        public void Decodes_Test_Vectors()
        {
            Assert.AreEqual(encoder.Decode(""), "");
            Assert.AreEqual(encoder.Decode("66"), "f");
            Assert.AreEqual(encoder.Decode("666F"), "fo");
            Assert.AreEqual(encoder.Decode("666F6F"), "foo");
            Assert.AreEqual(encoder.Decode("666F6F62"), "foob");
            Assert.AreEqual(encoder.Decode("666F6F6261"), "fooba");
            Assert.AreEqual(encoder.Decode("666F6F626172"), "foobar");
        }
    }
}

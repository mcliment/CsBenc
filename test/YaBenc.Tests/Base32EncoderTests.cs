using NUnit.Framework;

namespace YaBenc.Tests
{
    [TestFixture]
    public class Base32EncoderTests
    {
        [Test]
        public void Encodes()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Encode(0), "0");
            Assert.AreEqual(encoder.Encode(1234), "16J");
            Assert.AreEqual(encoder.Encode(ulong.MaxValue), "FZZZZZZZZZZZZ");
        }

        [Test]
        public void Encodes_With_Checksum()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Encode(0, true), "00");
            Assert.AreEqual(encoder.Encode(1234, true), "16JD");
            Assert.AreEqual(encoder.Encode(ulong.MaxValue, true), "FZZZZZZZZZZZZB");
        }

        [Test]
        public void Decodes()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Decode("16J"), 1234);
        }

        [Test]
        public void Decodes_With_Checksum()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Decode("16JD", true), 1234);
        }
    }
}

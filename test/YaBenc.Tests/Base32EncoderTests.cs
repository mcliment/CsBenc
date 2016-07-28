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

            Assert.AreEqual(encoder.Encode(1234), "16J");
        }

        [Test]
        public void Encodes_With_Checksum()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Encode(1234, true), "16JD");
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

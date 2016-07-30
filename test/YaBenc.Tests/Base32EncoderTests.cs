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

            Assert.AreEqual(encoder.Decode("0"), 0);
            Assert.AreEqual(encoder.Decode("16J"), 1234);
            Assert.AreEqual(encoder.Decode("FZZZZZZZZZZZZ"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_Lowercase()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Decode("16j"), 1234);
            Assert.AreEqual(encoder.Decode("fzzzzzzzzzzzz"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Separator()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Decode("16-J"), 1234);
            Assert.AreEqual(encoder.Decode("FZZ-ZZZ-ZZZ-ZZZZ"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Checksum()
        {
            var encoder = new Base32Encoder();

            Assert.AreEqual(encoder.Decode("16JD", true), 1234);
        }

        [Test]
        [Explicit]
        public void Random_Invert([Random(1000)] ulong input)
        {
            var encoder = new Base32Encoder();

            var encoded = encoder.Encode(input);
            var decoded = encoder.Decode(encoded);

            Assert.That(input, Is.EqualTo(decoded));
        }
    }
}

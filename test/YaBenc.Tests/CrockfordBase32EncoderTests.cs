using NUnit.Framework;

namespace YaBenc.Tests
{
    [TestFixture]
    public class CrockfordBase32EncoderTests
    {
        private readonly CrockfordBase32Encoder encoder = new CrockfordBase32Encoder();

        [Test]
        public void Encodes()
        {
            Assert.AreEqual(encoder.Encode(0), "0");
            Assert.AreEqual(encoder.Encode(1234), "16J");
            Assert.AreEqual(encoder.Encode(ulong.MaxValue), "FZZZZZZZZZZZZ");
        }

        [Test]
        public void Encodes_With_Checksum()
        {
            Assert.AreEqual(encoder.Encode(0, true), "00");
            Assert.AreEqual(encoder.Encode(1234, true), "16JD");
            Assert.AreEqual(encoder.Encode(ulong.MaxValue, true), "FZZZZZZZZZZZZB");
        }

        [Test]
        public void Decodes()
        {
            Assert.AreEqual(encoder.Decode("0"), 0);
            Assert.AreEqual(encoder.Decode("16J"), 1234);
            Assert.AreEqual(encoder.Decode("FZZZZZZZZZZZZ"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_Lowercase()
        {
            Assert.AreEqual(encoder.Decode("16j"), 1234);
            Assert.AreEqual(encoder.Decode("fzzzzzzzzzzzz"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Separator()
        {
            Assert.AreEqual(encoder.Decode("16-J"), 1234);
            Assert.AreEqual(encoder.Decode("FZZ-ZZZ-ZZZ-ZZZZ"), ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Checksum()
        {
            Assert.AreEqual(encoder.Decode("16JD", true), 1234);
        }

        [Test]
        public void Random_Invert([Random(10)] ulong input)
        {
            var encoded = encoder.Encode(input);
            var decoded = encoder.Decode(encoded);

            Assert.That(input, Is.EqualTo(decoded));
        }
    }
}

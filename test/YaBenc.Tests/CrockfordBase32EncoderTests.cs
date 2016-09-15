using NUnit.Framework;
using Shouldly;

namespace YaBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class CrockfordBase32EncoderTests
    {
        private readonly CrockfordBase32Encoder encoder = new CrockfordBase32Encoder();

        [TestCase(0UL, "0")]
        [TestCase(1234UL, "16J")]
        [TestCase(ulong.MaxValue, "FZZZZZZZZZZZZ")]
        public void Encodes(ulong value, string encoded)
        {
            encoder.Encode(value).ShouldBe(encoded);
        }

        [TestCase(0UL, "00")]
        [TestCase(1234UL, "16JD")]
        [TestCase(ulong.MaxValue, "FZZZZZZZZZZZZB")]
        public void Encodes_With_Checksum(ulong value, string encoded)
        {
            encoder.Encode(value, true).ShouldBe(encoded);
        }

        [TestCase("0", 0UL)]
        [TestCase("16J", 1234UL)]
        [TestCase("FZZZZZZZZZZZZ", ulong.MaxValue)]
        public void Decodes(string encoded, ulong decoded)
        {
            encoder.Decode(encoded).ShouldBe(decoded);
        }

        [TestCase("16j", 1234UL)]
        [TestCase("fzzzzzzzzzzzz", ulong.MaxValue)]
        public void Decodes_Lowercase(string encoded, ulong decoded)
        {
            encoder.Decode(encoded).ShouldBe(decoded);
        }

        [TestCase("16-J", 1234UL)]
        [TestCase("FZZ-ZZZ-ZZZ-ZZZZ", ulong.MaxValue)]
        public void Decodes_With_Separator(string encoded, ulong decoded)
        {
            encoder.Decode(encoded).ShouldBe(decoded);
        }

        [TestCase("16JD", 1234UL)]
        [TestCase("FZZZZZZZZZZZZB", ulong.MaxValue)]
        public void Decodes_With_Checksum(string encoded, ulong decoded)
        {
            encoder.Decode(encoded, true).ShouldBe(decoded);
        }

        [Test]
        public void Random_Invert([Random(10)] ulong input)
        {
            var encoded = encoder.Encode(input);
            var decoded = encoder.Decode(encoded);

            input.ShouldBe(decoded);
        }
    }
}

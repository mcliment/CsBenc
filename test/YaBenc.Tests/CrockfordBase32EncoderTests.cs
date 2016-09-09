using NUnit.Framework;
using Shouldly;

namespace YaBenc.Tests
{

    [TestFixture]
    public class CrockfordBase32EncoderTests
    {
        private readonly CrockfordBase32Encoder encoder = new CrockfordBase32Encoder();

        [Test]
        public void Encodes()
        {
            encoder.Encode(0).ShouldBe("0");
            encoder.Encode(1234).ShouldBe("16J");
            encoder.Encode(ulong.MaxValue).ShouldBe("FZZZZZZZZZZZZ");
        }

        [Test]
        public void Encodes_With_Checksum()
        {
            encoder.Encode(0, true).ShouldBe("00");
            encoder.Encode(1234, true).ShouldBe("16JD");
            encoder.Encode(ulong.MaxValue, true).ShouldBe("FZZZZZZZZZZZZB");
        }

        [Test]
        public void Decodes()
        {
            encoder.Decode("0").ShouldBe<ulong>(0);
            encoder.Decode("16J").ShouldBe<ulong>(1234);
            encoder.Decode("FZZZZZZZZZZZZ").ShouldBe(ulong.MaxValue);
        }

        [Test]
        public void Decodes_Lowercase()
        {
            encoder.Decode("16j").ShouldBe<ulong>(1234);
            encoder.Decode("fzzzzzzzzzzzz").ShouldBe(ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Separator()
        {
            encoder.Decode("16-J").ShouldBe<ulong>(1234);
            encoder.Decode("FZZ-ZZZ-ZZZ-ZZZZ").ShouldBe(ulong.MaxValue);
        }

        [Test]
        public void Decodes_With_Checksum()
        {
            encoder.Decode("16JD", true).ShouldBe<ulong>(1234);
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

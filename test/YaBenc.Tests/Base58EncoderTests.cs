using NUnit.Framework;
using Shouldly;

namespace YaBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class Base58EncoderTests
    {
        private readonly Base58Encoder encoder = new Base58Encoder();

        [TestCase(0UL, "1")]
        [TestCase(57UL, "z")]
        [TestCase(58UL, "21")]
        [TestCase(66051UL, "Ldp")]
        [TestCase(73300775185UL, "2vgLdhi")]
        [TestCase(281401388481450UL, "3CSwN61PP")]
        public void Encodes(ulong input, string output)
        {
            encoder.Encode(input).ShouldBe(output);
        }
    }
}

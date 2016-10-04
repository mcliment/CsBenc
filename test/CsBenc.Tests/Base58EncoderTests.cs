using NUnit.Framework;
using Shouldly;

namespace CsBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class Base58EncoderTests
    {
        private readonly SimpleEncoder encoder = Encoder.Base58();

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

        [TestCase("1", 0UL)]
        [TestCase("z", 57UL)]
        [TestCase("21", 58UL)]
        [TestCase("Ldp", 66051UL)]
        [TestCase("2vgLdhi", 73300775185UL)]
        [TestCase("3CSwN61PP", 281401388481450UL)]
        public void Decodes(string encoded, ulong output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

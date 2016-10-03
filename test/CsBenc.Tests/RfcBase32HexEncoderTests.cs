using NUnit.Framework;
using Shouldly;
using CsBenc.Strings;

namespace CsBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class RfcBase32HexEncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase32Hex();

        [TestCase("", "")]
        [TestCase("f", "CO======")]
        [TestCase("fo", "CPNG====")]
        [TestCase("foo", "CPNMU===")]
        [TestCase("foob", "CPNMUOG=")]
        [TestCase("fooba", "CPNMUOJ1")]
        [TestCase("foobar", "CPNMUOJ1E8======")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [TestCase("", "")]
        [TestCase("CO======", "f")]
        [TestCase("CPNG====", "fo")]
        [TestCase("CPNMU===", "foo")]
        [TestCase("CPNMUOG=", "foob")]
        [TestCase("CPNMUOJ1", "fooba")]
        [TestCase("CPNMUOJ1E8======", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

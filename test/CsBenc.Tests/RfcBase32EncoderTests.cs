using CsBenc.Encoders;
using NUnit.Framework;
using Shouldly;

namespace CsBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class RfcBase32EncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase32();

        [TestCase("", "")]
        [TestCase("f", "MY======")]
        [TestCase("fo", "MZXQ====")]
        [TestCase("foo", "MZXW6===")]
        [TestCase("foob", "MZXW6YQ=")]
        [TestCase("fooba", "MZXW6YTB")]
        [TestCase("foobar", "MZXW6YTBOI======")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [TestCase("", "")]
        [TestCase("MY======", "f")]
        [TestCase("MZXQ====", "fo")]
        [TestCase("MZXW6===", "foo")]
        [TestCase("MZXW6YQ=", "foob")]
        [TestCase("MZXW6YTB", "fooba")]
        [TestCase("MZXW6YTBOI======", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

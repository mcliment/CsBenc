using NUnit.Framework;
using Shouldly;
using YaBenc.Strings;

namespace YaBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class RfcBase64EncoderTests
    {
        private readonly RfcBase64Encoder encoder = new RfcBase64Encoder();

        [TestCase("", "")]
        [TestCase("f", "Zg==")]
        [TestCase("fo", "Zm8=")]
        [TestCase("foo", "Zm9v")]
        [TestCase("foob", "Zm9vYg==")]
        [TestCase("fooba", "Zm9vYmE=")]
        [TestCase("foobar", "Zm9vYmFy")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [TestCase("", "")]
        [TestCase("Zg==", "f")]
        [TestCase("Zm8=", "fo")]
        [TestCase("Zm9v", "foo")]
        [TestCase("Zm9vYg==", "foob")]
        [TestCase("Zm9vYmE=", "fooba")]
        [TestCase("Zm9vYmFy", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

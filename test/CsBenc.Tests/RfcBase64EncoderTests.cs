using NUnit.Framework;
using Shouldly;
using CsBenc.Strings;

namespace CsBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class RfcBase64EncoderTests
    {
        private readonly StringEncoder encoder = Strings.Encoder.RfcBase64();

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
        [TestCase("f", "Zg==")]
        [TestCase("fó", "ZsOz")]
        [TestCase("fóó", "ZsOzw7M=")]
        [TestCase("fóób", "ZsOzw7Ni")]
        [TestCase("fóóbà", "ZsOzw7Niw6A=")]
        [TestCase("fóóbàr", "ZsOzw7Niw6By")]
        public void Encodes_Test_Vectors_UTF8(string input, string encoded)
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

        [TestCase("", "")]
        [TestCase("Zg==", "f")]
        [TestCase("ZsOz", "fó")]
        [TestCase("ZsOzw7M=", "fóó")]
        [TestCase("ZsOzw7Ni", "fóób")]
        [TestCase("ZsOzw7Niw6A=", "fóóbà")]
        [TestCase("ZsOzw7Niw6By", "fóóbàr")]
        public void Decodes_Test_Vectors_UTF8(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

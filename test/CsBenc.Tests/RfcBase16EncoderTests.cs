using NUnit.Framework;
using Shouldly;
using CsBenc.Strings;

namespace CsBenc.Tests
{
    [TestFixture]
    [Parallelizable]
    public class RfcBase16EncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase16();

        [TestCase("", "")]
        [TestCase("f", "66")]
        [TestCase("fo", "666F")]
        [TestCase("foo", "666F6F")]
        [TestCase("foob", "666F6F62")]
        [TestCase("fooba", "666F6F6261")]
        [TestCase("foobar", "666F6F626172")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [TestCase("", "")]
        [TestCase("66", "f")]
        [TestCase("666F", "fo")]
        [TestCase("666F6F", "foo")]
        [TestCase("666F6F62", "foob")]
        [TestCase("666F6F6261", "fooba")]
        [TestCase("666F6F626172", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

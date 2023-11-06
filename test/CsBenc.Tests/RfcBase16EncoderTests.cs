using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class RfcBase16EncoderTests
    {
        private readonly StringEncoder _encoder = Encoder.RfcBase16();

        [Theory]
        [InlineData("", "")]
        [InlineData("f", "66")]
        [InlineData("fo", "666F")]
        [InlineData("foo", "666F6F")]
        [InlineData("foob", "666F6F62")]
        [InlineData("fooba", "666F6F6261")]
        [InlineData("foobar", "666F6F626172")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            _encoder.Encode(input).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("66", "f")]
        [InlineData("666F", "fo")]
        [InlineData("666F6F", "foo")]
        [InlineData("666F6F62", "foob")]
        [InlineData("666F6F6261", "fooba")]
        [InlineData("666F6F626172", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            _encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

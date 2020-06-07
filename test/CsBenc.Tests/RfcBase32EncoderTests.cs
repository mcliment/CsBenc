using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class RfcBase32EncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase32();

        [Theory]
        [InlineData("", "")]
        [InlineData("f", "MY======")]
        [InlineData("fo", "MZXQ====")]
        [InlineData("foo", "MZXW6===")]
        [InlineData("foob", "MZXW6YQ=")]
        [InlineData("fooba", "MZXW6YTB")]
        [InlineData("foobar", "MZXW6YTBOI======")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("MY======", "f")]
        [InlineData("MZXQ====", "fo")]
        [InlineData("MZXW6===", "foo")]
        [InlineData("MZXW6YQ=", "foob")]
        [InlineData("MZXW6YTB", "fooba")]
        [InlineData("MZXW6YTBOI======", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

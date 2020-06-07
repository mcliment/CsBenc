using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class RfcBase32HexEncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase32Hex();

        [Theory]
        [InlineData("", "")]
        [InlineData("f", "CO======")]
        [InlineData("fo", "CPNG====")]
        [InlineData("foo", "CPNMU===")]
        [InlineData("foob", "CPNMUOG=")]
        [InlineData("fooba", "CPNMUOJ1")]
        [InlineData("foobar", "CPNMUOJ1E8======")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("CO======", "f")]
        [InlineData("CPNG====", "fo")]
        [InlineData("CPNMU===", "foo")]
        [InlineData("CPNMUOG=", "foob")]
        [InlineData("CPNMUOJ1", "fooba")]
        [InlineData("CPNMUOJ1E8======", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

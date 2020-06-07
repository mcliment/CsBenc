using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class RfcBase64EncoderTests
    {
        private readonly StringEncoder encoder = Encoder.RfcBase64();

        [Fact]
        public void Encodes_Empty_Array()
        {
            encoder.Encode(new byte[] { }).ShouldBe("");
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("f", "Zg==")]
        [InlineData("fo", "Zm8=")]
        [InlineData("foo", "Zm9v")]
        [InlineData("foob", "Zm9vYg==")]
        [InlineData("fooba", "Zm9vYmE=")]
        [InlineData("foobar", "Zm9vYmFy")]
        public void Encodes_Test_Vectors(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("f", "Zg==")]
        [InlineData("fó", "ZsOz")]
        [InlineData("fóó", "ZsOzw7M=")]
        [InlineData("fóób", "ZsOzw7Ni")]
        [InlineData("fóóbà", "ZsOzw7Niw6A=")]
        [InlineData("fóóbàr", "ZsOzw7Niw6By")]
        public void Encodes_Test_Vectors_UTF8(string input, string encoded)
        {
            encoder.Encode(input).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Zg==", "f")]
        [InlineData("Zm8=", "fo")]
        [InlineData("Zm9v", "foo")]
        [InlineData("Zm9vYg==", "foob")]
        [InlineData("Zm9vYmE=", "fooba")]
        [InlineData("Zm9vYmFy", "foobar")]
        public void Decodes_Test_Vectors(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("Zg==", "f")]
        [InlineData("ZsOz", "fó")]
        [InlineData("ZsOzw7M=", "fóó")]
        [InlineData("ZsOzw7Ni", "fóób")]
        [InlineData("ZsOzw7Niw6A=", "fóóbà")]
        [InlineData("ZsOzw7Niw6By", "fóóbàr")]
        public void Decodes_Test_Vectors_UTF8(string encoded, string output)
        {
            encoder.Decode(encoded).ShouldBe(output);
        }
    }
}

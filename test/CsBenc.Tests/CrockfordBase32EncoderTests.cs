using System;
using System.Collections.Generic;
using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class CrockfordBase32EncoderTests
    {
        private readonly ChecksumEncoder _encoder = Encoder.CrockfordBase32();

        [Theory]
        [InlineData(0UL, "0")]
        [InlineData(1234UL, "16J")]
        [InlineData(ulong.MaxValue, "FZZZZZZZZZZZZ")]
        public void Encodes(ulong value, string encoded)
        {
            _encoder.Encode(value).ShouldBe(encoded);
        }

        [Theory]
        [InlineData(0UL, "00")]
        [InlineData(1234UL, "16JD")]
        [InlineData(ulong.MaxValue, "FZZZZZZZZZZZZB")]
        public void Encodes_With_Checksum(ulong value, string encoded)
        {
            _encoder.Encode(value, true).ShouldBe(encoded);
        }

        [Theory]
        [InlineData("0", 0UL)]
        [InlineData("16J", 1234UL)]
        [InlineData("FZZZZZZZZZZZZ", ulong.MaxValue)]
        public void Decodes(string encoded, ulong decoded)
        {
            _encoder.DecodeLong(encoded).ShouldBe(decoded);
        }

        [Theory]
        [InlineData("16j", 1234UL)]
        [InlineData("fzzzzzzzzzzzz", ulong.MaxValue)]
        public void Decodes_Lowercase(string encoded, ulong decoded)
        {
            _encoder.DecodeLong(encoded).ShouldBe(decoded);
        }

        [Theory]
        [InlineData("16-J", 1234UL)]
        [InlineData("FZZ-ZZZ-ZZZ-ZZZZ", ulong.MaxValue)]
        public void Decodes_With_Separator(string encoded, ulong decoded)
        {
            _encoder.DecodeLong(encoded).ShouldBe(decoded);
        }

        [Theory]
        [InlineData("16JD", 1234UL)]
        [InlineData("FZZZZZZZZZZZZB", ulong.MaxValue)]
        public void Decodes_With_Checksum(string encoded, ulong decoded)
        {
            _encoder.DecodeLong(encoded, true).ShouldBe(decoded);
        }

        [Theory]
        [MemberData(nameof(GetRandom), parameters: 10)]
        public void Random_Invert(ulong input)
        {
            var encoded = _encoder.Encode(input);
            var decoded = _encoder.DecodeLong(encoded);

            input.ShouldBe(decoded);
        }

        public static IEnumerable<object[]> GetRandom(int count)
        {
            var random = new Random(42);

            for (var i = 0; i < count; i++)
            {
                var buffer = new byte[8];
                random.NextBytes(buffer);
                yield return new object[] { BitConverter.ToUInt64(buffer, 0) };
            }
        }
    }
}

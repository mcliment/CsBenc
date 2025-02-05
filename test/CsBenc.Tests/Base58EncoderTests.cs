﻿using CsBenc.Encoders;
using Shouldly;
using Xunit;

namespace CsBenc.Tests
{
    public class Base58EncoderTests
    {
        private readonly SimpleEncoder _encoder = Encoder.Base58();

        [Theory]
        [InlineData(0UL, "1")]
        [InlineData(57UL, "z")]
        [InlineData(58UL, "21")]
        [InlineData(66051UL, "Ldp")]
        [InlineData(73300775185UL, "2vgLdhi")]
        [InlineData(281401388481450UL, "3CSwN61PP")]
        public void Encodes(ulong input, string output)
        {
            _encoder.Encode(input).ShouldBe(output);
        }

        [Theory]
        [InlineData("1", 0UL)]
        [InlineData("z", 57UL)]
        [InlineData("21", 58UL)]
        [InlineData("Ldp", 66051UL)]
        [InlineData("2vgLdhi", 73300775185UL)]
        [InlineData("3CSwN61PP", 281401388481450UL)]
        public void Decodes(string encoded, ulong output)
        {
            _encoder.DecodeLong(encoded).ShouldBe(output);
        }

        [Theory]
        [InlineData(new byte[] { }, "")]
        [InlineData(new byte[] { 0x61 }, "2g")]
        [InlineData(new byte[] { 0x62, 0x62, 0x62 }, "a3gV")]
        [InlineData(new byte[] { 0x63, 0x63, 0x63 }, "aPEr")]
        [InlineData(
            new byte[]
            {
                0x73,
                0x69,
                0x6d,
                0x70,
                0x6c,
                0x79,
                0x20,
                0x61,
                0x20,
                0x6c,
                0x6f,
                0x6e,
                0x67,
                0x20,
                0x73,
                0x74,
                0x72,
                0x69,
                0x6e,
                0x67
            },
            "2cFupjhnEsSn59qHXstmK2ffpLv2"
        )]
        [InlineData(
            new byte[]
            {
                0x00,
                0xeb,
                0x15,
                0x23,
                0x1d,
                0xfc,
                0xeb,
                0x60,
                0x92,
                0x58,
                0x86,
                0xb6,
                0x7d,
                0x06,
                0x52,
                0x99,
                0x92,
                0x59,
                0x15,
                0xae,
                0xb1,
                0x72,
                0xc0,
                0x66,
                0x47
            },
            "1NS17iag9jJgTHD1VXjvLCEnZuQ3rJDE9L"
        )]
        [InlineData(new byte[] { 0x51, 0x6b, 0x6f, 0xcd, 0x0f }, "ABnLTmg")]
        [InlineData(
            new byte[] { 0xbf, 0x4f, 0x89, 0x00, 0x1e, 0x67, 0x02, 0x74, 0xdd },
            "3SEo3LWLoPntC"
        )]
        [InlineData(new byte[] { 0x57, 0x2e, 0x47, 0x94 }, "3EFU7m")]
        [InlineData(
            new byte[] { 0xec, 0xac, 0x89, 0xca, 0xd9, 0x39, 0x23, 0xc0, 0x23, 0x21 },
            "EJDM8drfXA6uyA"
        )]
        [InlineData(new byte[] { 0x10, 0xc8, 0x51, 0x1e }, "Rt5zm")]
        [InlineData(
            new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            "1111111111"
        )]
        [InlineData(
            new byte[]
            {
                0x00,
                0x15,
                0x77,
                0xAE,
                0x26,
                0x0E,
                0x0D,
                0xD1,
                0xD3,
                0x3B,
                0xB6,
                0x5B,
                0x86,
                0x44,
                0x2B,
                0x59,
                0xEA,
                0xFB,
                0x04,
                0xDE,
                0x39,
                0xEE,
                0xAF,
                0xD1,
                0xCB
            },
            "12xWZZSKrrtFK28YukUjVaMc3f77GYE4CA"
        )]
        public void EncodesBytes(byte[] bytes, string expected)
        {
            var encoded = _encoder.Encode(bytes);

            encoded.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new byte[] { }, "")]
        [InlineData(new byte[] { 0x61 }, "2g")]
        [InlineData(new byte[] { 0x62, 0x62, 0x62 }, "a3gV")]
        [InlineData(new byte[] { 0x63, 0x63, 0x63 }, "aPEr")]
        [InlineData(
            new byte[]
            {
                0x73,
                0x69,
                0x6d,
                0x70,
                0x6c,
                0x79,
                0x20,
                0x61,
                0x20,
                0x6c,
                0x6f,
                0x6e,
                0x67,
                0x20,
                0x73,
                0x74,
                0x72,
                0x69,
                0x6e,
                0x67
            },
            "2cFupjhnEsSn59qHXstmK2ffpLv2"
        )]
        [InlineData(
            new byte[]
            {
                0x00,
                0xeb,
                0x15,
                0x23,
                0x1d,
                0xfc,
                0xeb,
                0x60,
                0x92,
                0x58,
                0x86,
                0xb6,
                0x7d,
                0x06,
                0x52,
                0x99,
                0x92,
                0x59,
                0x15,
                0xae,
                0xb1,
                0x72,
                0xc0,
                0x66,
                0x47
            },
            "1NS17iag9jJgTHD1VXjvLCEnZuQ3rJDE9L"
        )]
        [InlineData(new byte[] { 0x51, 0x6b, 0x6f, 0xcd, 0x0f }, "ABnLTmg")]
        [InlineData(
            new byte[] { 0xbf, 0x4f, 0x89, 0x00, 0x1e, 0x67, 0x02, 0x74, 0xdd },
            "3SEo3LWLoPntC"
        )]
        [InlineData(new byte[] { 0x57, 0x2e, 0x47, 0x94 }, "3EFU7m")]
        [InlineData(
            new byte[] { 0xec, 0xac, 0x89, 0xca, 0xd9, 0x39, 0x23, 0xc0, 0x23, 0x21 },
            "EJDM8drfXA6uyA"
        )]
        [InlineData(new byte[] { 0x10, 0xc8, 0x51, 0x1e }, "Rt5zm")]
        [InlineData(
            new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            "1111111111"
        )]
        [InlineData(
            new byte[]
            {
                0x00,
                0x15,
                0x77,
                0xAE,
                0x26,
                0x0E,
                0x0D,
                0xD1,
                0xD3,
                0x3B,
                0xB6,
                0x5B,
                0x86,
                0x44,
                0x2B,
                0x59,
                0xEA,
                0xFB,
                0x04,
                0xDE,
                0x39,
                0xEE,
                0xAF,
                0xD1,
                0xCB
            },
            "12xWZZSKrrtFK28YukUjVaMc3f77GYE4CA"
        )]
        public void DecodesBytes(byte[] bytes, string expected)
        {
            var decoded = _encoder.DecodeBytes(expected);

            decoded.ShouldBe(bytes);
        }
    }
}

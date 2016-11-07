using System;
using System.Diagnostics;
using System.Text;

namespace CsBenc.Encoders
{
    public class Base58Encoder : SimpleEncoder
    {
        private readonly string _alpha;

        public Base58Encoder(string alphabet) : base(alphabet)
        {
            _alpha = alphabet;
        }

        public override string Encode(byte[] input)
        {
            var leadingZeroes = 0;
            var length = 0;

            foreach (var n in input)
            {
                if (n == 0) leadingZeroes++;
                else break;
            }

            var resultSize = input.Length * 138 / 100 + 1; // log(256) / log(58) rounded up
            var result = new byte[resultSize];

            for (var pos = leadingZeroes; pos < input.Length; pos++)
            {
                int carry = input[pos];
                var i = 0;

                // Large integer division algorithm (adapted from base58.cpp)
                for (var resultPos = resultSize; (carry != 0 || i < length) && resultPos > 0; resultPos--, i++)
                {
                    carry = carry + 256 * result[resultPos - 1];
                    result[resultPos - 1] = (byte) (carry % 58);
                    carry = carry / 58;
                }

                Debug.Assert(carry == 0);
                length = i;
            }

            // Remove leading zeroes
            var leadingResultZeroes = resultSize - length;
            while (leadingResultZeroes < resultSize && result[leadingResultZeroes] == 0)
            {
                leadingResultZeroes++;
            }

            // Build result
            var encoded = new StringBuilder(leadingZeroes + resultSize - leadingResultZeroes /* known size */);

            encoded.Append(new string('1', leadingZeroes));

            for (var j = leadingResultZeroes; j < result.Length; j++)
            {
                encoded.Append(_alpha[result[j]]);
            }

            return encoded.ToString();
        }

        public override byte[] DecodeBytes(string encoded)
        {
            var zeroes = 0;

            for (var i = 0; i < encoded.Length; i++)
            {
                if (encoded[i] == '1') zeroes++;
                else break;
            }

            var resultSize = encoded.Length * 733 / 1000 + 1; // log(58) / log(256) rounded up
            var resultChars = new byte[resultSize];

            for (var i = zeroes; i < encoded.Length; i++)
            {
                var curr = encoded[i];

                var ch = _alpha.IndexOf(curr);
                if (ch < 0) throw new Exception($"Invalid character found in encoded string: {curr}");

                var carry = ch;

                for (var pos = resultChars.Length; pos > zeroes; pos--)
                {
                    carry = carry + 58 * resultChars[pos - 1];
                    resultChars[pos - 1] = (byte) (carry % 256);
                    carry = carry / 256;
                }

                Debug.Assert(carry == 0);
            }

            var trailingZeroes = 0;
            foreach (var ch in resultChars)
            {
                if (ch == 0) trailingZeroes++;
                else break;
            }

            var bytes = new byte[zeroes + resultChars.Length - trailingZeroes];
            for (var ix = zeroes; ix < bytes.Length; ix++)
            {
                bytes[ix] = resultChars[ix - zeroes + trailingZeroes];
            }

            return bytes;
        }
    }
}

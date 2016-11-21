using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace CsBenc.Internals
{
    internal class ArbitraryNumberProcessor : INumberProcessor
    {
        private readonly int _modulo;
        private readonly int _expandFactor;

        public ArbitraryNumberProcessor(int modulo)
        {
            _modulo = modulo;
            _expandFactor = (int) Math.Ceiling(Math.Log(256) * 100 / Math.Log(_modulo));
        }

        public virtual IEnumerable<byte> Chunk(ulong number)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }
            
            var chunks = YieldChunks(number);

            return chunks.Reverse();
        }

        public virtual byte[] Chunk(byte[] input, int startOffset, out int endOffset)
        {
            var length = input.Length;

            if (length == 0)
            {
                endOffset = 0;
                return new byte[] { };
            }

            var len = 0;

            var resultSize = length * _expandFactor / 100 + 1;
            var result = new byte[resultSize];

            for (var pos = startOffset; pos < length; pos++)
            {
                int carry = input[pos];
                var i = 0;

                // Large integer division algorithm (adapted from base58.cpp)
                for (var resultPos = resultSize; (carry != 0 || i < len) && resultPos > 0; resultPos--, i++)
                {
                    carry = carry + 256 * result[resultPos - 1];
                    result[resultPos - 1] = (byte)(carry % _modulo);
                    carry = carry / _modulo;
                }

                Debug.Assert(carry == 0);
                len = i;
            }

            // Remove leading zeroes
            var leadingResultZeroes = resultSize - len;
            while (leadingResultZeroes < resultSize && result[leadingResultZeroes] == 0)
            {
                leadingResultZeroes++;
            }

            endOffset = leadingResultZeroes;
            return result;
        }

        public virtual ulong CombineLong(byte[] chunks)
        {
            ulong result = 0;

            for (var i = 0; i < chunks.Length; i++)
            {
                result = (result * (ulong)_modulo) + chunks[i];
            }

            return result;
        }

        public virtual byte[] CombineBytes(byte[] chunks)
        {
            BigInteger number = 0;

            for (var i = 0; i < chunks.Length; i++)
            {
                number = number * _modulo + chunks[i];
            }

            var bytes = number.ToByteArray().Reverse().ToArray();

            if (bytes[0] == 0x00)
            {
                var result = new byte[bytes.Length - 1];

                Array.Copy(bytes, 1, result, 0, result.Length);

                return result;
            }

            return bytes;
        }

        private IEnumerable<byte> YieldChunks(ulong number)
        {
            var next = number;

            while (next > 0)
            {
                var rem = next % (ulong)_modulo; // remainder
                next = next / (ulong)_modulo; // division

                yield return (byte)rem;
            }
        }
    }
}

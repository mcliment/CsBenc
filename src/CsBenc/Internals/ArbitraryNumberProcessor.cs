using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsBenc.Internals
{
    internal class ArbitraryNumberProcessor : INumberProcessor
    {
        private readonly int _modulo;
        private readonly int _expandFactor;
        private readonly int _shrinkFactor;

        public ArbitraryNumberProcessor(int modulo)
        {
            _modulo = modulo;
            _expandFactor = (int)Math.Ceiling(Math.Log(256) * 100 / Math.Log(_modulo));
            _shrinkFactor = (int)Math.Ceiling(Math.Log(_modulo) * 1000 / Math.Log(256));
        }

        public virtual IEnumerable<byte> Chunk(ulong number)
        {
            if (number == 0)
            {
                return [0];
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
                return [];
            }

            var len = 0;

            var resultSize = length * _expandFactor / 100 + 1;
            var result = new byte[resultSize];

            for (var pos = startOffset; pos < length; pos++)
            {
                int carry = input[pos];
                var i = 0;

                // Large integer division algorithm (adapted from base58.cpp)
                for (
                    var resultPos = resultSize;
                    (carry != 0 || i < len) && resultPos > 0;
                    resultPos--, i++
                )
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

            foreach (var t in chunks)
            {
                result = (result * (ulong)_modulo) + t;
            }

            return result;
        }

        public virtual byte[] CombineBytes(byte[] chunks, int startOffset)
        {
            var resultSize = chunks.Length * _shrinkFactor / 1000 + 1;
            var resultChars = new byte[resultSize];

            for (var i = startOffset; i < chunks.Length; i++)
            {
                var curr = chunks[i];

                int ch = curr;

                var carry = ch;

                for (var pos = resultChars.Length; pos > startOffset; pos--)
                {
                    carry = carry + _modulo * resultChars[pos - 1];
                    resultChars[pos - 1] = (byte)(carry % 256);
                    carry = carry / 256;
                }

                Debug.Assert(carry == 0);
            }

            var trailingZeroes = 0;
            foreach (var ch in resultChars)
            {
                if (ch == 0)
                    trailingZeroes++;
                else
                    break;
            }

            var bytes = new byte[startOffset + resultChars.Length - trailingZeroes];
            for (var ix = startOffset; ix < bytes.Length; ix++)
            {
                bytes[ix] = resultChars[ix - startOffset + trailingZeroes];
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

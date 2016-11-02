using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CsBenc.Internals
{
    internal class ArbitraryNumberProcessor : INumberProcessor
    {
        private const int byteSize = 8;
        private readonly int _modulo;

        public ArbitraryNumberProcessor(int modulo)
        {
            _modulo = modulo;
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

        public virtual IEnumerable<byte> Chunk(byte[] value)
        {
            if (value.Length == 0)
            {
                return new byte[] { };
            }

            // Append 0 at the end to force positiveness
            var num = new BigInteger(value.Reverse().Concat(new byte[] { 0x00 }).ToArray());

            var chunks = YieldChunks(num);

            return chunks.Reverse();
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

        private IEnumerable<byte> YieldChunks(BigInteger number)
        {
            var next = number;

            while (next != BigInteger.Zero)
            {
                BigInteger rem;
                next = BigInteger.DivRem(next, _modulo, out rem);
                // var rem = next % (ulong)_modulo; // remainder
                // next = next / (ulong)_modulo; // division

                yield return (byte)rem;
            }
        }
    }
}

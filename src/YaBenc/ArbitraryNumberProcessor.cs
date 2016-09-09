using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public class ArbitraryNumberProcessor : INumberProcessor
    {
        private readonly int _modulo;

        public ArbitraryNumberProcessor(int modulo)
        {
            _modulo = modulo;
        }

        public IEnumerable<byte> Chunk(ulong number)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }

            var chunks = YieldChunks(number);

            return chunks.Reverse();
        }

        public ulong Combine(byte[] chunks)
        {
            ulong result = 0;

            for (var i = 0; i < chunks.Length; i++)
            {
                result = (result * (ulong)_modulo) + chunks[i];
            }

            return result;
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

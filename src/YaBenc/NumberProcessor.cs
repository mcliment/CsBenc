using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public class NumberProcessor
    {
        private readonly int _bitSize;

        public NumberProcessor(int bitSize)
        {
            _bitSize = bitSize;
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
                result = (result << _bitSize) + chunks[i];
            }

            return result;
        }

        private IEnumerable<byte> YieldChunks(ulong number)
        {
            var divider = (ulong)(1 << _bitSize) - 1;

            var next = number;

            while (next > 0)
            {
                var rem = (byte)(next & divider); // remainder
                next = next >> _bitSize; // division

                yield return rem;
            }
        }
    }
}

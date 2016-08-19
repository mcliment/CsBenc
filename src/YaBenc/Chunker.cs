using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public static class Chunker
    {
        public static IEnumerable<byte> GetChunks(string value, int size)
        {
            if (size == 2)
            {
                foreach (byte c in value)
                {
                    yield return (byte)(c >> 4);
                    yield return (byte)(c & 0xf);
                }
            }
        }

        public static IEnumerable<byte> GetChunks(ulong number, int size)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }

            var chunks = YieldChunks(number, size);

            return chunks.Reverse();
        }

        private static IEnumerable<byte> YieldChunks(ulong number, int size)
        {
            var divider = (ulong)(1 << size) - 1;

            var next = number;

            while (next > 0)
            {
                var rem = (byte)(next & divider); // remainder
                next = next >> size; // division

                yield return rem;
            }
        }
    }
}

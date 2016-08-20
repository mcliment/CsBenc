using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public static class Chunker
    {
        private const int byteSize = 8;

        public static IEnumerable<byte> GetChunks(string value, int size)
        {
            if (string.IsNullOrEmpty(value))
            {
                yield break;
            }

            // Simple case for base16 (no padding)
            if (size == 4)
            {
                foreach (byte ch in value)
                {
                    yield return (byte)(ch >> 4);
                    yield return (byte)(ch & 0xf);
                }
            }
            else
            {
                var rem = byteSize;
                var mask = (1 << size) - 1;
                var i = 0;
                var ch = (int)value[i++];

                while (rem > 0)
                {
                    if (rem > size)
                    {
                        rem = rem - size;
                    }
                    else if (i >= value.Length)
                    {
                        var dis = size - rem;

                        rem = 0;

                        ch = ch << dis;
                    }
                    else
                    {
                        rem = rem + byteSize - size;
                        ch = (ch << byteSize) | value[i++];
                    }

                    var m = mask << rem;

                    var v = (ch & m) >> rem;

                    yield return (byte)v;
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

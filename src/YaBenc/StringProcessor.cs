using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public class StringProcessor
    {
        private const int byteSize = 8;
        private readonly int _size;

        public StringProcessor(int size)
        {
            _size = size;
        }

        public IEnumerable<byte> Chunk(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                yield break;
            }

            // Simple case for base16 (no padding)
            if (_size == 4)
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
                var mask = (1 << _size) - 1;
                var i = 0;
                var ch = (int)value[i++];

                while (rem > 0)
                {
                    if (rem > _size)
                    {
                        rem = rem - _size;
                    }
                    else if (i >= value.Length)
                    {
                        var dis = _size - rem;

                        rem = 0;

                        ch = ch << dis;
                    }
                    else
                    {
                        rem = rem + byteSize - _size;
                        ch = (ch << byteSize) | value[i++];
                    }

                    var m = mask << rem;

                    var v = (ch & m) >> rem;

                    yield return (byte)v;
                }
            }
        }

        public char[] Pad(IEnumerable<char> chars, char pad)
        {
            var cs = chars.ToArray();

            var size = cs.Length;
            var newSize = size;

            while(newSize * _size % 8 != 0)
            {
                newSize++;
            }

            System.Array.Resize(ref cs, newSize);

            for(var i = size; i < newSize; i++)
            {
                cs[i] = pad;
            }

            return cs;
        }

        public IEnumerable<char> Combine(IEnumerable<int> decoded)
        {
            var decs = decoded.ToArray();
            
            if (decs.Length == 0)
            {
                yield break;
            }

            var rem = byteSize;
            var mask = (1 << byteSize) - 1;

            var i = 0;
            int dec = 0;
            var ch = 0;

            while (rem > 0)
            {
                if (i >= decs.Length)
                {
                    rem = 0;
                }
                else if (rem > _size)
                {
                    dec = decs[i++];

                    rem = rem - _size;
                    ch = (ch | dec << rem) & mask;
                }
                else if (rem <= _size)
                {
                    dec = decs[i++];

                    var disp = _size - rem;
                    ch = (ch | (dec >> disp)) & mask;

                    yield return (char)ch;

                    rem = byteSize - disp;

                    ch = dec << rem;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CsBenc.Internals
{
    internal class StringProcessor
    {
        private const int ByteSize = 8;
        private readonly int _size;

        public StringProcessor(int size)
        {
            _size = size;
        }

        public IEnumerable<byte> Chunk(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Enumerable.Empty<byte>();
            }

            return Chunk(System.Text.Encoding.UTF8.GetBytes(value));
        }

        public IEnumerable<byte> Chunk(byte[] value)
        {
            if (value.Length == 0)
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
                var rem = ByteSize;
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
                        rem = rem + ByteSize - _size;
                        ch = (ch << ByteSize) | value[i++];
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

            Array.Resize(ref cs, newSize);

            for(var i = size; i < newSize; i++)
            {
                cs[i] = pad;
            }

            return cs;
        }

        public byte[] Combine(int[] decoded)
        {
            if (decoded.Length == 0)
            {
                return Array.Empty<byte>();
            }

            // TODO :: Get a better approximation
            var result = new byte[decoded.Length];

            var rem = ByteSize;
            var mask = (1 << ByteSize) - 1;

            var i = 0;
            int dec;
            var ch = 0;
            var len = 0;

            while (rem > 0)
            {
                if (i >= decoded.Length)
                {
                    rem = 0;
                }
                else if (rem > _size)
                {
                    dec = decoded[i++];

                    rem = rem - _size;
                    ch = (ch | dec << rem) & mask;
                }
                else if (rem <= _size)
                {
                    dec = decoded[i++];

                    var disp = _size - rem;
                    ch = (ch | (dec >> disp)) & mask;

                    result[len] = (byte)ch;
                    len++;

                    rem = ByteSize - disp;

                    ch = dec << rem;
                }
            }

            var realResult = new byte[len];

            Array.Copy(result, realResult, len);

            return realResult;
        }
    }
}

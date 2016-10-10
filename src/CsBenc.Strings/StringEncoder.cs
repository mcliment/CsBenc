using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CsBenc.Strings.Impl;

namespace CsBenc.Strings
{
    public class StringEncoder
    {
        private readonly string _alphabet;
        private readonly char _pad;

        private readonly StringProcessor _processor;

        public StringEncoder(string alphabet, char pad)
        {
            _pad = pad;
            _alphabet = alphabet;

            var bits = CountBits(_alphabet.Length);
            _processor = new StringProcessor(bits);

            Debug.Assert(1 << bits == _alphabet.Length);
        }

        public string Encode(string value)
        {
            return EncodeChunks(_processor.Chunk(value));
        }

        public string Encode(byte[] input)
        {
            return EncodeChunks(_processor.Chunk(input));
        }

        private string EncodeChunks(IEnumerable<byte> chunks)
        {
            var array = chunks.ToArray();
            var chars = new char[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                chars[i] = _alphabet[array[i]];
            }

            var result = new string(_processor.Pad(chars, _pad));

            return result;
        }

        public string Decode(string input)
        {
            return Encoding.UTF8.GetString(DecodeBytes(input));
        }

        public byte[] DecodeBytes(string input)
        {
            return GetBytes(input.TrimEnd(_pad)).ToArray();
        }

        private IEnumerable<byte> GetBytes(string input)
        {
            var values = input.TrimEnd(_pad).Select(i => _alphabet.IndexOf(i));

            var result = _processor.Combine(values);

            return result;
        }

        private int CountBits(int length)
        {
            var bits = 0;
            var cur = length & 1;

            while (cur == 0 && bits < 8)
            {
                bits++;
                length = length >> 1;
                cur = length & 1;
            }

            Debug.Assert(bits > 0 && bits < 8);

            return bits;
        }
    }
}

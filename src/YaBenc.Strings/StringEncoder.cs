using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YaBenc.Strings
{
    public class StringEncoder
    {
        private readonly string _alphabet;
        private readonly char _pad;
        private readonly int _bits;

        private readonly StringProcessor _processor;

        public StringEncoder(string alphabet, char pad)
        {
            _pad = pad;
            _alphabet = alphabet;

            _bits = CountBits(_alphabet.Length);
            _processor = new StringProcessor(_bits);

            Debug.Assert(1 << _bits == _alphabet.Length);
        }

        public string Encode(string value)
        {
            var chunks = _processor.Chunk(value);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(_processor.Pad(chars, _pad));

            return result;
        }

        public string Decode(string input)
        {
            var chars = GetChars(input.TrimEnd(_pad)).ToArray();

            return new string(chars);
        }

        private IEnumerable<char> GetChars(string input)
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

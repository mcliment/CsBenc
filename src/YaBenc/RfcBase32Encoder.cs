using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaBenc
{
    public class RfcBase32Encoder
    {
        private readonly static int bits = 5;
        private readonly static int basE = 1 << bits;

        private readonly static string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private readonly static char _pad = '=';

        private readonly static StringProcessor _processor = new StringProcessor(bits);

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
        
    }
}

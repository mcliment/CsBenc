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

        public string Encode(string value)
        {
            var chunks = Chunker.GetChunks(value, bits);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(chars);

            while ((result.Length * bits % 8) != 0)
            {
                result += _pad;
            }

            return result;
        }

        public string Decode(string input)
        {
            var chars = GetChars(input).ToArray();

            return new string(chars);
        }

        private IEnumerable<char> GetChars(string input)
        {
            for (var i = 0; i < input.Length; i += 2)
            {
                var l = _alphabet.IndexOf(input[i]);
                var r = _alphabet.IndexOf(input[i + 1]);

                yield return (char)((l << bits) + r);
            }
        }
        
    }
}

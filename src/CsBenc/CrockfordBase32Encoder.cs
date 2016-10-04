using System.Collections.Generic;
using System.Linq;

namespace CsBenc
{
    public class CrockfordBase32Encoder : ChecksumEncoder
    {
        private readonly static Dictionary<char, byte> _equiv = new Dictionary<char, byte> {
            { 'I', 1 }, { 'i', 1 }, { 'L', 1 }, { 'l', 1 }, { 'O', 0 }, { 'o', 0 }
        };

        public CrockfordBase32Encoder() : base("0123456789ABCDEFGHJKMNPQRSTVWXYZ", "*~$=U")
        {
        }

        public override ulong Decode(string encoded, bool checksum)
        {
            var clean = Translate(CleanInput(encoded));

            return base.Decode(clean, checksum);
        }

        private string CleanInput(string input)
        {
            var upper = input.ToUpperInvariant();

            if (upper.Contains('-'))
            {
                var noHyphens = upper.Where(c => c != '-').ToArray();

                return new string(noHyphens);
            }

            return upper;
        }

        private string Translate(string input)
        {
            var translated = input.Select(c =>
            {
                return _equiv.ContainsKey(c) ? (char)_equiv[c] : c;
            });

            return new string(translated.ToArray());
        }
    }
}

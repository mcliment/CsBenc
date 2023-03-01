using System.Collections.Generic;
using System.Linq;

namespace CsBenc.Encoders
{
    /// <summary>
    /// Douglas Crockford's Base32 implementation of the Checksum encoder
    /// </summary>
    public class CrockfordBase32Encoder : ChecksumEncoder
    {
        private readonly static Dictionary<char, byte> _equiv = new Dictionary<char, byte> {
            { 'I', 1 }, { 'i', 1 }, { 'L', 1 }, { 'l', 1 }, { 'O', 0 }, { 'o', 0 }
        };

        /// <summary>
        /// Creates a new instance of the Crockford's Base32 encoder.
        /// </summary>
        public CrockfordBase32Encoder() : base("0123456789ABCDEFGHJKMNPQRSTVWXYZ", "*~$=U")
        {
        }

        /// <summary>
        /// Decodes an encoded string as a number (with an optional checksum).
        /// </summary>
        /// <param name="encoded">Encoded string with optional checksum</param>
        /// <param name="checksum">true to indicate that the string contains a checksum.</param>
        /// <returns>Decoded numeric value</returns>
        public override ulong DecodeLong(string encoded, bool checksum)
        {
            var clean = Translate(CleanInput(encoded));

            return base.DecodeLong(clean, checksum);
        }

        private static string CleanInput(string input)
        {
            var upper = input.ToUpperInvariant();

            if (upper.Contains('-'))
            {
                var noHyphens = upper.Where(c => c != '-').ToArray();

                return new string(noHyphens);
            }

            return upper;
        }

        private static string Translate(string input)
        {
            var translated = input.Select(c =>
            {
                return _equiv.ContainsKey(c) ? (char)_equiv[c] : c;
            });

            return new string(translated.ToArray());
        }
    }
}

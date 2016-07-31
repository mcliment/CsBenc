using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YaBenc
{
    public class CrockfordBase32Encoder
    {
        private readonly static string _alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        private readonly static string _checksums = "*~$=U";
        private readonly static Dictionary<char, byte> _equiv = new Dictionary<char, byte> {
            { 'I', 1 }, { 'i', 1 }, { 'L', 1 }, { 'l', 1 }, { 'O', 0 }, { 'o', 0 }
        };

        private readonly static int wordSize = 5;
        private readonly static int alphabetSize = _alphabet.Length;
        private readonly static int checksumBase = alphabetSize + _checksums.Length;

        public string Encode(ulong number, bool checksum = false)
        {
            var chunks = Chunker.GetChunks(number, wordSize);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(chars);

            if (checksum)
            {
                result += GetChecksum(number);
            }

            return result;
        }

        public ulong Decode(string encoded, bool checksum = false)
        {
            var clean = CleanInput(encoded);

            var chunks = GetChunks(checksum ? clean.Substring(0, clean.Length - 1) : clean).ToArray();
            var result = Combiner.Combine(chunks, wordSize);

            if (checksum)
            {
                var csc = GetChecksum(result);

                if (csc != encoded[encoded.Length - 1])
                {
                    throw new Exception("Checksum mismatch");
                }
            }

            return result;
        }

        private char GetChecksum(ulong number)
        {
            var checksum = (byte)(number % (ulong)checksumBase);

            var checksumChar = checksum > alphabetSize ? _checksums[alphabetSize - checksum] : _alphabet[checksum];

            return checksumChar;
        }

        private string CleanInput(string input)
        {
            var upper = input.ToUpperInvariant();

            if (upper.Contains('-'))
            {
                var good = upper.Where(c => c != '-').ToArray();

                return new string(good);
            }

            return upper;
        }

        private IEnumerable<byte> GetChunks(string encoded)
        {
            for (var i = 0; i < encoded.Length; i++)
            {
                var curr = encoded[i];
                var chunk = (byte)_alphabet.IndexOf(curr); // TODO :: Explore alternatives

                if (chunk < 0 && _equiv.ContainsKey(curr))
                {
                    chunk = _equiv[curr];
                }

                Debug.Assert(chunk >= 0);

                yield return chunk;
            }
        }
    }
}

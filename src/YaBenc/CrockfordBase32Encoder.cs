using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YaBenc
{
    public class CrockfordBase32Encoder
    {
        private readonly static int bits = 5;
        private readonly static int basE = 1 << bits;

        private readonly static string _alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        private readonly static string _checksums = "*~$=U";
        private readonly static Dictionary<char, byte> _equiv = new Dictionary<char, byte> {
            { 'I', 1 }, { 'i', 1 }, { 'L', 1 }, { 'l', 1 }, { 'O', 0 }, { 'o', 0 }
        };

        private readonly static int checksumBase = basE + _checksums.Length;

        public string Encode(ulong number, bool checksum = false)
        {
            var chunks = GetChunks(number);
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

            var values = GetValues(checksum ? clean.Substring(0, clean.Length - 1) : clean).ToArray();
            var result = SumValues(values);

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

        private IEnumerable<byte> GetChunks(ulong number)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }

            var chunks = YieldChunks(number);

            return chunks.Reverse();
        }

        private IEnumerable<byte> YieldChunks(ulong number)
        {
            var next = number;

            while (next > 0)
            {
                var rem = (byte)(next & (ulong)(basE - 1));
                next = next >> bits;

                yield return rem;
            }
        }

        private char GetChecksum(ulong number)
        {
            var cs = (byte)(number % (ulong)checksumBase);

            var csc = cs > basE ? _checksums[basE - cs] : _alphabet[cs];

            return csc;
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

        private IEnumerable<byte> GetValues(string encoded)
        {
            for (var i = 0; i < encoded.Length; i++)
            {
                var curr = encoded[i];
                var val = (byte)_alphabet.IndexOf(curr); // TODO :: Explore alternatives

                if (val < 0 && _equiv.ContainsKey(curr))
                {
                    val = _equiv[curr];
                }

                Debug.Assert(val >= 0);

                yield return val;
            }
        }

        private ulong SumValues(byte[] values)
        {
            ulong result = 0;

            for (var i = 0; i < values.Length; i++)
            {
                result = (result << bits) + values[i];
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;

namespace YaBenc
{
    public class Base32Encoder
    {
        private readonly static string _alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        private readonly static string _checksums = "*~$=U";
        private readonly static Dictionary<char, int> _equiv = new Dictionary<char, int> {
            { 'I', 1 }, { 'i', 1 }, { 'L', 1 }, { 'l', 1 }, { 'O', 0 }, { 'o', 0 }
        };

        private readonly static int bas = _alphabet.Length;
        private readonly static int csBase = _alphabet.Length + _checksums.Length;

        public string Encode(ulong number, bool checksum = false)
        {
            var result = "";

            var next = number;

            while (next > 0)
            {
                var rem = next % (ulong)bas;
                next = next / (ulong)bas;

                result = _alphabet[(int)rem] + result;
            }

            if (checksum)
            {
                var cs = (int)number % csBase;

                var csc = cs > bas ? _checksums[bas - cs] : _alphabet[cs];

                result += csc;
            }

            return result;
        }

        public ulong Decode(string encoded, bool checksum = false)
        {
            ulong result = 0;

            // TODO :: Check values in range
            var length = checksum ? encoded.Length - 1 : encoded.Length;

            for (var i = 0; i < length; i++)
            {
                var curr = encoded[i];
                var val = _alphabet.IndexOf(curr);

                if (val < 0 && _equiv.ContainsKey(curr))
                {
                    val = _equiv[curr];
                }

                // TODO :: Avoid pow, inverting stuff
                result += (ulong)(Math.Pow(bas, length - i - 1) * val);
            }

            if (checksum)
            {
                var cs = (int)result % csBase;

                var csc = cs > bas ? _checksums[bas - cs] : _alphabet[cs];

                if (csc != encoded[encoded.Length - 1])
                {
                    throw new Exception("Checksum mismatch");
                }
            }

            return result;
        }
    }
}

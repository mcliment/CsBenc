using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YaBenc
{
    public class Base58Encoder
    {
        private readonly static string _alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        private readonly static int bits = 58;
        private readonly static NumberProcessor _processor = new NumberProcessor(bits);

        public string Encode(ulong number /*, bool checksum = false */)
        {
            var chunks = _processor.Chunk(number);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(chars);

            //if (checksum)
            //{
            //    result += GetChecksum(number);
            //}

            return result;
        }

        public ulong Decode(string encoded/*, bool checksum = false*/)
        {
            //var clean = CleanInput(encoded);

            //var chunks = GetChunks(checksum ? clean.Substring(0, clean.Length - 1) : clean).ToArray();
            var chunks = GetChunks(encoded);
            var result = _processor.Combine(chunks.ToArray());

            //if (checksum)
            //{
            //    var csc = GetChecksum(result);

            //    if (csc != encoded[encoded.Length - 1])
            //    {
            //        throw new Exception("Checksum mismatch");
            //    }
            //}

            return result;
        }

        private IEnumerable<byte> GetChunks(string encoded)
        {
            for (var i = 0; i < encoded.Length; i++)
            {
                var curr = encoded[i];
                var chunk = (byte)_alphabet.IndexOf(curr); // TODO :: Explore alternatives

                //if (chunk < 0 && _equiv.ContainsKey(curr))
                //{
                //    chunk = _equiv[curr];
                //}

                Debug.Assert(chunk >= 0);

                yield return chunk;
            }
        }
    }
}

using System;
using System.Linq;

namespace CsBenc
{
    public class ChecksumEncoder : SimpleEncoder
    {
        private readonly string _checksums;
        private readonly int _checksumBase;

        public ChecksumEncoder(string alphabet, string checksums) : base(alphabet)
        {
            _checksums = checksums;
            _checksumBase = alphabet.Length + _checksums.Length;
        }

        public override string Encode(ulong number)
        {
            return Encode(number, false);
        }

        public virtual string Encode(ulong number, bool checksum)
        {
            var encoded = base.Encode(number);

            if (checksum)
            {
                encoded += GetChecksum(number);
            }

            return encoded;
        }

        public override ulong Decode(string encoded)
        {
            return Decode(encoded, false);
        }

        public virtual ulong Decode(string encoded, bool checksum)
        {
            var chunks = GetChunks(checksum ? encoded.Substring(0, encoded.Length - 1) : encoded).ToArray();
            var result = Processor.Combine(chunks);

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
            var checksum = (byte)(number % (ulong)_checksumBase);
            var alphabetSize = Alphabet.Length;

            var checksumChar = checksum > alphabetSize ? _checksums[alphabetSize - checksum] : Alphabet[checksum];

            return checksumChar;
        }
    }
}

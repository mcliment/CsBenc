using System;
using System.Linq;

namespace CsBenc.Encoders
{
    /// <summary>
    /// Simple encoder with checksum abilities.
    /// </summary>
    public class ChecksumEncoder : SimpleEncoder
    {
        private readonly string _checksums;
        private readonly int _checksumBase;

        /// <summary>
        /// Creates a new instance of an encoder with checksum abilities.
        /// </summary>
        /// <param name="alphabet">The alphabet to use for encoding. Limited to 255 characters</param>
        /// <param name="checksums">Extension to the alphabet for the checksums</param>
        public ChecksumEncoder(string alphabet, string checksums)
            : base(alphabet)
        {
            _checksums = checksums;
            _checksumBase = alphabet.Length + _checksums.Length;
        }

        /// <summary>
        /// Encodes a number as a string without checksum.
        /// </summary>
        /// <param name="number">Number to encode</param>
        /// <returns>String encoding of the number</returns>
        public override string Encode(ulong number)
        {
            return Encode(number, false);
        }

        /// <summary>
        /// Encodes a number as a string, optionally adding a checksum.
        /// </summary>
        /// <param name="number">Number to encode</param>
        /// <param name="checksum">true to add a checksum to the resulting string.</param>
        /// <returns>String encoding of the number with an optional checksum</returns>
        public virtual string Encode(ulong number, bool checksum)
        {
            var encoded = base.Encode(number);

            if (checksum)
            {
                encoded += GetChecksum(number);
            }

            return encoded;
        }

        /// <summary>
        /// Decodes an encoded string without checksum as a number.
        /// </summary>
        /// <param name="encoded">Encoded string without checksum</param>
        /// <returns>Decoded numeric value</returns>
        public override ulong DecodeLong(string encoded)
        {
            return DecodeLong(encoded, false);
        }

        /// <summary>
        /// Decodes an encoded string as a number (with an optional checksum).
        /// </summary>
        /// <param name="encoded">Encoded string with optional checksum</param>
        /// <param name="checksum">true to indicate that the string contains a checksum.</param>
        /// <returns>Decoded numeric value</returns>
        public virtual ulong DecodeLong(string encoded, bool checksum)
        {
            var chunks = GetChunks(checksum ? encoded.Substring(0, encoded.Length - 1) : encoded)
                .ToArray();
            var result = Processor.CombineLong(chunks);

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

            var checksumChar =
                checksum > alphabetSize ? _checksums[alphabetSize - checksum] : Alphabet[checksum];

            return checksumChar;
        }
    }
}

using CsBenc.Encoders;

namespace CsBenc
{
    public static class Encoder
    {
        /// <summary>
        /// Creates a new instance of a Crockford Base32 encoder/decoder.
        /// </summary>
        /// <returns>A <see cref="ChecksumEncoder"/> configured for Crockford Base32 encoding</returns>
        public static ChecksumEncoder CrockfordBase32()
        {
            return new CrockfordBase32Encoder();
        }

        /// <summary>
        /// Creates a new instance of a Bitcoin Base58 encoder/decoder.
        /// </summary>
        /// <returns>A <see cref="SimpleEncoder"/> configured for Bitcoin Base58 encoding</returns>
        public static SimpleEncoder Base58()
        {
            return new Base58Encoder("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz");
        }

        /// <summary>
        /// Creates a new instance of an RFC-4648 compliant Base16 encoder/decoder.
        /// </summary>
        /// <returns>A <see cref="StringEncoder"/> configured for RFC compliant Base16.</returns>
        public static StringEncoder RfcBase16()
        {
            return new StringEncoder("0123456789ABCDEF", '=');
        }

        /// <summary>
        /// Creates a new instance of an RFC-4648 compliant Base32 encoder/decoder.
        /// </summary>
        /// <returns>A <see cref="StringEncoder"/> configured for RFC compliant Base32.</returns>
        public static StringEncoder RfcBase32()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '=');
        }

        /// <summary>
        /// Creates a new instance of an RFC-4648 compliant Base32 encoder/decoder (hex variant). This variant is easily sortable while the standard is not.
        /// </summary>
        /// <returns>A <see cref="StringEncoder"/> configured for RFC compliant Base32 (hex variant).</returns>
        public static StringEncoder RfcBase32Hex()
        {
            return new StringEncoder("0123456789ABCDEFGHIJKLMNOPQRSTUV", '=');
        }

        /// <summary>
        /// Creates a new instance of an RFC-4648 compliant Base64 encoder/decoder.
        /// </summary>
        /// <returns>A <see cref="StringEncoder"/> configured for RFC compliant Base64.</returns>
        public static StringEncoder RfcBase64()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", '=');
        }

        /// <summary>
        /// Creates a new instance of an RFC-4648 compliant Base64 encoder/decoder (URL variant). This variant replaces / with _ to be safe for URLs.
        /// </summary>
        /// <returns>A <see cref="StringEncoder"/> configured for RFC compliant Base64 (URL variant).</returns>
        public static StringEncoder RfcBase64Url()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", '=');
        }
    }
}

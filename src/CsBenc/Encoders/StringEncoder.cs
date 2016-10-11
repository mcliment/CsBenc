using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CsBenc.Internals;

namespace CsBenc.Encoders
{
    /// <summary>
    /// An encoder that allows the encoding of streams and strings.
    /// </summary>
    public class StringEncoder
    {
        private readonly string _alphabet;
        private readonly char _pad;

        private readonly StringProcessor _processor;

        /// <summary>
        /// Creates a new instance of this encoder with the specified alphabet and padding character.
        /// </summary>
        /// <param name="alphabet">Alphabet to use by this encoder</param>
        /// <param name="pad">Padding character</param>
        public StringEncoder(string alphabet, char pad)
        {
            _pad = pad;
            _alphabet = alphabet;

            var bits = CountBits(_alphabet.Length);
            _processor = new StringProcessor(bits);

            Debug.Assert(1 << bits == _alphabet.Length);
        }

        /// <summary>
        /// Encode the specified string with this encoder.
        /// </summary>
        /// <param name="value">String to encode</param>
        /// <returns>Encoded string</returns>
        public string Encode(string value)
        {
            return EncodeChunks(_processor.Chunk(value));
        }

        /// <summary>
        /// Encode the bytes to its string encoding.
        /// </summary>
        /// <param name="input">Array of bytes to encode</param>
        /// <returns>Encoded string</returns>
        public string Encode(byte[] input)
        {
            return EncodeChunks(_processor.Chunk(input));
        }

        private string EncodeChunks(IEnumerable<byte> chunks)
        {
            var array = chunks.ToArray();
            var chars = new char[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                chars[i] = _alphabet[array[i]];
            }

            var result = new string(_processor.Pad(chars, _pad));

            return result;
        }

        /// <summary>
        /// Decode the specified string as an string in UTF8.
        /// </summary>
        /// <param name="input">Encoded string to decode</param>
        /// <returns>The decoded UTF8 string.</returns>
        public string Decode(string input)
        {
            return Encoding.UTF8.GetString(DecodeBytes(input));
        }

        /// <summary>
        /// Decode the specified string as an array of bytes.
        /// </summary>
        /// <param name="input">Encoded string to decode</param>
        /// <returns>Decoded array of bytes</returns>
        public byte[] DecodeBytes(string input)
        {
            return GetBytes(input.TrimEnd(_pad)).ToArray();
        }

        private IEnumerable<byte> GetBytes(string input)
        {
            var values = input.TrimEnd(_pad).Select(i => _alphabet.IndexOf(i));

            var result = _processor.Combine(values);

            return result;
        }

        private int CountBits(int length)
        {
            var bits = 0;
            var cur = length & 1;

            while (cur == 0 && bits < 8)
            {
                bits++;
                length = length >> 1;
                cur = length & 1;
            }

            Debug.Assert(bits > 0 && bits < 8);

            return bits;
        }
    }
}

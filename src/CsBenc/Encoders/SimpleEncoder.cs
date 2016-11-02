using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using CsBenc.Internals;
using System.Text;

namespace CsBenc.Encoders
{
    /// <summary>
    /// Provides a simple number encoder with an arbitrary dictionary.
    /// </summary>
    public class SimpleEncoder
    {
        private readonly string _alphabet;
        private readonly NumberProcessor _processor;

        /// <summary>
        /// Creates a new instance of the encoder with the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to use for the encoding. Limited to 255 characters.</param>
        public SimpleEncoder(string alphabet)
        {
            _alphabet = alphabet;
            _processor = new NumberProcessor(alphabet.Length);
        }

        protected string Alphabet { get { return _alphabet; } }

        protected INumberProcessor Processor { get { return _processor; } }

        /// <summary>
        /// Encode the number with the specified alphabet.
        /// </summary>
        /// <param name="number">Number to encode</param>
        /// <returns>The string encoding of the number</returns>
        public virtual string Encode(ulong number)
        {
            var chunks = _processor.Chunk(number);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(chars);

            return new string(chars);
        }

        public virtual string Encode(byte[] number)
        {
            var chunks = _processor.Chunk(number);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();

            return new string(chars);
        }

        /// <summary>
        /// Decodes the specified string to its numeric representation.
        /// </summary>
        /// <param name="encoded">Encoded string</param>
        /// <returns>The numeric representation of the encoded string</returns>
        public virtual ulong DecodeLong(string encoded)
        {
            var chunks = GetChunks(encoded);
            var result = _processor.CombineLong(chunks.ToArray());

            return result;
        }

        public virtual byte[] DecodeBytes(string encoded)
        {
            var chunks = GetChunks(encoded);
            var result = _processor.CombineBytes(chunks.ToArray());

            return result;
        }

        protected virtual IEnumerable<byte> GetChunks(string encoded)
        {
            for (var i = 0; i < encoded.Length; i++)
            {
                var curr = encoded[i];
                var chunk = (byte)_alphabet.IndexOf(curr); // TODO :: Explore alternatives

                Debug.Assert(chunk >= 0);

                yield return chunk;
            }
        }
    }
}

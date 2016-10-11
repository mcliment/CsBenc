using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CsBenc.Internals;

namespace CsBenc.Encoders
{
    public class SimpleEncoder
    {
        private readonly string _alphabet;
        private readonly NumberProcessor _processor;

        public SimpleEncoder(string alphabet)
        {
            _alphabet = alphabet;
            _processor = new NumberProcessor(alphabet.Length);
        }

        protected string Alphabet { get { return _alphabet; } }

        protected INumberProcessor Processor { get { return _processor; } }

        public virtual string Encode(ulong number)
        {
            var chunks = _processor.Chunk(number);
            var chars = chunks.Select(c => _alphabet[c]).ToArray();
            var result = new string(chars);

            return result;
        }

        public virtual ulong Decode(string encoded)
        {
            var chunks = GetChunks(encoded);
            var result = _processor.Combine(chunks.ToArray());

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

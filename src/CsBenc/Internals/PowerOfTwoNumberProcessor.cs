using System.Collections.Generic;
using System.Linq;

namespace CsBenc.Internals
{
    internal class PowerOfTwoNumberProcessor : ArbitraryNumberProcessor
    {
        private readonly int _power;

        public PowerOfTwoNumberProcessor(int power, int modulo)
            : base(modulo)
        {
            _power = power;
        }

        public override IEnumerable<byte> Chunk(ulong number)
        {
            if (number == 0)
            {
                return new byte[] { 0 };
            }

            var chunks = YieldChunks(number);

            return chunks.Reverse();
        }

        public override ulong CombineLong(byte[] chunks)
        {
            ulong result = 0;

            foreach (var t in chunks)
            {
                result = (result << _power) + t;
            }

            return result;
        }

        public override byte[] CombineBytes(byte[] chunks, int startOffset)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<byte> YieldChunks(ulong number)
        {
            var divider = (ulong)(1 << _power) - 1;

            var next = number;

            while (next > 0)
            {
                var rem = (byte)(next & divider); // remainder
                next >>= _power; // division

                yield return rem;
            }
        }
    }
}

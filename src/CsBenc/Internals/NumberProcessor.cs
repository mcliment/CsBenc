using System;
using System.Collections.Generic;

namespace CsBenc.Internals
{
    internal class NumberProcessor : INumberProcessor
    {
        private readonly INumberProcessor _wrapped;

        public NumberProcessor(int modulo)
        {
            if (modulo > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException("The base or modulo can't be larger than a byte", nameof(modulo));
            }
            
            switch (modulo)
            {
                case 2:
                    _wrapped = new PowerOfTwoNumberProcessor(1, modulo);
                    break;
                case 4:
                    _wrapped = new PowerOfTwoNumberProcessor(2, modulo);
                    break;
                case 8:
                    _wrapped = new PowerOfTwoNumberProcessor(3, modulo);
                    break;
                case 16:
                    _wrapped = new PowerOfTwoNumberProcessor(4, modulo);
                    break;
                case 32:
                    _wrapped = new PowerOfTwoNumberProcessor(5, modulo);
                    break;
                case 64:
                    _wrapped = new PowerOfTwoNumberProcessor(6, modulo);
                    break;
                case 128:
                    _wrapped = new PowerOfTwoNumberProcessor(7, modulo);
                    break;
                case 256:
                    _wrapped = new PowerOfTwoNumberProcessor(8, modulo);
                    break;
                default:
                    _wrapped = new ArbitraryNumberProcessor(modulo);
                    break;
            }
        }

        public IEnumerable<byte> Chunk(ulong number)
        {
            return _wrapped.Chunk(number);
        }

        public byte[] Chunk(byte[] bytes, int startOffset, out int endOffset)
        {
            return _wrapped.Chunk(bytes, startOffset, out endOffset);
        }

        public ulong CombineLong(byte[] chunks)
        {
            return _wrapped.CombineLong(chunks);
        }

        public byte[] CombineBytes(byte[] chunks, int startOffset)
        {
            return _wrapped.CombineBytes(chunks, startOffset);
        }
    }
}

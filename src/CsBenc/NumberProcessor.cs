using System;
using System.Collections.Generic;
using CsBenc.Impl;

namespace CsBenc
{
    public class NumberProcessor : INumberProcessor
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
                    _wrapped = new PowerOfTwoNumberProcessor(1);
                    break;
                case 4:
                    _wrapped = new PowerOfTwoNumberProcessor(2);
                    break;
                case 8:
                    _wrapped = new PowerOfTwoNumberProcessor(3);
                    break;
                case 16:
                    _wrapped = new PowerOfTwoNumberProcessor(4);
                    break;
                case 32:
                    _wrapped = new PowerOfTwoNumberProcessor(5);
                    break;
                case 64:
                    _wrapped = new PowerOfTwoNumberProcessor(6);
                    break;
                case 128:
                    _wrapped = new PowerOfTwoNumberProcessor(7);
                    break;
                case 256:
                    _wrapped = new PowerOfTwoNumberProcessor(8);
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

        public ulong Combine(byte[] chunks)
        {
            return _wrapped.Combine(chunks);
        }
    }
}

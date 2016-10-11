using System.Collections.Generic;

namespace CsBenc.Internals
{
    public interface INumberProcessor
    {
        IEnumerable<byte> Chunk(ulong number);

        ulong Combine(byte[] chunks);
    }
}

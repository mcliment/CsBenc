using System.Collections.Generic;

namespace CsBenc.Internals
{
    public interface INumberProcessor
    {
        IEnumerable<byte> Chunk(ulong number);

        IEnumerable<byte> Chunk(byte[] bytes);

        ulong CombineLong(byte[] chunks);

        byte[] CombineBytes(byte[] chunks);
    }
}

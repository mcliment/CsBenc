using System.Collections.Generic;

namespace CsBenc.Internals
{
    public interface INumberProcessor
    {
        IEnumerable<byte> Chunk(ulong number);

        byte[] Chunk(byte[] bytes, int startOffset, out int endOffset);

        ulong CombineLong(byte[] chunks);

        byte[] CombineBytes(byte[] chunks, int startOffset);
    }
}

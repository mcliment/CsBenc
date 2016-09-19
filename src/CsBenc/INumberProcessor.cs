using System.Collections.Generic;
using System.Linq;

namespace CsBenc
{
    public interface INumberProcessor
    {
        IEnumerable<byte> Chunk(ulong number);

        ulong Combine(byte[] chunks);
    }
}

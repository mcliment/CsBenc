using System.Collections.Generic;
using System.Linq;

namespace YaBenc
{
    public interface INumberProcessor
    {
        IEnumerable<byte> Chunk(ulong number);

        ulong Combine(byte[] chunks);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaBenc
{
    public static class Combiner
    {
        public static ulong Combine(byte[] chunks, int size)
        {
            ulong result = 0;

            for (var i = 0; i < chunks.Length; i++)
            {
                result = (result << size) + chunks[i];
            }

            return result;
        }
    }
}

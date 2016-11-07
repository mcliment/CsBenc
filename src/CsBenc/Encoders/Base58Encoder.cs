using System;

namespace CsBenc.Encoders
{
    public class Base58Encoder : SimpleEncoder
    {
        public Base58Encoder(string alphabet) : base(alphabet)
        {
        }

        public override string Encode(byte[] number)
        {
            var leadingZeroes = 0;

            for (var i = 0; i < number.Length; i++)
            {
                if (number[i] == 0) leadingZeroes++;
                else break;
            }

            var toEncode = new byte[number.Length - leadingZeroes];

            Array.Copy(number, leadingZeroes, toEncode, 0, toEncode.Length);

            var encoded = base.Encode(toEncode);

            var toAppend = new string('1', leadingZeroes);

            return toAppend + encoded;
        }

        public override byte[] DecodeBytes(string encoded)
        {
            var leadingOnes = 0;

            for (var i = 0; i < encoded.Length; i++)
            {
                if (i == '1') leadingOnes++;
                else break;
            }

            var subencoded = encoded.Substring(leadingOnes);

            var decoded = base.DecodeBytes(subencoded);

            var result = new byte[leadingOnes + decoded.Length];

            Array.Copy(decoded, 0, result, leadingOnes, decoded.Length);

            return result;
        }
    }
}

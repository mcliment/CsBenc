namespace YaBenc.Strings
{
    public class RfcBase32HexEncoder : StringEncoder
    {
        public RfcBase32HexEncoder() : base("0123456789ABCDEFGHIJKLMNOPQRSTUV", '=')
        {
        }
    }
}

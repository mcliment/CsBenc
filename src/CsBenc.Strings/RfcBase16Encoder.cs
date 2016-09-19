namespace CsBenc.Strings
{
    public class RfcBase16Encoder : StringEncoder
    {
        public RfcBase16Encoder() : base("0123456789ABCDEF", '=')
        {
        }
    }
}

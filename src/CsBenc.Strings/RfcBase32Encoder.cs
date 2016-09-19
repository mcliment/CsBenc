namespace CsBenc.Strings
{
    public class RfcBase32Encoder : StringEncoder
    {
        public RfcBase32Encoder() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '=')
        {
        }
    }
}

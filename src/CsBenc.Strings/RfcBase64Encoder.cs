namespace CsBenc.Strings
{
    public class RfcBase64Encoder : StringEncoder
    {
        public RfcBase64Encoder() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", '=')
        {
        }
    }
}

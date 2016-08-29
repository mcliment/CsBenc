namespace YaBenc
{
    public class RfcBase64UrlEncoder : StringEncoder
    {
        public RfcBase64UrlEncoder() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", '=')
        {
        }
    }
}

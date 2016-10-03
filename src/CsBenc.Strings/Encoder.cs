namespace CsBenc.Strings
{
    public static class Encoder
    {
        public static StringEncoder RfcBase16()
        {
            return new StringEncoder("0123456789ABCDEF", '=');
        }

        public static StringEncoder RfcBase32()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '=');
        }

        public static StringEncoder RfcBase32Hex()
        {
            return new StringEncoder("0123456789ABCDEFGHIJKLMNOPQRSTUV", '=');
        }

        public static StringEncoder RfcBase64()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", '=');
        }

        public static StringEncoder RfcBase64Url()
        {
            return new StringEncoder("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", '=');
        }
    }
}

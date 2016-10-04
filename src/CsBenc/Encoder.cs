namespace CsBenc
{
    public static class Encoder
    {
        public static ChecksumEncoder CrockfordBase32()
        {
            return new CrockfordBase32Encoder();
        }

        public static SimpleEncoder Base58()
        {
            return new SimpleEncoder("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz");
        }
    }
}

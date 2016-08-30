using NUnit.Framework;
using Shouldly;
using YaBenc.Strings;

namespace YaBenc.Tests
{
    [TestFixture]
    public class RfcBase32HexEncoderTests
    {
        private readonly RfcBase32HexEncoder encoder = new RfcBase32HexEncoder();

        [Test]
        public void Encodes_Test_Vectors()
        {
            encoder.Encode("").ShouldBe("");
            encoder.Encode("f").ShouldBe("CO======");
            encoder.Encode("fo").ShouldBe("CPNG====");
            encoder.Encode("foo").ShouldBe("CPNMU===");
            encoder.Encode("foob").ShouldBe("CPNMUOG=");
            encoder.Encode("fooba").ShouldBe("CPNMUOJ1");
            encoder.Encode("foobar").ShouldBe("CPNMUOJ1E8======");
        }

        [Test]
        public void Decodes_Test_Vectors()
        {
            encoder.Decode("").ShouldBe("");
            encoder.Decode("CO======").ShouldBe("f");
            encoder.Decode("CPNG====").ShouldBe("fo");
            encoder.Decode("CPNMU===").ShouldBe("foo");
            encoder.Decode("CPNMUOG=").ShouldBe("foob");
            encoder.Decode("CPNMUOJ1").ShouldBe("fooba");
            encoder.Decode("CPNMUOJ1E8======").ShouldBe("foobar");
        }
    }
}

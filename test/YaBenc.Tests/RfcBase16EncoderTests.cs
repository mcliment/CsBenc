using NUnit.Framework;
using Shouldly;
using YaBenc.Strings;

namespace YaBenc.Tests
{
    [TestFixture]
    public class RfcBase16EncoderTests
    {
        private readonly RfcBase16Encoder encoder = new RfcBase16Encoder();

        [Test]
        public void Encodes_Test_Vectors()
        {
            encoder.Encode("").ShouldBe("");
            encoder.Encode("f").ShouldBe("66");
            encoder.Encode("fo").ShouldBe("666F");
            encoder.Encode("foo").ShouldBe("666F6F");
            encoder.Encode("foob").ShouldBe("666F6F62");
            encoder.Encode("fooba").ShouldBe("666F6F6261");
            encoder.Encode("foobar").ShouldBe("666F6F626172");
        }

        [Test]
        public void Decodes_Test_Vectors()
        {
            encoder.Decode("").ShouldBe("");
            encoder.Decode("66").ShouldBe("f");
            encoder.Decode("666F").ShouldBe("fo");
            encoder.Decode("666F6F").ShouldBe("foo");
            encoder.Decode("666F6F62").ShouldBe("foob");
            encoder.Decode("666F6F6261").ShouldBe("fooba");
            encoder.Decode("666F6F626172").ShouldBe("foobar");
        }
    }
}

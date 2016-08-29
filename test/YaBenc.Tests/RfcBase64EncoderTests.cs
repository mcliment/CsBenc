using NUnit.Framework;
using Shouldly;

namespace YaBenc.Tests
{
    [TestFixture]
    public class RfcBase64EncoderTests
    {
        private readonly RfcBase64Encoder encoder = new RfcBase64Encoder();

        [Test]
        public void Encodes_Test_Vectors()
        {
            encoder.Encode("").ShouldBe("");
            encoder.Encode("f").ShouldBe("Zg==");
            encoder.Encode("fo").ShouldBe("Zm8=");
            encoder.Encode("foo").ShouldBe("Zm9v");
            encoder.Encode("foob").ShouldBe("Zm9vYg==");
            encoder.Encode("fooba").ShouldBe("Zm9vYmE=");
            encoder.Encode("foobar").ShouldBe("Zm9vYmFy");
        }

        [Test]
        public void Decodes_Test_Vectors()
        {
            encoder.Decode("").ShouldBe("");
            encoder.Decode("Zg==").ShouldBe("f");
            encoder.Decode("Zm8=").ShouldBe("fo");
            encoder.Decode("Zm9v").ShouldBe("foo");
            encoder.Decode("Zm9vYg==").ShouldBe("foob");
            encoder.Decode("Zm9vYmE=").ShouldBe("fooba");
            encoder.Decode("Zm9vYmFy").ShouldBe("foobar");
        }
    }
}

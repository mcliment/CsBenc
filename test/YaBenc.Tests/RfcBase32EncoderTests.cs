using NUnit.Framework;
using Shouldly;

namespace YaBenc.Tests
{
    [TestFixture]
    public class RfcBase32EncoderTests
    {
        private readonly RfcBase32Encoder encoder = new RfcBase32Encoder();

        [Test]
        public void Encodes_Test_Vectors()
        {
            encoder.Encode("").ShouldBe("");
            encoder.Encode("f").ShouldBe("MY======");
            encoder.Encode("fo").ShouldBe("MZXQ====");
            encoder.Encode("foo").ShouldBe("MZXW6===");
            encoder.Encode("foob").ShouldBe("MZXW6YQ=");
            encoder.Encode("fooba").ShouldBe("MZXW6YTB");
            encoder.Encode("foobar").ShouldBe("MZXW6YTBOI======");
        }

        [Test]
        public void Decodes_Test_Vectors()
        {
            encoder.Decode("").ShouldBe("");
            encoder.Decode("MY======").ShouldBe("f");
            encoder.Decode("MZXQ====").ShouldBe("fo");
            encoder.Decode("MZXW6===").ShouldBe("foo");
            encoder.Decode("MZXW6YQ=").ShouldBe("foob");
            encoder.Decode("MZXW6YTB").ShouldBe("fooba");
            encoder.Decode("MZXW6YTBOI======").ShouldBe("foobar");
        }
    }
}

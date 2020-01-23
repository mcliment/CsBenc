namespace Checks

open FsCheck
open global.Xunit

module Base58Checks =

    let myEncoder = CsBenc.Encoder.Base58()
    let config = { Config.QuickThrowOnFailure with EndSize = 1000 }

    let endodesAndDecodes (s:uint64) = 
        myEncoder.DecodeLong(myEncoder.Encode(s)) = s

    let encodesAndDecodesBytes (s:byte[]) =
        myEncoder.DecodeBytes(myEncoder.Encode(s)) = s

    [<Fact>]
    let ``Check that encoding and decoding returns original value`` () =
        Check.One (config, endodesAndDecodes)

    [<Fact>]
    let ``Check that byte encoding returns original value`` () =
        Check.One (config, encodesAndDecodesBytes)
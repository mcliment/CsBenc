namespace Checks

open FsCheck
open NUnit.Framework
open System

module Base64Checks =

    let systemEncoder = Convert.ToBase64String
    let systemDecoder = Convert.FromBase64String

    let myEncoder = CsBenc.Encoder.RfcBase64()

    let config = { Config.QuickThrowOnFailure with EndSize = 1000 }

    let encodesAsSystem (s:array<byte>) = 
        myEncoder.Encode(s) = systemEncoder s
        |> Prop.trivial (s.Length = 0)

    let decodesAsSystem (s:array<byte>) = 
        myEncoder.DecodeBytes(systemEncoder s) = systemDecoder (systemEncoder s)
        |> Prop.trivial (s.Length = 0)

    let endodesAndDecodes (s:array<byte>) = 
        myEncoder.DecodeBytes(myEncoder.Encode(s)) = s 
        |> Prop.trivial (s.Length = 0)

    [<Test>]
    let ``Check that base64 encodes as that on System`` () =
        Check.One (config, encodesAsSystem)

    [<Test>]
    let ``Check that base64 decodes as that on System`` () =
        Check.One (config, decodesAsSystem)

    [<Test>]
    let ``Check that base64 encoding and decoding returns original value`` () =
        Check.One (config, endodesAndDecodes)
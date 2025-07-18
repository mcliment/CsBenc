﻿namespace Checks

open FsCheck
open FsCheck.FSharp
open global.Xunit
open System

module Base64Checks =

    let systemEncoder = Convert.ToBase64String
    let systemDecoder = Convert.FromBase64String

    let myEncoder = CsBenc.Encoder.RfcBase64()

    let config = Config.QuickThrowOnFailure.WithEndSize(1000)

    let encodesAsSystem (s:array<byte>) = 
        myEncoder.Encode(s) = systemEncoder s
        |> Prop.trivial (s.Length = 0)

    let decodesAsSystem (s:array<byte>) = 
        myEncoder.DecodeBytes(systemEncoder s) = systemDecoder (systemEncoder s)
        |> Prop.trivial (s.Length = 0)

    let encodesAndDecodes (s:array<byte>) = 
        myEncoder.DecodeBytes(myEncoder.Encode(s)) = s 
        |> Prop.trivial (s.Length = 0)

    [<Fact>]
    let ``Check that base64 encodes as that on System`` () =
        Check.One (config, encodesAsSystem)

    [<Fact>]
    let ``Check that base64 decodes as that on System`` () =
        Check.One (config, decodesAsSystem)

    [<Fact>]
    let ``Check that base64 encoding and decoding returns original value`` () =
        Check.One (config, encodesAndDecodes)
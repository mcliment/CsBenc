namespace Checks

module Base64Checks =

    open FsCheck
    open NUnit.Framework
    open FsUnit
    open System

    let systemEncoder = Convert.ToBase64String
    let systemDecoder = Convert.FromBase64String

    let myEncoder = CsBenc.Encoder.RfcBase64()

    let encodesAsSystem (s:array<byte>) = myEncoder.Encode(s) = systemEncoder s
    let decodesAsSystem (s:string) = myEncoder.DecodeBytes(s) = systemDecoder s

    [<Test>]
    let ``Check that base64 encodes as that on System`` () =
        Check.Quick encodesAsSystem

    [<Test>]
    let ``Check that base64 decodes as that on System`` () =
        Check.Quick decodesAsSystem
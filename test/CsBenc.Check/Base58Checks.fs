namespace Checks

open FsCheck
open NUnit.Framework
open FsUnit
open System

module Base58Checks =

    let myEncoder = CsBenc.Encoder.Base58()

    let config = { Config.QuickThrowOnFailure with EndSize = 1000 }

    let endodesAndDecodes (s:uint64) = 
        myEncoder.Decode(myEncoder.Encode(s)) = s

    [<Test>]
    let ``Check that encoding and decoding returns original value`` () =
        Check.One (config, endodesAndDecodes)
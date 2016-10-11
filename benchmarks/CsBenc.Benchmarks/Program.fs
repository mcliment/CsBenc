module Program

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open CsBenc

type Base64EncodingComparison () =
    let N = 10000

    let base64net = System.Convert.ToBase64String
    let base64encoder = CsBenc.Encoder.RfcBase64()

    let mutable data = Array.zeroCreate<byte> N

    [<Setup>]
    member self.SetupData () =
        data <- Array.zeroCreate<byte> N
        (new Random(42)).NextBytes(data)

    [<Benchmark(Baseline = true)>]
    member self.NetBase64 () = base64net(data)

    [<Benchmark>]
    member self.OwnBase64 () = base64encoder.Encode(data)

type Base64DecodingComparison () =
    let N = 10000

    let base64net = System.Convert.FromBase64String
    let base64decoder = CsBenc.Encoder.RfcBase64()

    let mutable encoded = ""

    [<Setup>]
    member self.SetupData () =
        let mutable data = Array.zeroCreate<byte> N
        (new Random(42)).NextBytes(data)
        encoded <- System.Convert.ToBase64String(data)

    [<Benchmark(Baseline = true)>]
    member self.NetBase64 () = base64net(encoded)

    [<Benchmark>]
    member self.OwnBase64 () = base64decoder.Decode(encoded)


let defaultSwitch () = BenchmarkSwitcher [| typeof<Base64EncodingComparison>; typeof<Base64DecodingComparison> |]

[<EntryPoint>]
let main argv = 
    let summary = defaultSwitch().Run argv
    0 // return an integer exit code

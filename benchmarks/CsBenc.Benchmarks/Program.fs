module Program

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open CsBenc.Strings

type Base64Comparison () =
    let N = 10000

    let base64net = System.Convert.ToBase64String
    let base64encoder = CsBenc.Strings.Encoder.RfcBase64()

    let mutable data = Array.zeroCreate<byte> N

    [<Setup>]
    member self.SetupData () =
        data <- Array.zeroCreate<byte> N
        (new Random(42)).NextBytes(data)

    [<Benchmark(Baseline = true)>]
    member self.NetBase64 () = System.Convert.ToBase64String(data)

    [<Benchmark>]
    member self.OwnBase64 () = base64encoder.Encode(data)
        

let defaultSwitch () = BenchmarkSwitcher [| typeof<Base64Comparison> |]

[<EntryPoint>]
let main argv = 
    let summary = defaultSwitch().Run argv
    0 // return an integer exit code

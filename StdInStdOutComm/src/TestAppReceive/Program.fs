open System
open StdComm

[<EntryPoint>]
let main argv =
    let io = IO(Console.OpenStandardInput(), Console.OpenStandardOutput())

    while true do
        printfn "Waiting for message..."
        let msg = io.receive()
        printfn "received message: %s" msg
    0 // return an integer exit code
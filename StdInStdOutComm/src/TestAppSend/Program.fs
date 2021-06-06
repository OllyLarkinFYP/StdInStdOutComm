open System
open System.IO
open System.Diagnostics
open StdComm

[<EntryPoint>]
let main argv =
    let myProcess = new Process()
    myProcess.StartInfo.FileName <- "dotnet.exe"
    myProcess.StartInfo.Arguments <- "run --project C:/Users/ollyl/Documents/Important/Uni/Year_4/FYP/IPC/StdInStdOutComm/StdInStdOutComm/src/TestAppReceive/TestAppReceive.fsproj"
    myProcess.StartInfo.UseShellExecute <- false
    myProcess.StartInfo.RedirectStandardInput <- true
    myProcess.StartInfo.RedirectStandardOutput <- false

    if myProcess.Start()
    then printfn "Got true"
    else printfn "Got false"

    let myStreamWriter = myProcess.StandardInput

    let io = IO(Console.OpenStandardInput(), myStreamWriter.BaseStream)
    
    for _ in [0..3] do
        let msg = Console.ReadLine()
        io.send msg

    myStreamWriter.Close()

    myProcess.Kill()

    0 // return an integer exit code
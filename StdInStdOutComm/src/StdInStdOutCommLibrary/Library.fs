namespace StdComm

open System
open System.IO

type IO (inStream: Stream, outStream: Stream) =

    /// Size of the message header in bytes
    [<Literal>]
    let HeaderSize = 4

    member _.receive () =
        let getInt () =
            let mutable outVal = -1
            while outVal = -1 do
                outVal <- inStream.ReadByte()
            outVal

        let getChar () = char <| getInt()
        let getString () = string <| getChar()

        let size =
            [0 .. HeaderSize - 1]
            |> List.map (fun _ -> getInt())
            |> List.indexed
            |> List.map (fun (i, value) -> value <<< (i * 8))
            |> List.reduce (+)
        
        if size > 0
        then
            [0 .. size - 1]
            |> List.map (fun _ -> getString())
            |> List.reduce (+)
        else ""

    member _.send (message: string) = 
        (message.Length, [0 .. HeaderSize - 1])
        ||> List.mapFold (fun s _ -> 
            let str = (s &&& 15) |> byte
            str, (s >>> 8))
        |> fst
        |> List.iter outStream.WriteByte
        
        message
        |> Seq.map byte
        |> Seq.iter outStream.WriteByte

        outStream.Flush()

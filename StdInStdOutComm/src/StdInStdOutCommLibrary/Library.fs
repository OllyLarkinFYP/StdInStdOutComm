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
            |> fun a -> printfn "message size: %A" a; a
            |> List.indexed
            |> List.map (fun (i, value) -> value <<< (i * 4))
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
            let b = (s &&& 15) |> byte
            b, (s >>> 4))
        |> fst
        |> fun a -> printfn "sent message size: %A" a; a
        |> List.iter outStream.WriteByte
        
        message
        |> Seq.map byte
        |> Seq.iter outStream.WriteByte

        outStream.Flush()

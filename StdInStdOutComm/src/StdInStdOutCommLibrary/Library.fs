module StdComm

open System

/// Size of the message header in bytes
[<Literal>]
let HeaderSize = 4

let receive () =
    let getInt () =
        let mutable outVal = -1
        while outVal = -1 do
            outVal <- Console.Read()
        outVal

    let getChar () = char <| getInt()
    let getString () = string <| getChar()

    let size =
        [0 .. HeaderSize - 1]
        |> List.map (fun _ -> getInt())
        |> List.indexed
        |> List.map (fun (i, value) -> value <<< (i * 8))
        |> List.reduce (+)
    
    [0 .. size - 1]
    |> List.map (fun _ -> getString())
    |> List.reduce (+)

let send (message: string) = 
    let size = message.Length
    let header =
        (size, [0 .. HeaderSize - 1])
        ||> List.mapFold (fun s _ -> 
            let str = (s &&& 15) |> char |> string
            str, (s >>> 8))
        |> fst
        |> List.reduce (+)
    printf "%s" (header + message)

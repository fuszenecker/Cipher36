﻿namespace Cipher36

module main =

    open System
    open GLFSR

    [<EntryPoint>]
    let main args =
        try
            if Array.length args < 2
                then failwith "Usage: Cipher36 initValue numberofChars"

            let initValue = args.[0]
            let (success, numberOfChars) = Int32.TryParse(args.[1])

            if not success then
                failwithf "Wrong number format of %A" args.[1]

            let state = GLFSR.initialize initValue

            printfn "Cipher36 is initialized as %A" state

            let mutable state' = state

            printfn "Stepped into state %d --> %A" 0 state'

            for i = 1 to numberOfChars do
                state' <- GLFSR.step state'
                printfn "Stepped into state %d --> %A" i state'

            0
        with 
        | exn as x -> printfn "Exception caught: %s" x.Message
                      0
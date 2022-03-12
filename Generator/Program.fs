namespace Cipher36

module main =

    open System
    open GLFSR

    [<EntryPoint>]
    let main args =
        let characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

        try
            if Array.length args < 2
                then failwith "Usage: Cipher36 initValue numberofChars"

            let initValue = args.[0]
            let (success, numberOfChars) = Int32.TryParse(args.[1])

            if not success then
                failwithf "Wrong number format of %A" args.[1]

            let mutable state = GLFSR.initialize initValue

            printfn "Cipher36 is initialized as %A" state
            printf "Generating %d characters" numberOfChars

            let numberofBits = ((36I ** numberOfChars).ToByteArray().Length) * 8

            let mutable state' = state
            let mutable ones = 0
            let mutable zeroes = 0
            let mutable superNumber = bigint.Zero

            for i = 0 to numberofBits do
                state <- GLFSR.stepAndShrink state
                superNumber <- superNumber * 2I

                if state.bit
                then
                    superNumber <- superNumber + 1I
                    ones <- ones + 1
                else
                    zeroes <- zeroes + 1  

            for i = 0 to numberOfChars - 1 do
                if i % 5 = 0 then printf " "
                if i % 25 = 0 then printf "\n"
                if i % 125 = 0 then printf "\n----\nP%03d\n----\n" (i / 125)

                let (superNumber', character) = bigint.DivRem (superNumber, 36I)
                superNumber <- superNumber'

                let char = characters.[int character]

                printf "%c" char

            printfn "\n\nOnes: %d, zeroes: %d" ones zeroes
            0
        with
        | exn as x -> printfn "Exception caught: %s" x.Message
                      0

namespace Cipher36

module GLFSR =
    type State = {
        state: bigint
        bit: bool
    }

    // 4 ==> [4, 3] --> [4, *(4 - N)] --> 
    //       [4, 1, 0] --> 0x1_0011

    // let upperBit =
    //     2I ** 4
    
    // let reducerPolinomial = 
    //     2I ** (4 - 4) + 
    //     2I ** (4 - 3) + 
    //     upperBit

    // 8 ==> [8, 6, 5, 4] --> [8, *(8 - N)] --> 
    //       [8, 4, 3, 2, 0] --> 0x1_0001_1101 --> 0x11D (OK)
    
    let upperBit =
        2I ** 8

    let reducerPolinomial = 
        2I ** (8 - 8) + 
        2I ** (8 - 6) + 
        2I ** (8 - 5) +
        2I ** (8 - 4) +
        upperBit

    // 4096 ==> 4096, 4095, 4081, 4069, 0 = 4096, *(4096 - N) =
    //      4096, 27, 15, 1, 0

    // let upperBit =
    //     2I ** 4096

    // let reducerPolinomial = 
    //     2I ** (4096 - 4096) + 
    //     2I ** (4096 - 4095) + 
    //     2I ** (4096 - 4081) +
    //     2I ** (4096 - 4069) +
    //     upperBit

    let reducerByteArray = reducerPolinomial.ToByteArray ()

    let initialize str =
        printfn "Upper bit = %A" upperBit
        printfn "Reducer polinomial = %A" reducerPolinomial
        
        let value = bigint.Parse ("0" + str, System.Globalization.NumberStyles.HexNumber)
    
        { state = value; bit = not value.IsEven }

    let step state =
        let newState = state.state * 2I
        
        if newState.CompareTo(upperBit) < 0
        then 
            { state = newState; bit = not newState.IsEven }
        else
            let stateByteArray = newState.ToByteArray()

            if stateByteArray.Length <> reducerByteArray.Length
                then failwith "Array length are not equal"

            Array.iteri 
                (fun i _ -> stateByteArray.[i] <- stateByteArray.[i] ^^^ reducerByteArray.[i]) 
                stateByteArray

            let result = new bigint(stateByteArray)

            { state = result; bit = not result.IsEven }

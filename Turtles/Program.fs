open Turtles
open System 

[<EntryPoint>]
let main argv = 
    let t1 : Turtle = (10.0, (10.0,10.0))
    let t2 = move 10.0 t1 
    printfn "%A" t2 
    Console.ReadLine() |> ignore
    0 // return an integer exit code

open Turtles
open System 

let printSeq (seq1:seq<Line*Turtle>) = 
    let printer (lt: Line*Turtle)= 
        let (l,t) = lt
        printf "%A \n" l
    Seq.iter printer seq1; 

[<EntryPoint>]
let main _ = 
    let zeroMove = ((0.0f,0.0f),(0.0f,0.0f))
    let t1 : Turtle = (0.0, (200.0,200.0))
    let t2 : Turtle = (-12.0, (300.0,300.0))
//    Seq.unfold myTurtle (step t2) |> printSeq 
    Console.ReadLine() |> ignore
    0 
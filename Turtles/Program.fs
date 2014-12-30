open Turtles
open System 

let step (t: Turtle) : Turtle =
  t 
    |> move 1.0 
    |> turnDeg 1.0

let myTurtle (t: Turtle) = 
   let dir, _= t 
   if dir > Math.PI then 
        None 
   else
        Some(t,step t)

let printSeq seq1 = Seq.iter (printf "%A \n") seq1; 

[<EntryPoint>]
let main argv = 
    let t1 : Turtle = (0.0, (200.0,200.0))
    let t2 : Turtle = (0.0, (300.0,300.0))
    let tseq = Seq.unfold myTurtle (step t1)
    tseq |> printSeq 
//    play t1 (360*10) |> printf "%A" 
//    play t2 (160*2) |>  printf "%A" 
//    play t1 (360*10) |> printf "%A" 
    Console.ReadLine() |> ignore
    0 // return an integer exit code
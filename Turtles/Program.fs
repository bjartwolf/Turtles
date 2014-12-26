open Turtles
open System 
open Painter 

let step (t: Turtle) : Turtle =
  t 
    |> move 1.0 
    |> turnDeg 1.0
    |> move 1.0 

let rec play (t: Turtle) (turns: int) : Turtle= 
   if turns = 0 then printfn "%A" t 
                     t
   else play (step t) (turns - 1)

[<EntryPoint>]
let main argv = 
    let t1 : Turtle = (0.0, (200.0,200.0))
    let t2 : Turtle = (0.0, (300.0,300.0))
    play t1 (360*10) |> printf "%A" 
    play t2 (160*2) |>  printf "%A" 
    play t1 (360*10) |> printf "%A" 
    Console.ReadLine() |> ignore
    0 // return an integer exit code
open Turtles
open System 
type Line = (single*single)*(single*single)

let step (t: Turtle) : Turtle =
  t 
    |> move 1.0 
    |> turnDeg 1.0

let myTurtle (_: Line, t: Turtle) = 
   let dir, _= t 
   let distFromPi = Math.Abs(dir % (2.0*Math.PI))
   if  distFromPi < 0.001 || distFromPi > (2.0*Math.PI-0.001) then 
        None 
   else
        let t' = step t   
        let _,(x,y) = t
        let _,(x',y') = t'
        let lastMove = ((single x,single y),(single x',single y'))
        Some((lastMove,t),(lastMove, t'))

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
    Seq.unfold myTurtle (zeroMove, (step t1)) |> printSeq 
//    Seq.unfold myTurtle (step t2) |> printSeq 
    Console.ReadLine() |> ignore
    0 
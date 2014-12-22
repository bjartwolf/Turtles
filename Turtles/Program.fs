module Turtles
open System

type Dir = double 
type Position = double*double
type Turtle = Dir * Position
type Length = double 
type Angle = double 

let roundN (nrOfdoubles: int)(value:double) = Math.Round(value, nrOfdoubles)
let round5 = roundN 5 
let round10 = roundN 10 

let turn (a:Angle) (t: Turtle): Turtle = 
    let dir,pos = t 
    let dir' = dir + a
    (dir', pos) 

let turn90 = turn (double (Math.PI/2.0))

let turn60 = turn (double (Math.PI/3.0))

let round (digits:int)(t:Turtle): Turtle =
    let dir,(x,y) = t 
    let rounder = roundN digits
    let pos' = (rounder x,rounder y)
    (dir, pos') 

let isSamePosition'ish (digits:int)(t1: Turtle)(t2: Turtle): bool = 
    let t1' = round digits t1 
    let t2' = round digits t2 
    let _, (x1,y1) = t1'
    let _, (x2,y2) = t2'
    x1 = x2 && y1 = y2 

let move (l:Length)(t: Turtle) : Turtle = 
    let dir,(x,y) = t 
    let x' = x + l * cos dir
    let y' = y + l * sin dir
    let pos' = (x',y')
    (dir, pos') 

[<EntryPoint>]
let main argv = 
    let t1 : Turtle = (10.0, (10.0,10.0))
    let t2 = move 10.0 t1 
    printfn "%A" t2 
    Console.ReadLine() |> ignore
    0 // return an integer exit code

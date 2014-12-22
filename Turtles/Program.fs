module Turtles
open System

type Dir = decimal 
type Position = decimal*decimal
type Turtle = Dir * Position
type Length = decimal 
type Angle = decimal 

let roundN (nrOfDecimals: int)(value:decimal) = Math.Round(value, nrOfDecimals)
let round5 = roundN 5 
let round10 = roundN 10 

let dcos (value: decimal) : decimal =
   decimal (cos (double value))

let dsin (value: decimal) : decimal =
   decimal (sin(double value))

let turn (a:Angle) (t: Turtle): Turtle = 
    let dir,pos = t 
    let dir' = dir + a
    (dir', pos) 

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
    let x' = x + l * dcos dir
    let y' = y + l * dsin dir
    let pos' = (x',y')
    (dir, pos') 

[<EntryPoint>]
let main argv = 
    let t1 : Turtle = (10m, (10m,10m))
    let t2 = move 10m t1 
    printfn "%A" t2 
    Console.ReadLine() |> ignore
    0 // return an integer exit code

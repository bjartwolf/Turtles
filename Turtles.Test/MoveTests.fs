module Turtles.Test

open NUnit.Framework
open FsCheck
open FsUnit
open FsCheck.NUnit
open System 
open Turtles

[<TestFixture>] 
type ``Given a LightBulb that has had its state set to true`` ()=
    [<Test>] member x.
     ``when I ask whether it is On it answers true.`` ()=
            true |> should be True
            
[<Property>]
let ``Moving 360n + 180 should move in opposite direction`` (n: int) =
    let t0 = (1m, (0m, 0m))
    let t1 = t0 
                |> move 10m 
    let t2 = t0 
                |> turn (decimal(decimal(n)*decimal(2.0*Math.PI)))
                |> turn (decimal Math.PI)
                |> move 10m 
    let _,(x,y) = t2
    let t3 = 0m,(-x,-y)
    isSamePosition'ish 10 t1 t3

[<Property>]
let ``Given any location, heading 0 moving 90 degrees and forward 4 times end up in same location`` (x: decimal) (y: decimal) = 
    let t1: Turtle = 0m, (x/100m, y/100m)
    let t2 = t1 |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m 
    isSamePosition'ish 4 t1 t2

[<Property>]
let ``Given any location and heading moving 90 degrees and forward 4 times end up in same location to three digits`` (x: decimal) (y: decimal) = 
    let t: Turtle = 1m, (x/100m, y/100m)
    let t1 = t |> move 0m // Just to get the rounding of the move function
    let t2 = t |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m |> turn (decimal (Math.PI/2.0))
               |> move 10m 
    isSamePosition'ish 3 t1 t2

[<Property>]
let ``Moving a multiple of 360 should move forward the same`` (n: int) =
    let t1: Turtle = move 10m (1m, (0m, 0m)) 
    let t2: Turtle = move 10m (1m+decimal(decimal(n)*decimal(2.0*Math.PI)), (0m, 0m)) 
    isSamePosition'ish 3 t1 t2
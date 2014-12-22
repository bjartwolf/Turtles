module Turtles.Test

open NUnit.Framework
open FsCheck
open FsUnit
open FsCheck.NUnit
open System 
open Turtles

[<Property>]
let ``Moving 360n + 180 should move in opposite direction`` (n: int) =
    let t0 = (1.0, (0.0, 0.0)): Turtle
    let t1 = t0 
                |> move 10.0 
    let t2 = t0 |> turn (double n*2.0*Math.PI)
                |> turn Math.PI
                |> move 10.0 
    let _,(x,y) = t2
    let t3 = 0.0,(-x,-y)
    isSamePosition'ish 10 t1 t3

[<Property (Verbose=true)>]
let ``Given any location, heading 0 moving 90 degrees and forward 4 times end up in same location`` (x_dec: decimal) (y_dec: decimal) = 
    let x = double x_dec
    let y = double y_dec
    let y' = max -1e8 (min 1e8 y)
    let x' = max -1e8 (min 1e8 x)
    let t1: Turtle = 0.0, (x', y')
    let t2 = t1 |> move 10.0 |> turn90
                |> move 10.0 |> turn90
                |> move 10.0 |> turn90
                |> move 10.0 
    isSamePosition'ish 5 t1 t2

[<Property (Verbose=true)>]
let ``Given any location and heading moving 90 degrees and forward 4 times end up in same location to three digits`` (x_dec: decimal) (y_dec: decimal) (heading_int: int)= 
    let x = double x_dec
    let y = double y_dec
    let y' = max -1e8 (min 1e8 y)
    let x' = max -1e8 (min 1e8 x)
    let heading = double (heading_int/10000) 
    let t: Turtle = heading, (double (x'), double (y'))
    let t1 = t |> move 0.0 // Just to get the rounding of the move function
    let t2 = t |> move 10.0 |> turn90
               |> move 10.0 |> turn90
               |> move 10.0 |> turn90
               |> move 10.0 
    isSamePosition'ish 7 t1 t2

[<Property>]
let ``Moving a multiple of 360 should move forward the same`` (n: int) =
    let t0: Turtle = (1.0, (0.0, 0.0)) 
    let t1: Turtle = t0 
                        |> move 10.0 
    let t2: Turtle = t0 
                        |> turn ((double n)*2.0*Math.PI)
                        |> move 10.0
    isSamePosition'ish 3 t1 t2
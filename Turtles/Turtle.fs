module Turtles
open System

type Dir = double // The direction of the turtle
type Position = double*double // The x-y position of the turtle
type Turtle = Dir * Position // A turtle state is its direction and position
type Length = double // How long the turtle should move
// Should make this unit of work?
type Angle = double // How much the turtle should turn
type Degrees = double // How much the turtle should turn in degrees

let roundN (nrOfdoubles: int) (value:double) = Math.Round(value, nrOfdoubles)
let round5 = roundN 5 
let round10 = roundN 10 

// Turns the turtle a (radians) degrees
let turn (a:Angle) (t: Turtle) : Turtle = 
    let dir,pos = t 
    let dir' = dir + a
    (dir', pos) 

// Turns the turtle a (radians) degrees
let turnDeg (a:Degrees) (t: Turtle) : Turtle = 
    turn (a * double (Math.PI/180.0)) t

let turn60 = turn (double (Math.PI/3.0))
let turn90 = turn (double (Math.PI/2.0))

// Rounds of the turtle position
let round (digits:int) (t:Turtle) : Turtle =
    let dir,(x,y) = t
    let rounder = roundN digits
    let pos' = (rounder x,rounder y)
    (dir, pos')

let isSamePosition'ish (digits:int) (t1: Turtle) (t2: Turtle): bool =
    let t1' = round digits t1
    let t2' = round digits t2
    let _, (x1,y1) = t1'
    let _, (x2,y2) = t2'
    x1 = x2 && y1 = y2

// Move in the current direction
let move (l:Length)(t: Turtle) : Turtle = 
    let dir,(x,y) = t 
    let x' = x + l * cos dir
    let y' = y + l * sin dir
    let pos' = (x',y')
    (dir, pos')

type Line = (single*single)*(single*single)
type Lines = Line list

let step (t: Turtle) (degreesToTurn: double) : Turtle =
  t 
    |> turnDeg degreesToTurn 
    |> move (0.4 * degreesToTurn)

let turtleLine (t:Turtle) (t': Turtle) : Line = let _,(x,y) = t
                                                let _,(x',y') = t'
                                                let l: Line = ((single x,single y),(single x',single y'))
                                                l

// Sort of importont to be tail recursive
let rec yieldTurtle (edges: int) (t: Turtle): seq<Line option> = 
   let turnDeg = 360.0/ (float edges)
   seq {
        let t' = step t turnDeg  
        yield Some (turtleLine t t')
        let dir, _= t'
        let distFromPi = Math.Abs(dir % (2.0*Math.PI)) in 
        if (distFromPi < 0.01 || distFromPi > (2.0*Math.PI-0.01)) then 
            yield None
        else 
            yield! (yieldTurtle edges t')
    }
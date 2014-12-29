module Painter

open System.Drawing
open System.Windows.Forms

type point = double*double
type line = point*point
type PainterMsg = 
    | Line of line 

let painter =
    MailboxProcessor<PainterMsg>.Start(fun inbox -> 
                let rec loop n =
                    async { let! msg = inbox.Receive()
                            match msg with 
                                | Line(point)  -> let (p1,p2) = point 
                                                  let (x1,y1) = p1
                                                  let (x2,y2) = p2
                                                  let x1 = float32 x1 
                                                  let y1 = float32 y1 
                                                  let x2 = float32 x2 
                                                  let y2 = float32 y2 
                                                  return! loop 0}
                loop 0)

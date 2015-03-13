#r "System.Xml.Linq.dll"
#r "build/Turtles.dll"
open System
open My.Turtles 

let template = """
<?xml version="1.0" encoding="UTF-8" standalone="no"?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->
<svg
   xmlns:dc="http://purl.org/dc/elements/1.1/"
   xmlns:cc="http://creativecommons.org/ns#"
   xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
   xmlns:svg="http://www.w3.org/2000/svg"
   xmlns="http://www.w3.org/2000/svg"
   xmlns:sodipodi="http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd"
   xmlns:inkscape="http://www.inkscape.org/namespaces/inkscape"
   width="3200"
   height="800"
   id="svg2"
   version="1.1"
   inkscape:version="0.91 r13725"
   sodipodi:docname="drawing.svg"
   viewBox="0 0 3200 800">
  <defs
     id="defs4" />
  <sodipodi:namedview
     id="base"
     pagecolor="#ffffff"
     bordercolor="#666666"
     borderopacity="1.0"
     inkscape:pageopacity="0.0"
     inkscape:pageshadow="2"
     inkscape:zoom="0.2065625"
     inkscape:cx="1600"
     inkscape:cy="400"
     inkscape:document-units="px"
     inkscape:current-layer="layer1"
     showgrid="false"
     inkscape:window-width="1920"
     inkscape:window-height="1017"
     inkscape:window-x="-8"
     inkscape:window-y="-8"
     inkscape:window-maximized="1" />
  <metadata
     id="metadata7">
    <rdf:RDF>
      <cc:Work
         rdf:about="">
        <dc:format>image/svg+xml</dc:format>
        <dc:type
           rdf:resource="http://purl.org/dc/dcmitype/StillImage" />
        <dc:title></dc:title>
      </cc:Work>
    </rdf:RDF>
  </metadata>
  <g
     inkscape:label="1-default"
     inkscape:groupmode="layer"
     id="layer1"
     >
    [SVG]
  </g>
</svg>
"""
let turtleLineToSvg (l: Line option)  =
    match l with 
        | None -> ""
        | Some l -> let (x1,y1), (x2,y2) = l
                    let formatter = sprintf @"<line x1=""%f"" y1=""%f"" x2=""%f"" y2=""%f"" style=""stroke:rgb(255,0,0);stroke-width:2""/>" 
                    formatter x1 y1 x2 y2
type Multiplier = One | Two 

let nearStartPos (pos: Position) (startPosition: Position) = 
    let x,y = pos
    let x5 = round5 x
    let y5 = round5 y
    let x', y' = startPosition
    let x'5 = round5 x'
    let y'5 = round5 y'
    x5 = x'5 && y5 = y'5
     
let rec turtlePoly (turning: int) (t: Turtle) (startPosition: Position) (multiplier: Multiplier): seq<Line option*Turtle> = 
   let step = move 100.0
   let multiplier'  = match multiplier with 
                        | One -> Two
                        | Two -> One
   let degreesToTurn  = float turning * 1.0<Degrees> * (match multiplier with
                                                            | One -> 1.0
                                                            | Two -> 2.0)
   seq {
        let t' = step t |> turnDeg degreesToTurn 
        yield (Some (turtleLine t t'), t')
        let t'' = step t' |> turnDeg (2.0*degreesToTurn)
        yield (Some (turtleLine t' t''), t')
        let dir, pos = t''
        if closeToPi dir && nearStartPos pos startPosition then 
            yield (None, t'')
        else 
            yield! (turtlePoly turning t'' startPosition multiplier')
    }
//for x in 1 ..1.. 5 do // 24*15 = 360
//    for y in 1 ..5.. 15 do
let sumPositions (p1: Position) (p2: Position) =
    let x1, y1 = p1
    let x2, y2 = p2
    (x1+x2,y1+y2)
let mutable startPos = (float -200.0, float 80.0)
let incrementPos = (float 220.0, float 35.0)
let degrees = 70 
let mutable svg = "" 
for i in 1 .. 15 do
    startPos <- (sumPositions startPos incrementPos)
    let turtle = turtlePoly degrees (0.0<Radians>, startPos ) startPos One
    svg <- svg + (turtle |> Seq.map (fun (l, _) -> turtleLineToSvg l) |> String.concat "\r\n")
startPos <- (float -200.0, float 320.0)
for i in 1 .. 15 do
    startPos <- (sumPositions startPos (float 220.0, float 35.0))
    let turtle = turtlePoly degrees (0.0<Radians>, startPos ) startPos One
    svg <- svg + (turtle |> Seq.map (fun (l, _) -> turtleLineToSvg l) |> String.concat "\r\n")
startPos <- (float -200.0, float 560.0)
for i in 1 .. 15 do
    startPos <- (sumPositions startPos (float 220.0, float 35.0))
    let turtle = turtlePoly degrees (0.0<Radians>, startPos ) startPos One
    svg <- svg + (turtle |> Seq.map (fun (l, _) -> turtleLineToSvg l) |> String.concat "\r\n")
startPos <- (float -200.0, float -160.0)
for i in 1 .. 15 do
    startPos <- (sumPositions startPos (float 220.0, float 35.0))
    let turtle = turtlePoly degrees (0.0<Radians>, startPos ) startPos One
    svg <- svg + (turtle |> Seq.map (fun (l, _) -> turtleLineToSvg l) |> String.concat "\r\n")
let res = template.Replace("[SVG]", svg)
System.IO.File.WriteAllText("C:\\temp\\turtles.svg", res)
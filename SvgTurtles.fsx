﻿#r "System.Xml.Linq.dll"
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
     transform="translate(0,0)">
    [SVG]
  </g>
</svg>
"""
let turtleLineToSvg (l: Line option)  =
    match l with 
        | None -> ""
        | Some l -> let (x1,y1), (x2,y2) = l
                    let formatter = sprintf @"<line x1=""%.2f"" y1=""%.2f"" x2=""%.2f"" y2=""%.2f"" style=""stroke:rgb(255,0,0);stroke-width:2""/>" 
                    formatter x1 y1 x2 y2
let x = 10
let y = 10
let mutable svg = ""
for x in 1 ..1.. 24 do // 24*15 = 360
    for y in 1 ..2.. 15 do
        let turtle = turtlePoly (x*15+y) (0.0<Radians>, (float (x+1) * 50.0,float (y-1) *50.0))
        let lines = turtle |> Seq.map (fun (l, _) -> turtleLineToSvg l) |> String.concat ""
        svg <- lines + svg
let res = template.Replace("[SVG]", svg)
System.IO.File.WriteAllText("C:\\temp\\turtles.svg", res)
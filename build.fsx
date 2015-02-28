// include Fake libs
#r "tools/FAKE/tools/FakeLib.dll"

open Fake

RestorePackages()
#I "packages/FSharp.Formatting.2.6.1/lib/net40/"
#r "FSharp.CodeFormat.dll"
#r "FSharp.Literate.dll"
open FSharp.Literate
open System.IO

// Directories
let buildDir  = "./build/"
let testDir   = "./test/"

// Filesets
let appReferences  = !! "src/Turtle/Turtles/**/*.fsproj"

let testReferences = !! "src/Test/**/*.fsproj"

// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir]
)

Target "BuildApp" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->  
    !! (testDir + "/Turtles.Test.dll")
        |> NUnit (fun p -> 
            {p with
                DisableShadowCopy = true; 
                OutputFile = testDir + "TestResults.xml"})
)

Target "Docs" (fun _ ->
    let source = "./src/Turtle/Turtles/" 
    let templateSource = "./Docs" 
//    let template = Path.Combine(templateSource, "template.html")
    let template = Path.Combine(templateSource, "template-project.html")
//    let script = Path.Combine(source, "Turtle.fs")
    let projInfo =
      [ "page-description", "F# Turtles"
        "page-author", "Bjørn Einar Bjartnes"
        "github-link", "https://github.com/bjartwolf/turtles"
        "project-name", "F# Turtles" ]
//    Literate.ProcessScriptFile(script, template)
    Literate.ProcessDirectory(source, template, source + "\\output", replacements = projInfo)
//    Literate.ProcessDirectory(source, template, "output")
)
// Build order
"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "NUnitTest"
  ==> "Docs"

// start build
//RunTargetOrDefault "NUnitTest"
RunTargetOrDefault "Docs"

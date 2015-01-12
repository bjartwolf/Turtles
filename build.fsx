// include Fake libs
#r "packages/FAKE/tools/FakeLib.dll"

open Fake

RestorePackages()
// Directories
let buildDir  = "./build/"
let testDir   = "./test/"

// Filesets
let appReferences  = !! "src/Turtle\*.fsproj"

let testReferences = !! "src/Test/**/*.fsproj"

// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir]
)

Target "BuildApp" (fun _ ->

    // compile all projects below src/app/
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

// Build order
"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "NUnitTest"

// start build
RunTargetOrDefault "NUnitTest"

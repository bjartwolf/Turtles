// include Fake libs
#r "tools/FAKE/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build/"
let testDir   = "./test/"

//let testReferences = !! "src/test/**/*.csproj"
let testReferences = !! "Turtles.Test/*.csproj"

// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; testDir]
)

Target "BuildApp" (fun _ ->
    CreateCSharpAssemblyInfo "./src/app/Calculator/Properties/AssemblyInfo.cs"
        [Attribute.Title "Calculator Command line tool"
         Attribute.Description "Sample project for FAKE - F# MAKE"
         Attribute.Guid "A539B42C-CB9F-4a23-8E57-AF4E7CEE5BAA"
         Attribute.Product "Calculator"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./src/app/CalculatorLib/Properties/AssemblyInfo.cs"
        [Attribute.Title "Calculator library"
         Attribute.Description "Sample project for FAKE - F# MAKE"
         Attribute.Guid "EE5621DB-B86B-44eb-987F-9C94BCC98441"
         Attribute.Product "Calculator"
         Attribute.Version version
         Attribute.FileVersion version]

    // compile all projects below src/app/
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->  
    !! (testDir + "/NUnit.Test.*.dll")
        |> NUnit (fun p -> 
            {p with
                DisableShadowCopy = true; 
                OutputFile = testDir + "TestResults.xml"})
)

Target "xUnitTest" (fun _ ->  
    !! (testDir + "/xUnit.Test.*.dll")
        |> xUnit (fun p -> 
            {p with 
                ShadowCopy = false;
                HtmlOutput = true;
                XmlOutput = true;
                OutputDir = testDir })
)

Target "FxCop" (fun _ ->
    !! (buildDir + "/**/*.dll") 
        ++ (buildDir + "/**/*.exe")
        |> FxCop (fun p -> 
            {p with                     
                ReportFileName = testDir + "FXCopResults.xml";
                ToolPath = fxCopRoot})
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*") 
        -- "*.zip" 
        |> Zip buildDir (deployDir + "Calculator." + version + ".zip")
)

// Build order
"Clean"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "FxCop"
  ==> "NUnitTest"
  =?> ("xUnitTest",hasBuildParam "xUnitTest")  // only if FAKE was called with parameter xUnitTest
  ==> "Deploy"

// start build
RunTargetOrDefault "Deploy"
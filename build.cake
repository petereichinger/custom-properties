//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var projectDir = Argument("projectDir",  (string)null);
var targetDir = Argument("projectRelativePath", "Assets/CustomProperties/");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build/");

var releaseDir = buildDir + Directory("bin/Release/");
var outputDirRuntime = buildDir;
var outputDirEditor = outputDirRuntime + Directory("Editor");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});
Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./CustomProperties.sln", settings =>
        settings.SetConfiguration(configuration)
            .WithProperty("OutDir", MakeAbsolute(buildDir).ToString())
        );
});

Task("CopyToProject")
    .IsDependentOn("Build")
    .Does(() =>
{

    if (string.IsNullOrEmpty(projectDir)){
        Error("Specify a project directory\nExample:\n   ./build.ps1 -ScriptArgs '-projectDir=Path/To/Unity/Project' -target CopyToProject");
        Error("Optional: -projectRelativePath to specify a path inside the project where to copy the file");
        throw new ArgumentException("-projectDir is not set");
    } else {
        var runtimePath = Directory(projectDir) + Directory(targetDir);
        Information($"Copying files to: {runtimePath.ToString()}");
        var filesRuntime = GetFiles(buildDir.ToString() + "/CustomProperties.dll") +
                           GetFiles(buildDir.ToString() + "/CustomProperties.xml");
        var filesEditor  = GetFiles(buildDir.ToString() + "/CustomPropertiesEditor.dll") +
                           GetFiles(buildDir.ToString() + "/CustomPropertiesEditor.xml");

        var editorPath = runtimePath + Directory("Editor");

        EnsureDirectoryExists(editorPath);
        CopyFiles(filesRuntime, runtimePath);
        CopyFiles(filesEditor, editorPath);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

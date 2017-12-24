//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

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
        settings.SetConfiguration(configuration));
});

Task("CopyToDir")
    .IsDependentOn("Build")
    .Does(() =>
{
    var filesRuntime = GetFiles(releaseDir + File("CustomProperties.dll")) + GetFiles(releaseDir + File("CustomProperties.xml"));
    var filesEditor = GetFiles(releaseDir + File("CustomPropertiesEditor.dll")) + GetFiles(releaseDir + File("CustomPropertiesEditor.xml"));
    CreateDirectory(outputDirRuntime);
    CreateDirectory(outputDirEditor);

    CopyFiles(filesEditor, outputDirEditor);
    CopyFiles(filesRuntime, outputDirRuntime);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("CopyToDir");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

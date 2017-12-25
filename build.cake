//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
const string PROJECT_PATH_ARGUMENT = "projectPath";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var projectDir = Argument(PROJECT_PATH_ARGUMENT,  (string)null);

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
    MSBuild("./CustomProperties.sln", settings =>
        settings.SetConfiguration(configuration)
            .WithProperty("OutDir", MakeAbsolute(buildDir).ToString())
        );
});

void ShowProjectDirInfo() {
    Error("Specify a project directory");
    Error("Example:");
    if (IsRunningOnWindows())
    {
        Error($"./build.ps1 -ScriptArgs '-{PROJECT_PATH_ARGUMENT}=PathToProject/Assets/CustomProperties/' -target CopyToProject");
    } else {
        Error($"./build.sh -Target=CopyToProject -{PROJECT_PATH_ARGUMENT}=PathToProject/Assets/CustomProperties/");
    }
}

Task("CopyToProject")
    .IsDependentOn("Build")
    .Does(() =>
{

    if (string.IsNullOrEmpty(projectDir)){
        ShowProjectDirInfo();
        throw new ArgumentException($"-{PROJECT_PATH_ARGUMENT} is not set");
    } else {
        var runtimePath = Directory(projectDir);
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

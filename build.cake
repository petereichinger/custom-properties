#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
const string PROJECT_PATH_ARGUMENT = "projectPath";
const string UNITY_PATH_ARGUMENT = "unityPath";
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

var solutionFile = File("./CustomProperties.sln");

var unityAssemblySubDirectoryWindows = Directory("/Editor/Data/Managed");
var unityAssemblySubDirectoryMac = Directory("/Contents/Managed/");
var unityDirectory = (string)null;

var searchPaths = new List<string>{
    "C:/Program Files/Unity 2017.3",
    "C:/Program Files/Unity",
    "/Applications/Unity/Unity.app"
    };
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("FindUnity")
    .IsDependentOn("Clean")
    .Does(() =>
{
    if (HasArgument(UNITY_PATH_ARGUMENT)) {
        unityDirectory = Argument<string>(UNITY_PATH_ARGUMENT);
    } else {
    foreach (var searchPath in searchPaths) {
            if (DirectoryExists(searchPath)) {
                unityDirectory = searchPath;
                break;
            }
        }
    }
    if (string.IsNullOrEmpty(unityDirectory) || !DirectoryExists(unityDirectory)){
        Error("Could not find Unity. Please specify a valid Unity installation with -unityPath argument");
        throw new ArgumentException("Unity not found");
    }
    Information($"Using {unityDirectory}");
});

Task("Build")
    .IsDependentOn("FindUnity")
    .Does(() =>
{

    var referencePath = unityDirectory;
    if (IsRunningOnWindows()){
        referencePath += unityAssemblySubDirectoryWindows;
    } else {
        referencePath += unityAssemblySubDirectoryMac;
    }
    MSBuild(solutionFile, settings =>
        settings.SetVerbosity(Verbosity.Minimal)
            .SetConfiguration(configuration)
            .WithProperty("OutDir", MakeAbsolute(buildDir).ToString())
            .WithProperty("ReferencePath", referencePath)
        );
});

void ShowProjectDirInfo() {
    Error("Specify a project directory");
    Error("Example:");
    if (IsRunningOnWindows()) {
        Error($"./build.ps1 -{PROJECT_PATH_ARGUMENT} PathToProject/Assets/CustomProperties/ -{UNITY_PATH_ARGUMENT} Optional/Unity/Path -target CopyToProject");
    } else {
        Error($"./build.sh -Target=CopyToProject -{PROJECT_PATH_ARGUMENT}=PathToProject/Assets/CustomProperties/ -{UNITY_PATH_ARGUMENT} Optional/Unity/Path");
    }
}

FilePathCollection GenerateFilePathCollection(params FilePath[] paths){
    return new FilePathCollection(paths, PathComparer.Default);
}

Task("CopyToProject")
    .IsDependentOn("Build")
    .Does(() => {
    if (string.IsNullOrEmpty(projectDir)){
        ShowProjectDirInfo();
        throw new ArgumentException($"-{PROJECT_PATH_ARGUMENT} is not set");
    } else {
        var runtimePath = Directory(projectDir);
        var editorPath = runtimePath + Directory("Editor");

        EnsureDirectoryExists(editorPath);

        Information($"Copying files to: {runtimePath.ToString()}");
        var runtimeDll = buildDir + File("CustomProperties.dll");
        var runtimeXml = buildDir + File("CustomProperties.xml");

        var editorDll = buildDir + File("CustomPropertiesEditor.dll");
        var editorXml = buildDir + File("CustomPropertiesEditor.xml");

        var filesRuntime = GenerateFilePathCollection(runtimeDll, runtimeXml);
        var filesEditor = GenerateFilePathCollection(editorDll, editorXml);

        CopyFiles(filesRuntime, runtimePath);
        CopyFiles(filesEditor, editorPath);
    }
});

Task("PrepareRelease")
    .IsDependentOn("Build")
    .Does(()=> {

    Information("Creating ZIP archive: CustomProperties.zip")
    var runtimeDll = buildDir + File("CustomProperties.dll");
    var runtimeXml = buildDir + File("CustomProperties.xml");
    var editorDll = buildDir + File("CustomPropertiesEditor.dll");
    var editorXml = buildDir + File("CustomPropertiesEditor.xml");
    var readme = File("README.md");

    var collection = GenerateFilePathCollection(runtimeDll,runtimeXml,editorDll,editorXml,readme);

    Zip("./", "CustomProperties.zip", collection);
});

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

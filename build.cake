#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
const string PROJECT_PATH_ARGUMENT = "projectPath";
const string UNITY_PATH_ARGUMENT = "unityPath";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build/");

var releaseDir = buildDir + Directory("bin/Release/");

var solutionFile = File("./src/custom-properties.sln");

var unityAssemblySubDirectoryWindows = Directory("/Editor/Data/Managed");
var unityAssemblySubDirectoryMac = Directory("/Contents/Managed/");
var unityDirectory = (string)null;

var searchPaths = new List<string>{
    "C:/Program Files/Unity",
    "/Applications/Unity/Unity.app"
    };
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
    CleanDirectory(buildDir);
});

Task("FindUnity")
    .IsDependentOn("Clean")
    .Does(() => {
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
    .Does(() => {
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
        Error($"./build.ps1 -{PROJECT_PATH_ARGUMENT} PathToProject/Assets/custom-properties/ -{UNITY_PATH_ARGUMENT} Optional/Unity/Path -target CopyToProject");
    } else {
        Error($"./build.sh -Target=CopyToProject -{PROJECT_PATH_ARGUMENT}=PathToProject/Assets/custom-properties/ -{UNITY_PATH_ARGUMENT} Optional/Unity/Path");
    }
}

FilePathCollection GenerateFilePathCollection(params FilePath[] paths){
    return new FilePathCollection(paths, PathComparer.Default);
}

Task("CopyToProject")
    .IsDependentOn("Build")
    .Does(() => {
    if (!HasArgument(PROJECT_PATH_ARGUMENT)) {
        ShowProjectDirInfo();
        throw new ArgumentException($"-{PROJECT_PATH_ARGUMENT} is not set");
    } else {
        var runtimePath = Directory(Argument<string>(PROJECT_PATH_ARGUMENT));
        var editorPath = runtimePath + Directory("Editor");

        EnsureDirectoryExists(editorPath);

        Information($"Copying files to: {runtimePath.ToString()}");
        var runtimeDll = buildDir + File("custom-properties.dll");
        var runtimeXml = buildDir + File("custom-properties.xml");
        var editorDll = buildDir + File("custom-properties-editor.dll");
        var editorXml = buildDir + File("custom-properties-editor.xml");

        var filesRuntime = GenerateFilePathCollection(runtimeDll, runtimeXml);
        var filesEditor = GenerateFilePathCollection(editorDll, editorXml);

        CopyFiles(filesRuntime, runtimePath);
        CopyFiles(filesEditor, editorPath);
    }
});

Task("PrepareRelease")
    .IsDependentOn("Build")
    .Does(()=> {

    Information("Creating ZIP archive: custom-properties.zip");
    var runtimeDll = buildDir + File("custom-properties.dll");
    var runtimeXml = buildDir + File("custom-properties.xml");
    var editorDll = buildDir + File("custom-properties-editor.dll");
    var editorXml = buildDir + File("custom-properties-editor.xml");

    var readme =buildDir + File("README.md") ;
    CopyFile(File("README.md"), readme);

    var collection = GenerateFilePathCollection(runtimeDll,runtimeXml,editorDll,editorXml,readme);

    Zip("./build", "custom-properties.zip", collection);
});

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

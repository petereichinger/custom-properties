#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
const string PROJECT_PATH_ARGUMENT = "project";
const string UNITY_PATH_ARGUMENT = "unity";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build/");

var releaseDir = buildDir + Directory("bin/Release/");

var solutionFile = File("./src/editor-extensions.sln");

var unityAssemblySubDirectoryWindows = Directory("/Editor/Data/Managed");
var unityAssemblySubDirectoryMac = Directory("/Contents/Managed/");
var unityDirectory = (string)null;

var hubPaths = new List<string>{
    "C:/Program Files/Unity/Hub/Editor"
};

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
        foreach (var hubPath in hubPaths){
            if (DirectoryExists(hubPath)){
                var versions = GetSubDirectories(hubPath);
                unityDirectory = versions.Last().ToString();
            }
        }
        if (string.IsNullOrEmpty(unityDirectory)){
            foreach (var searchPath in searchPaths) {
                if (DirectoryExists(searchPath)) {
                    unityDirectory = searchPath;
                    break;
                }
            }
        }
    }
    Information($"Using {unityDirectory}");
    if (string.IsNullOrEmpty(unityDirectory) || !DirectoryExists(unityDirectory)){
        Error($"Could not find Unity. Please specify a valid Unity installation with -{UNITY_PATH_ARGUMENT} argument");
        throw new ArgumentException("Unity not found");
    }
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

FilePathCollection RuntimeFiles() {
    return GenerateFilePathCollection(
        buildDir + File("custom-properties.dll"),
        buildDir + File("custom-properties.xml"));
}

FilePathCollection EditorFiles() {
    return GenerateFilePathCollection(
        buildDir + File("custom-properties-editor.dll"),
        buildDir + File("custom-properties-editor.xml"),
        buildDir + File("enhanced-editor.dll"),
        buildDir + File("enhanced-editor.xml"),
        buildDir + File("transform-editor.dll"),
        buildDir + File("transform-editor.xml"));
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

        CopyFiles(RuntimeFiles(), runtimePath);
        CopyFiles(EditorFiles(), editorPath);
    }
});

Task("PrepareRelease")
    .IsDependentOn("Build")
    .Does(()=> {

    Information("Creating ZIP archive: custom-properties.zip");

    var readme = buildDir + File("README.md") ;
    CopyFile(File("README.md"), readme);

    var collection = EditorFiles() + RuntimeFiles() + readme;

    Zip(buildDir, "custom-properties.zip", collection);
});

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

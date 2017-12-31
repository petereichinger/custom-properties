# Unity Extensions

This is a collection of extensions for the Unity 3D engine.

## Usage

This is only tested in Unity 2017.3. It uses the new .NET runtime.

The repo contains a Visual Studio 2017 project. You can build it yourself or check out the *Releases*.

This contains multiple separate extensions for Unity 3D. They can be all added separately.

## Build


### Visual Studio 

This project contains a Visual Studio 2017 solution file.
The assemblies `UnityEditor.dll` and `UnityEngine.dll` are already referenced and should work when Unity is installed at the default location. 

### Cake

*Usage:*

```
Windows: ./build.ps1
Linux / macOS: ./build.sh
```

This will build the solution and put the results in the `build` folder.

There is a task `CopyToProject` which will copy the build `.dll` and `.xml` files to a Unity project.
See the help message that will be printed for instructions.

There are optional parameters:

|  Parameter   |                                         Meaning                                         |
|--------------|-----------------------------------------------------------------------------------------|
| -project | Path to copy the resulting dll and xml files to. This is useful for testing.            |
| -unity   | Path to a Unity installation. This will force the build to use this Unity installation. |

# Enhanced Editor

File: `custom-editors.dll`

Contains a new editor for all `MonoBehaviour`-derived components. This will optionally display all lists and arrays with a `ReorderableList`. The behaviour can be changed within a preference pane.

# Custom Properties

Files: `custom-properties.dll` and `custom-properties-editor.dll`

Contains a multitude of custom properties.

### EnumFlags

Attribute that draws an enum field as a flag dropdown. The used enum should have the Attribute `System.Flags`.

### ExecuteButton

Attribute that shows a button that executes a method on the object.

### HelpBox

Attribute that shows a help box. This is a decorator.

### Min / Max

These Attributes limit a `float` field to a minimum or maximum respectively.

### MinMax

Interprets `Vector2` and `Vector2Int` values as a minmax range.

### NotNull

Attribute that shows an error box if an object reference is not set.

### NotWhiteSpace

Attribute for strings that shows an error message when the value is empty or whitespace only.

### Readonly

Attribute that displays the value of the field as a label.

### RequireLayer

Attribute for layer masks that shows an error message when no layer is selected.

### Tag

Attribute that can be added to `string` attributes. Instead of the `string` value a dropdown with all available Tags is displayed.

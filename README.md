# Custom Properties for Unity 3D

This is a collection of custom properties for the Unity 3D engine.

## Usage

This is only tested in Unity 2017.2. It uses the new .NET runtime.

The repo contains a Visual Studio 2017 project. You can build it yourself or check out the *Releases*.

The solution consists of two dll projects. One contains the attribute definitions (`Runtime`) and the other the editor code that draws the Attributes. The editor dll must be placed inside an `Editor` folder within Unity.

### Build


#### Visual Studio 

This project contains a Visual Studio 2017 solution file. 

#### Cake

Usage:

```
Windows: ./build.ps1
Linux: ./buildl.ps1
macOS: ./buildm.ps1
```

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

### NotWhitespace

Attribute for strings that shows an error message when the value is empty or whitespace only.

### Readonly

Attribute that displays the value of the field as a label.

### Tag

Attribute that can be added to `string` attributes. Instead of the `string` value a dropdown with all available Tags is displayed.

# Custom Properties for Unity 3D

This is a collection of custom properties for the Unity 3D engine.

## Usage

This is only tested in Unity 2017.2. It uses the new .NET runtime.

The repo contains a Visual Studio 2017 project. You can build it yourself or check out the *Releases*.

### EnumFlags

Attribute that draws an enum field as a flag dropdown. The used enum should have the Attribute `System.Flags`.

### ExecuteButton

Attribute that shows a button that executes a method on the object.

### HelpBox

Attribute that shows a help box. This is a decorator.

### MinMax

Interprets `Vector2` and `Vector2Int` values as a minmax range.

### NotNull

Attribute that shows an error box if an object reference is not set.

### Readonly

Attribute that displays the value of the field as a label.

### Tag

Attribute that can be added to `string` attributes. Instead of the `string` value a dropdown with all available Tags is displayed.

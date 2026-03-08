# ArcConverter
Convert Minecraft Legacy Console .arc files to .zip files and back to .arc

## Usage

```arguments
<Input.arc> <Output.zip> OPTIONAL: <bool>
Input file can be either an zip or arc file
Output file can be an arc or zip file
Boolean is a true or false if the input file should be deleted, used for automation to flip-flop archive types.
```

```cmd
@dotnet ArcConverter.dll MediaWindows64.arc MediaWindows64.zip true
```
Writes to `*.zip`\ and then deletes the .arc file
---

```cmd
@dotnet ArcConverter.dll MediaWindows64.arc MediaWindows64.zip true
```
Writes to `*.arc`\ and then deletes the .zip file
---

## The inspiration to build this project:
[Miku-666](https://github.com/NessieHax)

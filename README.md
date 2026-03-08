# ArcConverter

.net 9 implementation of an arc to zip converter:
Convert Minecraft Legacy Console .arc files to .zip files and back to .arc

## Usage

| Arguments: <Input.arc> <Output.zip> OPTIONAL: \<bool> |
| ---------------------------------------------- |
| Input file can be either an zip or arc file     |
| Output file can be an arc or zip file |
| Delete input file that was converted? |

 >bool is a true or false value, if the input file should be deleted or not.
 This can be used in automation to flip-flop archive types.

---
## Command Line Examples:

```cmd
@dotnet ArcConverter.dll MediaWindows64.arc MediaWindows64.zip true
```
Writes to `*.zip`\
_if bool is true then the utility deletes the .arc file_

---

```cmd
@dotnet ArcConverter.dll MediaWindows64.zip MediaWindows64.arc true
```
Writes to `*.arc`\
_if bool is true then the utility deletes the .zip file_

---

## The inspiration to build this project:

[Miku-666](https://github.com/NessieHax)

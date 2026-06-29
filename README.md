![DL Count](https://img.shields.io/github/downloads/monkheyonepiece/Dragon-Quest-11-3DS-Save-Editor/total.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/88s05mxijfh619at?svg=true)](https://ci.appveyor.com/project/monkheyonepiece/DQ11_3ds)

# Overview
Save data editor in english, french and japanese for Dragon Quest XI (3DS).

[This tool is based on turtle-insect's original project and is maintained as a fork..](https://github.com/turtle-insect/DQ11_3DS)

# Official Website
http://www.dq11.jp/

# Requirements
* Windows
* .NET Framework 4.8
* Extracting save data
* Writing back save data

# Build Environment
* Windows 10 or 11(64bit)
* Visual Studio 2022 or more recent

# Editing Procedure
* Extract the save data
   * This will produce the following files (number refer to the three possible Adventure Log):
      * Data0 (Data1, Data2)
      * mini_0 (mini_1, mini_2)
      * system
* Load `Data0` (`Data1`, `Data2`)
* Perform any desired edits
* Export `Data0` (`Data1`, `Data2`)
* Restore/write the save data back

BetterFuck [![Status Aquana][status-aquana]][andivionian-status-classifier] [![Build status][badge-appveyor]][build-appveyor] [![Build Status][badge-travis]][build-travis]
==========

About
-----
BetterFuck is a Brainfuck language compiler into managed code (CLR). Compiled
programs are ordinary CLR assemblies and therefore can be executed under MS .NET
or Mono runtimes.

Usage
-----
To use compiler, first save your Brainfuck code to plain text file (encoding
does not matter, but it is recommended to use UTF-8 with or without BOM for any
purposes). File extension does not matter either, but it is common practice to
use `.b` extension for Brainfuck sources. If you're new to Brainfuck, see below
for additional information about Brainfuck language.

Then you can run `BFuck.Console` frontend to compile your source. Consider this:

    > ls
    BFuck.Compiler.dll
    BFuck.Console.exe
    BFuck.Runtime.dll
    source.b

Here `BFuck.Compiler.dll`, `BFuck.Console.exe` and `BFuck.Runtime.dll` are
necessary BetterFuck libraries and `source.b` is user-written Brainfuck source
file. For this example, we'll use simple source:

    > cat source.b
    ,+.,+.,+.

This program reads three characters from input stream and prints next character
for each character read.

Compile our program:

    > BFuck.Console source.b
    source.b... ok.

(In non-Windows environments you may experience problems when running CLR
executables from terminal; use `mono BFuck.Console.exe source.b` command then.)

Now we got additional file in our directory: `source.exe`. Let's test it:

    > source.exe
    abcdef

(Here I've entered letters `ace` with my keyboard, and program inserted one
additional character after every my keypress.)

(Same comments about running Mono-related stuff in non-Windows environments
applies here.)

Note that your compiled assembly (`source.exe` in our case) references
`BFuck.Runtime.dll` assembly so it has to be in the same directory for your
program to function properly.

About the language
------------------
BetterFuck runtime machine is a simple collection of memory cells. Every cell is
of CLR type System.Int16 and therefore can store values from 0 to 65535. Every
value can be interpreted as ordinary character and be printed to Unicode-aware
output stream.

Count of cells can be changed when configuring BetterFuck engine; current
default is 256 cells.

In Brainfuck, there are only 8 single-character commands:

1. `+` for incrementing current cell value.

2. `-` for decrementing current cell value.

3. `>` for moving one cell right (forward).

4. `<` for moving one cell left (back).

5. `.` for writing current cell value to output stream.

6. `,` for reading single character from input stream to current cell.

7. `[` for starting loop.

8. `]` for jumping to previous `[` if current cell value is not equal to 0;
   proceeds to next command otherwise.

Source code can include any other characters; they will be dropped before
compilation.

BetterFuck compiler also introduces the concept of comments. Any piece of line
starting with `#` character will be treated as comment till end of line.

[andivionian-status-classifier]: https://github.com/ForNeVeR/andivionian-status-classifier#status-aquana-
[build-appveyor]: https://ci.appveyor.com/project/ForNeVeR/betterfuck/branch/develop
[build-travis]: https://travis-ci.org/ForNeVeR/BetterFuck

[badge-appveyor]: https://ci.appveyor.com/api/projects/status/1cqgyhipyausce5g/branch/develop?svg=true
[badge-travis]: https://travis-ci.org/ForNeVeR/BetterFuck.svg?branch=develop
[status-aquana]: https://img.shields.io/badge/status-aquana-yellowgreen.svg

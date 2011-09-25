BetterFuck
==========

License
=======
Copyright (C) 2011 by ForNeVeR

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

About
=====
BetterFuck is a Brainfuck language compiler into managed code (CLR). Compiled
programs are ordinary CLR assemblies and therefore can be executed under MS .NET
or Mono runtimes.

Usage
=====
To use compiler, first save your Brainfuck code to plain text file (encoding does
not matter, but it is recommended to use UTF-8 with or without BOM for any
purposes). File extension does not matter either, but it is common practice to
use `.b` extension for Brainfuck sources. If you're new to Brainfuck, look next
section for additional information about Brainfuck language.

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
executables from terminal; use `mono BFuck.Console source.b` command then.)

Now we got two additional files in our directory: `source.exe` and `source.mod`.
`source.mod` is main program module containing executable code. `source.exe` is
almost-empty executable module necessary for running main module. Let's test
them:

    > source.exe
    abcdef

(Here I've entered letters `acd` with my keyboard, and program inserted one
additional character after every my keypress.)

(Same comments about running Mono-related stuff in non-Windows environments
applies here.)

Not that your compiled assembly (source.exe in our case) references
BFuck.Runtime assembly so it have to be in the same directory for you program to
function properly.

About the language
==================
BetterFuck runtime machine is simple collection of memory cells. Every cell is of
CLR type System.Char and therefore can store values from 0 to 65535. Every value
can be interpreted as ordinary character and be printed to Unicode-aware output
stream.

Count of cells can be changed when configuring BetterFuck engine; current default
is 256 cells.

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

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
There is still no any user-oriented interface for the compiler. Please come back
later.

About language and runtime
==========================
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

8. `]` for jumping to previous `[` if current cell value is not equal to 0.
proceeds to next command otherwise.

Source code can include any other characters; they will be dropped before
compilation.

BetterFuck compiler also introduces the concept of comments. Any piece of line
starting with `#` character will be treated as comment till end of line.

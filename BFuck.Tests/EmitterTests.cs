﻿/* Copyright (C) 2011 by ForNeVeR
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Diagnostics;
using System.IO;
using BFuck.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BFuck.Tests
{
    /// <summary>
    /// Class for testing BFuck.Compiler.Emitter.
    /// </summary>
    [TestClass]
    public class EmitterTests
    {
        /// <summary>
        /// Test of empty source code compiling.
        /// </summary>
        [TestMethod]
        public void EmptyProgramTest()
        {
            const string fileName = @"EmptyProgram.exe";
            Emitter.Compile(@"EmptyProgram", @"", fileName);
            AssertOutput(fileName, @"");
        }

        /// <summary>
        /// Tests single character output program.
        /// </summary>
        [TestMethod]
        public void OutputTest()
        {
            const string fileName = @"OutputTest.exe";
            Emitter.Compile(@"OutputTest", @"
# Test some commands in comment: + > > < +
++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.", fileName);
            AssertOutput(fileName, "<"); // character with code 60
        }

        /// <summary>
        /// Tests simple loop.
        /// </summary>
        [TestMethod]
        public void LoopTest()
        {
            const string fileName = @"LoopTest.exe";
            Emitter.Compile(@"LoopTest", @"
# Same as above, but with loop.
++++++[>++++++++++<-]>.", fileName);
            AssertOutput(fileName, "<");
        }

        /// <summary>
        /// Checks output of program to be equal to some test value.
        /// </summary>
        /// <param name="fileName">Program to be executed.</param>
        /// <param name="output">Program output.</param>
        private void AssertOutput(string fileName, string output)
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            var bytes = File.ReadAllBytes(fileName);
            var assembly = AppDomain.CurrentDomain.Load(bytes);
            assembly.EntryPoint.Invoke(null, null);

            Assert.AreEqual(output, writer.ToString());
        }
    }
}

using System;
using System.IO;
using BFuck.Compiler;
using Xunit;

namespace BFuck.Tests
{
    /// <summary>
    /// Class for testing BFuck.Compiler.Emitter.
    /// </summary>
    public class EmitterTests
    {
        /// <summary>
        /// Test of empty source code compiling.
        /// </summary>
        [Fact]
        public void EmptyProgramTest()
        {
            const string fileName = @"EmptyProgram.exe";
            Emitter.Compile(@"EmptyProgram", @"", fileName);
            AssertOutput(fileName, @"");
        }

        /// <summary>
        /// Tests single character output program.
        /// </summary>
        [Fact]
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
        [Fact]
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

            Assert.Equal(output, writer.ToString());
        }
    }
}

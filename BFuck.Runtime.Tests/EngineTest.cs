/* Copyright (C) 2011 by ForNeVeR
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BFuck.Runtime.Tests
{
    /// <summary>
    /// Class for testing BFuck.Runtime.Engine.
    /// </summary>
    [TestClass]
    public class EngineTest
    {
        /// <summary>
        /// Tests default memory allocation.
        /// </summary>
        [TestMethod]
        public void AllocationTest()
        {
            const int memorySize = 100;
            var engine = new Engine(memorySize);
            char mem_0 = engine.Get();
            Assert.AreEqual(mem_0, '\0');
            for (int i = 0; i < memorySize; ++i)
            {
                engine.Forward();
                char mem_i = engine.Get();
                Assert.AreEqual(mem_i, '\0');
            }
        }

        /// <summary>
        /// Tests memory operations.
        /// </summary>
        [TestMethod]
        public void DataTest()
        {
            const int iterations = 100;
            var engine = new Engine(1);
            for (int i = 0; i < iterations; ++i)
            {
                engine.Add();
                Assert.AreEqual(engine.Get(), (char) i + 1);
            }
            for (int i = 0; i < iterations; ++i)
            {
                engine.Dec();
                Assert.AreEqual(engine.Get(), (char) iterations - i - 1);
            }
            Assert.AreEqual(engine.Get(), '\0');
        }

        /// <summary>
        /// Tests moving through memory.
        /// </summary>
        [TestMethod]
        public void MovingTest()
        {
            const int memorySize = 100;
            var engine = new Engine(memorySize);
            engine.Add();
            for (int i = 0; i < memorySize; ++i)
            {
                engine.Forward();
            }
            Assert.AreEqual(engine.Get(), '\x01');
        }
    }
}

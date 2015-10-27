using BFuck.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BFuck.Tests
{
    /// <summary>
    /// Class for testing BFuck.Runtime.Engine.
    /// </summary>
    [TestClass]
    public class EngineTests
    {
        /// <summary>
        /// Tests default memory allocation.
        /// </summary>
        [TestMethod]
        public void AllocationTest()
        {
            const int memorySize = 100;
            var engine = new Engine(memorySize);
            Assert.AreEqual(engine.Get(), 0);
            for (int i = 0; i < memorySize; ++i)
            {
                engine.Forward();
                Assert.AreEqual(engine.Get(), 0);
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
                Assert.AreEqual(engine.Get(), i + 1);
            }
            for (int i = 0; i < iterations; ++i)
            {
                engine.Dec();
                Assert.AreEqual(engine.Get(), iterations - i - 1);
            }
            Assert.AreEqual(engine.Get(), 0);
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
            Assert.AreEqual(engine.Get(), 1);
        }
    }
}

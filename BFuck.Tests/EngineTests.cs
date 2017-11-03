using BFuck.Runtime;
using Xunit;

namespace BFuck.Tests
{
    /// <summary>
    /// Class for testing BFuck.Runtime.Engine.
    /// </summary>
    public class EngineTests
    {
        /// <summary>
        /// Tests default memory allocation.
        /// </summary>
        [Fact]
        public void AllocationTest()
        {
            const int memorySize = 100;
            var engine = new Engine(memorySize);
            Assert.Equal(engine.Get(), 0);
            for (int i = 0; i < memorySize; ++i)
            {
                engine.Forward();
                Assert.Equal(engine.Get(), 0);
            }
        }

        /// <summary>
        /// Tests memory operations.
        /// </summary>
        [Fact]
        public void DataTest()
        {
            const int iterations = 100;
            var engine = new Engine(1);
            for (int i = 0; i < iterations; ++i)
            {
                engine.Add();
                Assert.Equal(engine.Get(), i + 1);
            }
            for (int i = 0; i < iterations; ++i)
            {
                engine.Dec();
                Assert.Equal(engine.Get(), iterations - i - 1);
            }
            Assert.Equal(engine.Get(), 0);
        }

        /// <summary>
        /// Tests moving through memory.
        /// </summary>
        [Fact]
        public void MovingTest()
        {
            const int memorySize = 100;
            var engine = new Engine(memorySize);
            engine.Add();
            for (int i = 0; i < memorySize; ++i)
            {
                engine.Forward();
            }
            Assert.Equal(engine.Get(), 1);
        }
    }
}

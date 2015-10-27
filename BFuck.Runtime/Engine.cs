using System;
using System.Collections.Generic;
using System.Linq;

namespace BFuck.Runtime
{
    /// <summary>
    /// Engine of BetterFuck interpreter.
    /// </summary>
    public class Engine
    {
        #region Private data

        /// <summary>
        /// Index of current selected cell.
        /// </summary>
        private int _pointer;

        /// <summary>
        /// Memory size in value units.
        /// </summary>
        private readonly int _memorySize;

        /// <summary>
        /// List of memory values.
        /// </summary>
        private readonly List<byte> _memory;

        /// <summary>
        /// Dictionary for pbrain procedures.
        /// </summary>
        private readonly Dictionary<short, Action> _procedures;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of BFuck engine.
        /// </summary>
        /// <param name="memorySize">Size of BFuck memory.</param>
        public Engine(int memorySize)
        {
            _memorySize = memorySize;
            _memory = new List<byte>();
            _procedures = new Dictionary<short, Action>();
        }

        #endregion
        
        #region Cell movement
        
        /// <summary>
        /// Moves pointer one cell back.
        /// </summary>
        public void Back()
        {
            --_pointer;
            if (_pointer < 0)
                _pointer = _memorySize - 1;
        }

        /// <summary>
        /// Moves pointer one cell forward.
        /// </summary>
        public void Forward()
        {
            ++_pointer;
            if (_pointer >= _memorySize)
                _pointer = 0;
        }

        #endregion

        #region Data access

        /// <summary>
        /// Adds one to current cell value.
        /// </summary>
        public void Add()
        {
            EnsureMemoryExpanded();
            _memory[_pointer] += 1;
        }

        /// <summary>
        /// Decrements current cell value by one.
        /// </summary>
        public void Dec()
        {
            EnsureMemoryExpanded();
            _memory[_pointer] -= 1;
        }

        /// <summary>
        /// Returns value from current cell.
        /// </summary>
        public short Get()
        {
            if (_memory.Count > _pointer)
                return _memory[_pointer];
            else
                return 0;
        }

        #endregion

        #region IO operations

        /// <summary>
        /// Prints current cell value to output stream as printable character.
        /// </summary>
        public void Out()
        {
            EnsureMemoryExpanded();
            Console.Write((char) _memory[_pointer]);
        }

        /// <summary>
        /// Reads one character from input stream and puts into current cell.
        /// </summary>
        public void In()
        {
            EnsureMemoryExpanded();
            _memory[_pointer] = (byte) Console.ReadKey().KeyChar;
        }

        #endregion

        #region pbrain support

        /// <summary>
        /// Associates a procedure with code taken from current cell.
        /// </summary>
        /// <param name="procedure">Procedure to register.</param>
        public void Register(Action procedure)
        {
            _procedures[Get()] = procedure;
        }

        /// <summary>
        /// Calls a procedure associated with code taken from current cell.
        /// </summary>
        public void Call()
        {
            _procedures[Get()]();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Ensures memory size is enough for accessing data in current cell.
        /// </summary>
        private void EnsureMemoryExpanded()
        {
            if (_pointer >= _memory.Count)
            {
                int difference = _pointer - _memory.Count + 1;
                _memory.AddRange(Enumerable.Repeat((byte) 0, difference));
            }
        }

        #endregion
    }
}

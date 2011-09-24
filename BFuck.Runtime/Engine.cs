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
        private readonly List<char> _memory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of BFuck engine.
        /// </summary>
        /// <param name="memorySize">Size of BFuck memory.</param>
        public Engine(int memorySize)
        {
            _memorySize = memorySize;
            _memory = new List<char>();
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
            _memory[_pointer] += (char) 1;
        }

        /// <summary>
        /// Decrements current cell value by one.
        /// </summary>
        public void Dec()
        {
            EnsureMemoryExpanded();
            _memory[_pointer] -= (char)1;
        }

        /// <summary>
        /// Returns value from current cell.
        /// </summary>
        public char Get()
        {
            EnsureMemoryExpanded();
            return _memory[_pointer];
        }

        #endregion

        #region IO operations

        /// <summary>
        /// Prints current cell value to output stream as printable character.
        /// </summary>
        public void Out()
        {
            EnsureMemoryExpanded();
            Console.Write(_memory[_pointer]);
        }

        /// <summary>
        /// Reads one character from input stream and puts into current cell.
        /// </summary>
        public void In()
        {
            EnsureMemoryExpanded();
            _memory[_pointer] = Console.ReadKey().KeyChar;
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
                _memory.AddRange(Enumerable.Repeat(default(char), difference));
            }
        }

        #endregion
    }
}

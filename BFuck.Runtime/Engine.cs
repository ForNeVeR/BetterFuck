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

namespace BFuck.Runtime
{
    /// <summary>
    /// Engine of BetterFuck interpreter.
    /// </summary>
    /// <typeparam name="TValue">Type of underlying memory cells.</typeparam>
    public class Engine<TValue>
    {
        #region Cell movement
        
        /// <summary>
        /// Moves pointer one cell back.
        /// </summary>
        public void Back()
        {
            
        }

        /// <summary>
        /// Moves pointer one cell forward.
        /// </summary>
        public void Forward()
        {
            
        }

        #endregion

        #region Data management

        /// <summary>
        /// Adds one to current cell value.
        /// </summary>
        public void Add()
        {
            
        }

        /// <summary>
        /// Decrements current cell value by one.
        /// </summary>
        public void Dec()
        {
            
        }

        #endregion

        #region IO operations

        /// <summary>
        /// Prints current cell value to output stream as printable character.
        /// </summary>
        public void Out()
        {
            
        }

        /// <summary>
        /// Reads one character from input stream and puts into current cell.
        /// </summary>
        public void In()
        {
            
        }

        /// <summary>
        /// Returns value from current cell.
        /// </summary>
        public TValue Get()
        {
            return default(TValue);
        }

        #endregion
    }
}

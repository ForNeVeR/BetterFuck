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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using BFuck.Runtime;

namespace BFuck.Compiler
{
    /// <summary>
    /// Emitter class emits commands to target assembly.
    /// </summary>
    public static class Emitter
    {
        /// <summary>
        /// Default memory size for created engines, characters.
        /// </summary>
        private const int DefaultMemorySize = 256;

        /// <summary>
        /// Compiles <paramref name="source"/> and prepares in-memory assembly.
        /// </summary>
        public static void Compile(string name, string source, string fileName)
        {
            source = StripComments(source);

            var assemblyName = new AssemblyName(name);
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.Save);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, fileName);
            var typeBuilder = moduleBuilder.DefineType("Program",
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit);
            var methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static,
                typeof(void), new Type[0]);

            assemblyBuilder.SetEntryPoint(methodBuilder);
            var ilGenerator = methodBuilder.GetILGenerator();

            ProduceInitCode(ilGenerator);
            ProduceCode(source, ilGenerator);
            ProduceFinalCode(ilGenerator);

            typeBuilder.CreateType();
            assemblyBuilder.Save(fileName);
        }

        /// <summary>
        /// Deletes comments from <paramref name="source"/> string.
        /// </summary>
        /// <returns>Plain code without unnecessary characters.</returns>
        private static string StripComments(string source)
        {
            var lines = source.Split('\n');

            var result = new StringBuilder();
            foreach (var line in lines)
            {
                foreach (var character in line)
                {
                    if (new[] { '+', '-', '>', '<', '.', ',', '[', ']' }.Contains(character))
                        result.Append(character);
                    else if (character == '#')
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Produces BetterFuck initialization code to specified generator.
        /// </summary>
        private static void ProduceInitCode(ILGenerator ilGenerator)
        {
            ilGenerator.BeginScope();

            ilGenerator.DeclareLocal(typeof(Engine));
            ilGenerator.Emit(OpCodes.Ldc_I4, DefaultMemorySize);

            var constructorInfo = typeof(Engine).GetConstructor(new[] { typeof(int) });
            ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        /// <summary>
        /// Produces BetterFuck finalization code for Main method in target assembly.
        /// </summary>
        /// <param name="ilGenerator"></param>
        private static void ProduceFinalCode(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ret);
            ilGenerator.EndScope();
        }

        /// <summary>
        /// Emits code of specified source fragment to specified generator.
        /// </summary>
        /// <param name="code">Brainfick source code.</param>
        /// <param name="ilGenerator">IL generator.</param>
        private static void ProduceCode(string code, ILGenerator ilGenerator)
        {
            for (int i = 0; i < code.Length; ++i)
            {
                ilGenerator.Emit(OpCodes.Ldloc_0);
                switch (code[i])
                {
                    case '+':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Add"), null);
                        break;
                    case '-':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Dec"), null);
                        break;
                    case '>':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Forward"), null);
                        break;
                    case '<':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Back"), null);
                        break;
                    case '.':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Out"), null);
                        break;
                    case ',':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("In"), null);
                        break;
                    case '[':
                        var label = ilGenerator.DefineLabel();
                        int endLoop = code.IndexOf(']', i + 1);
                        if (endLoop == -1)
                            throw new Exception(String.Format("Square bracket at index {0} is not closed.", i));
                        string innerCode = code.Substring(i, endLoop - i - 1);
                        ProduceCode(innerCode, ilGenerator);
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Get"), null);
                        ilGenerator.Emit(OpCodes.Ldc_I4, 0);
                        ilGenerator.Emit(OpCodes.Beq, label);
                        i = endLoop + 1;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported operation required.");
                }
            }
        }
    }
}

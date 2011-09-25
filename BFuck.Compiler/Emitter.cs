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
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            var typeBuilder = moduleBuilder.DefineType("BetterFuck", TypeAttributes.Public | TypeAttributes.Class);
            var methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static,
                typeof(void), new Type[0]);
            var ilGenerator = methodBuilder.GetILGenerator();

            ProduceInitCode(ilGenerator);
            ProduceCode(source, ilGenerator);
            ProduceFinalCode(ilGenerator);

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
                    if (new[] { '+', '-', '>', '<', '.', ',' }.Contains(character))
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
            ilGenerator.Emit(OpCodes.Ldarg, DefaultMemorySize);

            var constructorInfo = typeof (Engine).GetConstructor(new[] {typeof (int)});
            ilGenerator.Emit(OpCodes.Call, constructorInfo);
        }

        /// <summary>
        /// Produces BetterFuck finalization code for Main method in target assembly.
        /// </summary>
        /// <param name="ilGenerator"></param>
        private static void ProduceFinalCode(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ret);
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
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Forward"), null);
                        break;
                    case '.':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("Out"), null);
                        break;
                    case ',':
                        ilGenerator.EmitCall(OpCodes.Call, typeof(Engine).GetMethod("In"), null);
                        break;
                    case '[':
                    case ']':
                        // TODO: Add loop implementation.
                    default:
                        throw new NotSupportedException("Unsupported operation queried.");
                }
            }
        }
    }
}

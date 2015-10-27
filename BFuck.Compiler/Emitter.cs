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
        /// All commands available for compiler.
        /// </summary>
        private static readonly char[] Commands = new[] {'+', '-', '>', '<', '.', ',', '[', ']', '(', ')'};

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
            ProduceCode(source, typeBuilder, ilGenerator);
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
                    if (Commands.Contains(character))
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
        /// <param name="typeBuilder">Type builder for target assembly.</param>
        /// <param name="ilGenerator">IL generator.</param>
        private static void ProduceCode(string code, TypeBuilder typeBuilder, ILGenerator ilGenerator)
        {
            for (int i = 0; i < code.Length; ++i)
            {
                switch (code[i])
                {
                    case '+':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Add"), null);
                        break;
                    case '-':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Dec"), null);
                        break;
                    case '>':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Forward"), null);
                        break;
                    case '<':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Back"), null);
                        break;
                    case '.':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Out"), null);
                        break;
                    case ',':
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("In"), null);
                        break;
                    case '[':
                        var loopStartLabel = ilGenerator.DefineLabel();
                        ilGenerator.MarkLabel(loopStartLabel);
                        string innerBlock = GetCodeBlock(code, i);
                        ProduceCode(innerBlock, typeBuilder, ilGenerator);
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, typeof (Engine).GetMethod("Get"), null);
                        ilGenerator.Emit(OpCodes.Ldc_I4, 0);
                        ilGenerator.Emit(OpCodes.Bne_Un, loopStartLabel);
                        i += innerBlock.Length + 1;
                        break;
                    case '(':
                        var procedureBody = GetCodeBlock(code, i);
                        // TODO: Invent better naming scheme.
                        var methodBuilder = typeBuilder.DefineMethod("procedure_" + procedureBody,
                            MethodAttributes.Private | MethodAttributes.Static, typeof (void), new[] {typeof (Engine)});

                        var procedureIlGenerator = methodBuilder.GetILGenerator();
                        procedureIlGenerator.Emit(OpCodes.Ldarg_0);
                        procedureIlGenerator.DeclareLocal(typeof(Engine));
                        procedureIlGenerator.Emit(OpCodes.Stloc_0);
                        ProduceCode(procedureBody, typeBuilder, procedureIlGenerator);

                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.EmitCall(OpCodes.Call, methodBuilder, null);

                        i += procedureBody.Length + 1;
                        break;
                    default:
                        throw new NotSupportedException("Unsupported operation required.");
                }
            }
        }

        /// <summary>
        /// Extracts code block starting at index <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="code">Code from which block will be extracted.</param>
        /// <param name="startIndex">Index of code starting token (opening brace).</param>
        /// <returns>Code block without braces.</returns>
        private static string GetCodeBlock(string code, int startIndex)
        {
            int closingBraceIndex = FindClosingBrace(code, startIndex);
            string innerBlock = code.Substring(startIndex + 1, closingBraceIndex - startIndex - 1);
            return innerBlock;
        }

        /// <summary>
        /// Finds closing brace in <paramref name="code"/> for brace at
        /// <paramref name="code"/>[<paramref name="index"/>].
        /// </summary>
        /// <param name="code">Code to search for brace.</param>
        /// <param name="index">Index of opening brace.</param>
        /// <returns></returns>
        private static int FindClosingBrace(string code, int index)
        {
            char openingBrace = code[index];
            char closingBrace;
            if (openingBrace == '[')
            {
                closingBrace = ']';
            }
            else if (openingBrace == '(')
            {
                closingBrace = ')';
            }
            else
                throw new Exception("Not opened brace.");

            for (int i = index + 1; i < code.Length; ++i)
            {
                if (code[i] == openingBrace)
                    i = FindClosingBrace(code, i);
                else if (code[i] == closingBrace)
                    return i;
            }

            throw new Exception(string.Format(
                "Cannot find closing brace for brace at position {0} in code fragment \"{1}\".", index, code));
        }
    }
}

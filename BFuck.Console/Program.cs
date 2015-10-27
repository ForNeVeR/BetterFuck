using System.IO;
using BFuck.Compiler;

namespace BFuck.Console
{
    /// <summary>
    /// Main class for console compiler.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for console compiler.
        /// </summary>
        /// <param name="args">List of command line arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length < 1 || args[0] == @"--help")
            {
                PrintHelp();
                return;
            }

            foreach (var arg in args)
            {
                System.Console.Write("{0}... ", arg);
                string programName = Path.GetFileNameWithoutExtension(arg);
                string source = File.ReadAllText(arg);
                Emitter.Compile(programName, source, string.Format(@"{0}.exe", programName));
                System.Console.WriteLine("ok.");
            }
        }

        private static void PrintHelp()
        {
            System.Console.WriteLine(@"BetterFuck Compiler console frontend.

Usage:
    BFuck.Console [arg1, arg2, ...]
where [arg1, arg2, ...] - paths to source files. Resulting assemblies and
modules will be saved in current directory with .exe and .mod extensions. Do
not forget to distribute BFuck.Runtime.dll assembly with your assemblies!");
        }
    }
}

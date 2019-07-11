using LoxInterpreter.Interpreters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoxInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage : lox <script>");
            }
            else if (args.Length == 1)
            {
                var task = RunFile(args[0]);
                task.Wait();
            }
            else
            {
                RunPrompt();
            }
        }

        private static async Task RunFile(string file)
        {
            Run(await File.ReadAllTextAsync(file));
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                Run(Console.ReadLine());
            }
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            foreach(var token in scanner.ScanTokents())
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}

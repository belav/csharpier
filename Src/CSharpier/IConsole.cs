using System;
using System.Text;

namespace CSharpier
{
    public interface IConsole
    {
        void WriteLine(string? line);
        void Write(string value);
        void WriteWithColor(string value, ConsoleColor color);
        Encoding InputEncoding { get; }
    }

    public class SystemConsole : IConsole
    {
        private readonly ConsoleColor originalColor = Console.ForegroundColor;

        public void WriteLine(string? line)
        {
            Console.WriteLine(line);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteWithColor(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = this.originalColor;
        }

        public Encoding InputEncoding => Console.InputEncoding;
    }
}

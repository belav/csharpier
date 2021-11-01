using System;
using System.Text;

namespace CSharpier
{
    public interface IConsole
    {
        void WriteLine(string? line = null);
        void Write(string value);
        Encoding InputEncoding { get; }
        ConsoleColor ForegroundColor { set; }
        void ResetColor();
    }

    public class SystemConsole : IConsole
    {
        public void WriteLine(string? line = null)
        {
            Console.WriteLine(line);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public ConsoleColor ForegroundColor
        {
            set => Console.ForegroundColor = value;
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public Encoding InputEncoding => Console.InputEncoding;
    }
}

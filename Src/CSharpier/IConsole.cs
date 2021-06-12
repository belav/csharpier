using System;

namespace CSharpier
{
    public interface IConsole
    {
        void WriteLine(string? line);
        void Write(string value);
    }

    public class SystemConsole : IConsole
    {
        public void WriteLine(string? line)
        {
            Console.WriteLine(line);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}

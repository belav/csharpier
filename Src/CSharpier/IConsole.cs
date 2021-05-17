using System;

namespace CSharpier
{
    public interface IConsole
    {
        void WriteLine(string? line);
    }

    public class SystemConsole : IConsole
    {
        public void WriteLine(string? line)
        {
            Console.WriteLine(line);
        }
    }
}

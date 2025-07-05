using System;

namespace CSharpier.VisualStudio
{
    using System.Linq;

    public static class FunctionRunner
    {
        public static string? RunUntilNonNull(params Func<string?>[] functions)
        {
            return functions
                .Select(possibleFunction => possibleFunction())
                .OfType<string>()
                .FirstOrDefault();
        }
    }
}

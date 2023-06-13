namespace Ignore
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A holder for a regex and replacer function.
    /// The function is invoked if the input matches the regex.
    /// </summary>
    public class Replacer
    {
        private readonly string name;

        private readonly Regex regex;

        private readonly MatchEvaluator matchEvaluator;

        public Replacer(string name, Regex regex, Func<Match, string> replacer)
        {
            this.name = name;
            this.regex = regex;
            matchEvaluator = new MatchEvaluator(replacer);
        }

        public string Invoke(string pattern)
        {
            return regex.Replace(pattern, matchEvaluator);
        }

        public override string ToString()
        {
            return name;
        }
    }
}

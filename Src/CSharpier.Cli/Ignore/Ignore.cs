namespace Ignore
{
    using System.Collections.Generic;
    using System.Linq;

    public class Ignore
    {
        private readonly List<IgnoreRule> rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ignore"/> class.
        /// </summary>
        public Ignore()
        {
            rules = new List<IgnoreRule>();
            OriginalRules = new List<string>();
        }

        /// <summary>
        /// Gets the list of the original rules passed in to the class ctor.
        /// </summary>
        public List<string> OriginalRules { get; }

        /// <summary>
        /// Adds the given pattern to this <see cref="Ignore"/> instance.
        /// </summary>
        /// <param name="rule">Gitignore style pattern string.</param>
        /// <returns>Current instance of <see cref="Ignore"/>.</returns>
        public Ignore Add(string rule)
        {
            OriginalRules.Add(rule);
            rules.Add(new IgnoreRule(rule));
            return this;
        }

        /// <summary>
        /// Adds the given pattern list to this <see cref="Ignore"/> instance.
        /// </summary>
        /// <param name="patterns">List of gitignore style pattern strings.</param>
        /// <returns>Current instance of <see cref="Ignore"/>.</returns>
        public Ignore Add(IEnumerable<string> patterns)
        {
            var patternList = patterns.ToList();
            OriginalRules.AddRange(patternList);
            patternList.ForEach(pattern => rules.Add(new IgnoreRule(pattern)));
            return this;
        }

        /// <summary>
        /// Test whether the input path is ignored as per the rules
        /// specified in the class ctor.
        /// </summary>
        /// <param name="path">File path to consider.</param>
        /// <returns>A boolean indicating if the path is ignored.</returns>
        public bool IsIgnored(string path)
        {
            var ignore = IsPathIgnored(path);

            return ignore;
        }

        /// <summary>
        /// Filters the input paths as per the rules specified in the
        /// class ctor.
        /// </summary>
        /// <param name="paths">List of input file paths.</param>
        /// <returns>List of filtered paths (the paths that are not ignored).</returns>
        public IEnumerable<string> Filter(IEnumerable<string> paths)
        {
            var filteredPaths = new List<string>();
            foreach (var path in paths)
            {
                var ignore = IsPathIgnored(path);
                if (ignore == false)
                {
                    filteredPaths.Add(path);
                }
            }

            return filteredPaths;
        }

        private bool IsPathIgnored(string path)
        {
            var ignore = false;
            foreach (var rule in rules)
            {
                if (rule.Negate)
                {
                    if (ignore && rule.IsMatch(path))
                    {
                        ignore = false;
                    }
                }
                else if (!ignore && rule.IsMatch(path))
                {
                    ignore = true;
                }
            }

            return ignore;
        }
    }
}

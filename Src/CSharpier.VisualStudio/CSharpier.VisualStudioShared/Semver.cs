namespace CSharpier.VisualStudio
{
    using System;
    using NuGet.Versioning;

    public static class Semver
    {
        public static bool GTE(string version, string otherVersion)
        {
            return SemanticVersion.Parse(version).CompareTo(SemanticVersion.Parse(otherVersion))
                >= 0;
        }
    }
}

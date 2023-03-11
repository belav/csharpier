namespace CSharpier.VisualStudio
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string value, string containsValue)
        {
            return value.IndexOf(containsValue, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}

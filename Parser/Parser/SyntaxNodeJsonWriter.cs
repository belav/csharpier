using System;
using System.IO;
using Newtonsoft.Json;

namespace Parser
{
    public partial class SyntaxNodeJsonWriter
    {
        private static string WriteBoolean(string name, bool value)
        {
            if (value)
            {
                return $"\"{name}\":true";
            }

            return null;
        }

        private static string WriteString(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return $"\"{name}\":{JsonConvert.ToString(value, '"', StringEscapeHandling.Default)}";
            }

            return null;
        }

        private static string WriteInt(string name, int value)
        {
            if (value != 0)
            {
                return $"\"{name}\":{value}";   
            }

            return null;
        }
    }
}
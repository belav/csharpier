using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CSharpier.Playground
{
    public static class StringExtensions
    {
        public static void EnsureDirectoryExists(this FileInfo fileInfo)
        {
            fileInfo.Directory.EnsureExists();
        }
        
        public static void EnsureExists(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Name.EndsWith("$"))
            {
                return;
            }

            directoryInfo.Parent?.EnsureExists();

            if (directoryInfo.Exists)
            {
                return;
            }

            try
            {
                directoryInfo.Create();
            }
            catch (Exception ex)
            {
                throw new Exception("It was not possible to create the path " + directoryInfo.FullName, ex);
            }
        }

        public static string CalculateHash(this string value)
        {
            using var hasher = MD5.Create();
            var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
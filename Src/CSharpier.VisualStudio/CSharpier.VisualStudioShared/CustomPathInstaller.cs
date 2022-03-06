using System;
using System.IO;

namespace CSharpier.VisualStudio
{
    public class CustomPathInstaller
    {
        private readonly Logger logger;

        private static CustomPathInstaller? instance;

        public static CustomPathInstaller GetInstance(CSharpierPackage package)
        {
            return instance ??= new CustomPathInstaller(package);
        }

        private CustomPathInstaller(CSharpierPackage package)
        {
            this.logger = Logger.Instance;
        }

        public void EnsureVersionInstalled(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return;
            }
            var directoryForVersion = this.GetDirectoryForVersion(version);
            if (Directory.Exists(directoryForVersion))
            {
                this.logger.Debug("Directory at " + directoryForVersion + " already exists");
                return;
            }

            var arguments =
                $"tool install csharpier --version {version} --tool-path \"{directoryForVersion}\" ";
            ProcessHelper.ExecuteCommand("dotnet", arguments);
        }

        public string GetPathForVersion(string version)
        {
            return Path.Combine(this.GetDirectoryForVersion(version), "dotnet-csharpier");
        }

        private string GetDirectoryForVersion(string version)
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CSharpier",
                version
            );
        }
    }
}

using System;
using System.Collections.Generic;
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
            var pathToDirectoryForVersion = this.GetDirectoryForVersion(version);
            if (Directory.Exists(pathToDirectoryForVersion))
            {
                try
                {
                    var env = new Dictionary<string, string> { { "DOTNET_NOLOGO", "1" } };

                    var versionFromCommand = ProcessHelper.ExecuteCommand(
                        this.GetPathForVersion(version),
                        "--version",
                        env
                    );

                    this.logger.Debug("dotnet csharpier --version output: " + versionFromCommand);

                    if (versionFromCommand.Equals(version))
                    {
                        this.logger.Debug(
                            "CSharpier at " + pathToDirectoryForVersion + " already exists"
                        );
                        return;
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Warn(
                        "Exception while running 'dotnet csharpier --version' in "
                            + pathToDirectoryForVersion
                    );
                    this.logger.Error(ex);
                }

                // if we got here something isn't right in the current directory
                Directory.Delete(pathToDirectoryForVersion, true);
            }

            var arguments =
                $"tool install csharpier --version {version} --tool-path \"{pathToDirectoryForVersion}\" ";
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

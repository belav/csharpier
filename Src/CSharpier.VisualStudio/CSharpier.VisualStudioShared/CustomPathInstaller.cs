using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpier.VisualStudio
{
    public class CustomPathInstaller
    {
        private readonly Logger logger;
        private readonly string customPath;

        private static CustomPathInstaller? instance;

        public static CustomPathInstaller GetInstance(CSharpierPackage package)
        {
            return instance ??= new CustomPathInstaller(package);
        }

        private CustomPathInstaller(CSharpierPackage package)
        {
            this.logger = Logger.Instance;
            this.customPath = CSharpierOptions.Instance.CustomPath ?? string.Empty;
        }

        public bool EnsureVersionInstalled(string version, string directory)
        {
            if (string.IsNullOrEmpty(version))
            {
                return true;
            }

            if (this.customPath != "")
            {
                this.logger.Debug("Using csharpier at a custom path of " + this.customPath);
                return true;
            }

            var pathToDirectoryForVersion = this.GetDirectoryForVersion(version);
            if (Directory.Exists(pathToDirectoryForVersion))
            {
                if (this.ValidateInstall(pathToDirectoryForVersion, version))
                {
                    return true;
                }

                this.logger.Debug(
                    $"Removing directory at {pathToDirectoryForVersion} because it appears to be corrupted"
                );

                Directory.Delete(pathToDirectoryForVersion, true);
            }

            var arguments =
                $"tool install csharpier --version {version} --tool-path \"{pathToDirectoryForVersion}\" ";
            ProcessHelper.ExecuteCommand("dotnet", arguments, null, directory);

            return this.ValidateInstall(pathToDirectoryForVersion, version);
        }

        private bool ValidateInstall(string pathToDirectoryForVersion, string version)
        {
            try
            {
                var env = new Dictionary<string, string> { { "DOTNET_NOLOGO", "1" } };

                var output = ProcessHelper.ExecuteCommand(
                    this.GetPathForVersion(version),
                    "--version",
                    env
                );

                this.logger.Debug("dotnet csharpier --version output: " + output);
                var versionWithoutHash = output.Split('+')[0];
                this.logger.Debug("Using " + versionWithoutHash + " as the version number.");

                if (versionWithoutHash.Equals(version))
                {
                    this.logger.Debug(
                        "CSharpier at " + pathToDirectoryForVersion + " already exists"
                    );
                    return true;
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

            return false;
        }

        public string GetPathForVersion(string version)
        {
            return Path.Combine(this.GetDirectoryForVersion(version), "dotnet-csharpier");
        }

        private string GetDirectoryForVersion(string version)
        {
            if (this.customPath != "")
            {
                return this.customPath;
            }

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CSharpier",
                version
            );
        }
    }
}

package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import com.intellij.util.system.OS;
import java.io.File;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class CustomPathInstaller {

    private final DotNetProvider dotNetProvider;
    Logger logger = CSharpierLogger.getInstance();
    String customPath;

    public CustomPathInstaller(Project project) {
        if (CSharpierSettings.getInstance(project).getUseCustomPath()) {
            this.customPath = CSharpierSettings.getInstance(project).getCustomPath();
        }

        this.dotNetProvider = DotNetProvider.getInstance(project);
    }

    public boolean ensureVersionInstalled(String version, String directory) throws Exception {
        if (version == null || version.equals("")) {
            return true;
        }
        if (this.customPath != "" && this.customPath != null) {
            this.logger.debug("Using csharpier at a custom path of " + this.customPath);
            return true;
        }

        var pathToDirectoryForVersion = getDirectoryForVersion(version);
        var directoryForVersion = new File(pathToDirectoryForVersion);
        if (directoryForVersion.exists()) {
            if (this.validateInstall(pathToDirectoryForVersion, version)) {
                return true;
            }

            this.logger.debug(
                    "Removing directory at " +
                    pathToDirectoryForVersion +
                    " because it appears to be corrupted"
                );
            deleteDirectory(directoryForVersion);
        }

        var command = List.of(
            "tool",
            "install",
            "csharpier",
            "--version",
            version,
            "--tool-path",
            pathToDirectoryForVersion
        );
        this.dotNetProvider.execDotNet(command, new File(directory));

        return this.validateInstall(pathToDirectoryForVersion, version);
    }

    private boolean validateInstall(String pathToDirectoryForVersion, String version) {
        var pathForVersion = "";
        try {
            var env = Map.of("DOTNET_ROOT", this.dotNetProvider.getDotNetRoot());

            pathForVersion = this.getPathForVersion(version);
            var command = List.of(pathForVersion, "--version");
            var output = ProcessHelper.executeCommand(
                command,
                env,
                new File(pathToDirectoryForVersion)
            );

            if (output == null) {
                return false;
            }

            this.logger.debug(pathForVersion + "--version output: " + version);
            var versionWithoutHash = output.trim().split(Pattern.quote("+"))[0];
            this.logger.debug("Using " + versionWithoutHash + " as the version number.");

            if (versionWithoutHash.equals(version)) {
                this.logger.debug("CSharpier at " + pathToDirectoryForVersion + " already exists");
                return true;
            }
        } catch (Exception ex) {
            this.logger.warn(
                    "Exception while running '" +
                    pathForVersion +
                    " --version' in " +
                    pathToDirectoryForVersion,
                    ex
                );
        }

        return false;
    }

    private boolean deleteDirectory(File directoryToBeDeleted) {
        File[] allContents = directoryToBeDeleted.listFiles();
        if (allContents != null) {
            for (File file : allContents) {
                deleteDirectory(file);
            }
        }
        return directoryToBeDeleted.delete();
    }

    private String getDirectoryForVersion(String version) throws Exception {
        if (this.customPath != "" && this.customPath != null) {
            return this.customPath;
        }

        if (OS.CURRENT == OS.Windows) {
            return Path.of(System.getenv("LOCALAPPDATA"), "CSharpier", version).toString();
        }

        var userHome = System.getProperty("user.home");
        if (userHome == null) {
            throw new Exception("There was no user.home property and the OS was not windows");
        }

        var path = Path.of(userHome, ".cache/csharpier", version).toString();

        var arch = this.dotNetProvider.getArchitecture();
        return arch.map(x -> Path.of(path, x).toString()).orElse(path);
    }

    public String getPathForVersion(String version) throws Exception {
        var newCommandsVersion = "1.0.0-alpha1";
        var filename = Semver.gte(version, newCommandsVersion) ? "csharpier" : "dotnet-csharpier";
        var path = Path.of(getDirectoryForVersion(version), filename);
        return path.toString();
    }
}

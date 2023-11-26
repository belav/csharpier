package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import org.apache.commons.lang.SystemUtils;

import java.io.File;
import java.nio.file.Path;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class CustomPathInstaller {
    Logger logger = CSharpierLogger.getInstance();
    String customPath;

    public CustomPathInstaller(CSharpierSettings settings) {
        this.customPath = settings.getCustomPath();
    }

    public boolean ensureVersionInstalled(String version) throws Exception {
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
                    "Removing directory at " + pathToDirectoryForVersion + " because it appears to be corrupted"
            );
            deleteDirectory(directoryForVersion);
        }

        var command = new String[]{"dotnet", "tool", "install", "csharpier", "--version", version, "--tool-path", pathToDirectoryForVersion};
        ProcessHelper.ExecuteCommand(command, null, null);

        return this.validateInstall(pathToDirectoryForVersion, version);
    }

    private boolean validateInstall(String pathToDirectoryForVersion, String version) {
        try {
            Map<String, String> env = new HashMap<>();
            env.put("DOTNET_NOLOGO", "1");

            var command = new String[] { this.getPathForVersion(version), "--version" };
            var output = ProcessHelper.ExecuteCommand(command, env, new File(pathToDirectoryForVersion)).trim();

            this.logger.debug("dotnet csharpier --version output: " + output);

            if (output.split(Pattern.quote("+"))[0].equals(version))
            {
                this.logger.debug("CSharpier at " + pathToDirectoryForVersion + " already exists");
                return true;
            }
        }
        catch (Exception ex) {
            this.logger.warn("Exception while running 'dotnet csharpier --version' in " + pathToDirectoryForVersion, ex);
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

        if (SystemUtils.IS_OS_WINDOWS) {
            return Path.of(System.getenv("LOCALAPPDATA"), "CSharpier", version).toString();
        }

        var userHome = System.getProperty("user.home");
        if (userHome == null) {
            throw new Exception("There was no user.home property and the OS was not windows");
        }

        return Path.of(userHome, ".cache/csharpier", version).toString();
    }

    public String getPathForVersion(String version) throws Exception {
        var path = Path.of(getDirectoryForVersion(version), "dotnet-csharpier");
        return path.toString();
    }
}

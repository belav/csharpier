package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import org.apache.commons.lang.SystemUtils;

import java.io.File;
import java.nio.file.Path;

public class CustomPathInstaller {
    Logger logger = CSharpierLogger.getInstance();

    public void ensureVersionInstalled(String version) throws Exception {
        if (version == null || version.equals("")) {
            return;
        }
        var directoryForVersion = getDirectoryForVersion(version);
        var file = new File(directoryForVersion);
        if (file.exists()) {
            this.logger.debug("File at " + directoryForVersion + " already exists");
            return;
        }

        var command = new String[]{"dotnet", "tool", "install", "csharpier", "--version", version, "--tool-path", directoryForVersion};
        ProcessHelper.ExecuteCommand(command, null, new File(this.getUserHome()));
    }

    private String getDirectoryForVersion(String version) throws Exception {
        return Path.of(this.getUserHome(), ".cache/csharpier", version).toString();
    }

    private String getUserHome() throws Exception {
        // https://docs.microsoft.com/en-us/dotnet/core/install/macos#path-differences
        this.logger.debug("PATH is " + System.getenv("PATH"));
        this.logger.debug("DOTNET_ROOT is " + System.getenv("DOTNET_ROOT"));

        if (SystemUtils.IS_OS_WINDOWS) {
            return System.getenv("LOCALAPPDATA");
        }

        var userHome = System.getProperty("user.home");
        if (userHome == null) {
            throw new Exception("There was no user.home property and the OS was not windows");
        }

        return userHome;
    }

    public String getPathForVersion(String version) throws Exception {
        var path = Path.of(getDirectoryForVersion(version), "dotnet-csharpier");
        return path.toString();
    }
}

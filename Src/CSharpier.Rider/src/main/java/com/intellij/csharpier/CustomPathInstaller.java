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
        ProcessHelper.ExecuteCommand(command, null, null);
    }

    private String getDirectoryForVersion(String version) throws Exception {
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

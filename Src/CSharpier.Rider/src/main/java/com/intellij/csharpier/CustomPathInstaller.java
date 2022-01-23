package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

import java.io.File;
import java.nio.file.Path;

public class CustomPathInstaller {
    Logger logger = CSharpierLogger.getInstance();

    public void ensureVersionInstalled(String version) {
        if (version == null || version.equals("")) {
            return;
        }
        String directoryForVersion = getDirectoryForVersion(version);
        File file = new File(directoryForVersion);
        if (file.exists()) {
            this.logger.debug("File at " + directoryForVersion + " already exists");
            return;
        }

        String[] command = {"dotnet", "tool", "install", "csharpier", "--version", version, "--tool-path", directoryForVersion};
        ProcessHelper.ExecuteCommand(command, null, null);
    }

    private String getDirectoryForVersion(String version) {
        String userHome = System.getProperty("user.home");
        Path path = Path.of(userHome, ".csharpier", version);
        return path.toString();
    }

    public String getPathForVersion(String version) {
        // TODO what about linux? can someone test it?
        Path path = Path.of(getDirectoryForVersion(version), "dotnet-csharpier");
        return path.toString();
    }
}

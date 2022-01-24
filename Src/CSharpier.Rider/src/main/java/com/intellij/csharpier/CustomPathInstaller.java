package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import org.apache.commons.lang.SystemUtils;

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
        Path path = SystemUtils.IS_OS_LINUX
                ? Path.of(System.getProperty("user.home"), ".cache/csharpier", version)
                : Path.of(System.getenv("LOCALAPPDATA"), "CSharpier", version);
        return path.toString();
    }

    public String getPathForVersion(String version) {
        Path path = Path.of(getDirectoryForVersion(version), "dotnet-csharpier");
        return path.toString();
    }
}

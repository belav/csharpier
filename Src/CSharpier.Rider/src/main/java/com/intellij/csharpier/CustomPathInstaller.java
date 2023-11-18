package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import org.apache.commons.lang.SystemUtils;

import java.io.File;
import java.nio.file.Path;
import java.util.HashMap;
import java.util.Map;

public class CustomPathInstaller {
    Logger logger = CSharpierLogger.getInstance();

    public void ensureVersionInstalled(String version) throws Exception {
        if (version == null || version.equals("")) {
            return;
        }
        var pathToDirectoryForVersion = getDirectoryForVersion(version);
        var directoryForVersion = new File(pathToDirectoryForVersion);
        if (directoryForVersion.exists()) {
            try {
                Map<String, String> env = new HashMap<>();
                env.put("DOTNET_NOLOGO", "1");

                var command = new String[] { getPathForVersion(version), "--version" };
                var output = ProcessHelper.ExecuteCommand(command, env, new File(pathToDirectoryForVersion));

                this.logger.debug("dotnet csharpier --version output: " + output);

                if (output.equals(version))
                {
                    this.logger.debug("CSharpier at " + pathToDirectoryForVersion + " already exists");
                    return;
                }
            }
            catch (Exception ex) {
                // TODO somehow I got a bunch of the versions to install that were missing dotnet-csharpier in the root of the custom path
                // this needs to do a better job of figuring that out and reporting it.
                // I think when this fails to install, then the other stuff gets stuck in an infinite loop
                logger.warn("Exception while running 'dotnet csharpier --version' in " + pathToDirectoryForVersion, ex);
            }

            // if we got here something isn't right in the current directory
            deleteDirectory(directoryForVersion);
        }

        var command = new String[]{"dotnet", "tool", "install", "csharpier", "--version", version, "--tool-path", pathToDirectoryForVersion};
        ProcessHelper.ExecuteCommand(command, null, null);
    }

    boolean deleteDirectory(File directoryToBeDeleted) {
        File[] allContents = directoryToBeDeleted.listFiles();
        if (allContents != null) {
            for (File file : allContents) {
                deleteDirectory(file);
            }
        }
        return directoryToBeDeleted.delete();
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

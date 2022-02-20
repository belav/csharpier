package com.intellij.csharpier;

import org.apache.commons.lang.SystemUtils;
import java.io.*;
import java.util.HashMap;
import java.util.Map;

public class ProcessHelper {

    public static String ExecuteCommand(String[] command, Map<String, String> env, File workingDirectory) {
        var logger = CSharpierLogger.getInstance();
        try {
            var directoryToLog = workingDirectory == null ? "" : " in " + workingDirectory;

            logger.debug("user.dir is " + System.getProperty("user.dir"));
            logger.debug("Running " + String.join(" ", command) + directoryToLog);
            var processBuilder = new ProcessBuilder(GetShellCommandIfNeeded(command));

            if (env == null) {
                env = new HashMap<>();
            }

            if (!env.containsKey("PATH")) {
                env.put("PATH", GetPathWithDotNetBinary());
            }

            processBuilder.environment().putAll(env);

            if (workingDirectory != null) {
                processBuilder.directory(workingDirectory);
            }

            var process = processBuilder.start();
            var output = new BufferedReader(new InputStreamReader(process.getInputStream()));
            var error = new BufferedReader(new InputStreamReader(process.getErrorStream()));
            var result = new StringBuilder();
            var errorResult = new StringBuilder();
            String line;
            while ((line = output.readLine()) != null) {
                result.append(line);
                result.append("\n");
            }
            while ((line = error.readLine()) != null) {
                errorResult.append(line);
                errorResult.append("\n");
            }

            var exitVal = process.waitFor();
            if (exitVal == 0){
                return result.toString().trim();
            }
            logger.debug(errorResult.toString());
        } catch (Exception e) {
            logger.error(e.getMessage());
            e.printStackTrace();

        }
        return null;
    }

    private static String GetPathWithDotNetBinary() {
        var path = System.getenv("PATH");

        // For Mac, the .NET binary isn't available for ProcessBuilder, so we'll add the default
        // installation location to the PATH. We'll prefer the ARM version and fallback to the x64.
        if (SystemUtils.IS_OS_MAC) {
            return path + ":/usr/local/share/dotnet:/usr/local/share/dotnet/x64/dotnet";
        }

        // For others, it seems the .NET binary is already available to ProcessBuilder by default.
        // So we'll just return the PATH as is.
        return path;
    }

    // For Mac, we'll have updated the PATH to include the .NET binary. However, setting
    // the PATH doesn't affect the ProcessBuilder's command, but it will apply to
    // spawned processes, so we'll run the desired command in the OS's default shell.
    private static String[] GetShellCommandIfNeeded(String[] command) {
        if (SystemUtils.IS_OS_MAC) {
            return new String[] {"/bin/zsh", "-c", String.join(" ", command)};
        }

        return command;
    }
}

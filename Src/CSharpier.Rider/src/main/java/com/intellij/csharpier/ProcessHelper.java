package com.intellij.csharpier;

import java.io.*;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ProcessHelper {

    public static String executeCommand(
        List<String> command,
        Map<String, String> env,
        File workingDirectory
    ) {
        var logger = CSharpierLogger.getInstance();
        try {
            var directoryToLog = workingDirectory == null ? "" : " in " + workingDirectory;

            logger.debug("Running " + String.join(" ", command) + directoryToLog);
            var processBuilder = new ProcessBuilder(command);

            if (env == null) {
                env = new HashMap<>();
            } else {
                env = new HashMap<>(env);
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
            if (exitVal == 0) {
                return result.toString().trim();
            }
            logger.debug("ErrorOutput: " + errorResult);
        } catch (Exception e) {
            logger.error(e);
        }
        return null;
    }
}

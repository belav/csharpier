package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

import java.io.*;
import java.util.Map;

public class ProcessHelper {

    public static String ExecuteCommand(String[] command, Map<String, String> env, File workingDirectory) {
        var logger = CSharpierLogger.getInstance();
        try {
            logger.debug("Running " + String.join(" ", command) + " in " + workingDirectory);
            var processBuilder = new ProcessBuilder(command);
            if (env != null) {
                processBuilder.environment().putAll(env);
            }
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
}

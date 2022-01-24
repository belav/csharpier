package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

import java.io.*;
import java.util.Map;

public class ProcessHelper {

    public static String ExecuteCommand(String[] command, Map<String, String> env, File workingDirectory) {
        Logger logger = CSharpierLogger.getInstance();
        try {
            logger.debug("Running " + String.join(" ", command) + " in " + workingDirectory);
            ProcessBuilder processBuilder = new ProcessBuilder(command);
            if (env != null) {
                processBuilder.environment().putAll(env);
            }
            if (workingDirectory != null) {
                processBuilder.directory(workingDirectory);
            }

            Process process = processBuilder.start();
            BufferedReader output = new BufferedReader(new InputStreamReader(process.getInputStream()));
            BufferedReader error = new BufferedReader(new InputStreamReader(process.getErrorStream()));
            StringBuilder result = new StringBuilder();
            StringBuilder errorResult = new StringBuilder();
            String line;
            while ((line = output.readLine()) != null) {
                result.append(line);
                result.append("\n");
            }
            while ((line = error.readLine()) != null) {
                errorResult.append(line);
                errorResult.append("\n");
            }

            int exitVal = process.waitFor();
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

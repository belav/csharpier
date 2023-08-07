package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class CSharpierProcessSingleFile implements ICSharpierProcess {
    Logger logger = CSharpierLogger.getInstance();
    String csharpierPath;

    public CSharpierProcessSingleFile(String csharpierPath) {
        this.csharpierPath = csharpierPath;
    }

    @Override
    public String formatFile(String content, String fileName) {
        try {
            this.logger.debug("Running " + this.csharpierPath + " --write-stdout");
            var processBuilder = new ProcessBuilder(this.csharpierPath, "--write-stdout");
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            processBuilder.redirectErrorStream(true);
            var process = processBuilder.start();

            var stdin = process.getOutputStream();
            var stdOut = new BufferedReader(new InputStreamReader(process.getInputStream()));

            stdin.write(content.getBytes());
            stdin.close();

            var output = new StringBuilder();

            var nextCharacter = stdOut.read();
            while (nextCharacter != -1) {
                output.append((char)nextCharacter);
                nextCharacter = stdOut.read();
            }

            var result = output.toString();

            if (process.exitValue() == 0 && !result.contains("Failed to compile so was not formatted.")) {
                return result;
            }
            else {
                this.logger.error(result);
            }
        } catch (Exception e) {
            this.logger.error("error", e);
        }

        return "";
    }

    @Override
    public void dispose() {

    }
}

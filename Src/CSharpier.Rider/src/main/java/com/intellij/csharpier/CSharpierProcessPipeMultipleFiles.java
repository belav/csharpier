package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicBoolean;

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {
    Logger logger = CSharpierLogger.getInstance();

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        try {
            var processBuilder = new ProcessBuilder(csharpierPath, "--pipe-multiple-files");
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            this.process = processBuilder.start();

            var charset = useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

            this.stdin = new OutputStreamWriter(this.process.getOutputStream(), charset);
            this.stdOut = new BufferedReader(new InputStreamReader(this.process.getInputStream(), charset));
        } catch (Exception e) {
            this.logger.error("error", e);
        }

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.formatFile("public class ClassName { }", "Test.cs");
        this.formatFile("public class ClassName { }", "Test.cs");
    }

    @Override
    public String formatFile(String content, String filePath) {
        try {

            this.stdin.write(filePath);
            this.stdin.write('\u0003');
            this.stdin.write(content);
            this.stdin.write('\u0003');
            this.stdin.flush();

            var stringBuilder = new StringBuilder();

            var nextCharacter = this.stdOut.read();
            while (nextCharacter != -1) {
                if (nextCharacter == '\u0003') {
                    break;
                }
                stringBuilder.append((char) nextCharacter);
                nextCharacter = this.stdOut.read();
            }

            var result = stringBuilder.toString();

            if (result == null || result.isEmpty())
            {
                this.logger.info("File is ignored by .csharpierignore or there was an error");
                return "";
            }

            return result;

        } catch (Exception e) {
            this.logger.error(e);
            e.printStackTrace();
            return "";
        }
    }

    @Override
    public void dispose() {
        this.process.destroy();
    }
}

package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.nio.charset.Charset;
import java.util.concurrent.atomic.AtomicBoolean;

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {
    Logger logger = CSharpierLogger.getInstance();
    String csharpierPath;

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;
    BufferedReader stdError;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        this.csharpierPath = csharpierPath;
        try {
            var processBuilder = new ProcessBuilder(csharpierPath, "--pipe-multiple-files");
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            this.process = processBuilder.start();

            var charset = useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

            this.stdin = new OutputStreamWriter(this.process.getOutputStream(), charset);
            this.stdOut = new BufferedReader(new InputStreamReader(this.process.getInputStream(), charset));
            this.stdError = new BufferedReader(new InputStreamReader(this.process.getErrorStream(), charset));
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
        this.logger.info("Formatting file at " + filePath);

        try {
            this.logger.info(filePath);
            this.stdin.write(filePath);
            this.stdin.write('\u0003');
            this.stdin.write(content);
            this.stdin.write('\u0003');
            this.stdin.flush();

            var output = new StringBuilder();
            var errorOutput = new StringBuilder();

            var done = new AtomicBoolean(false);

            // TODO look into ExecutorService.invokeAny to get this cleaned up like the VS version
            // https://docs.oracle.com/javase/8/docs/api/java/util/concurrent/ExecutorService.html#invokeAny-java.util.Collection-
            var outputReaderThread = CreateReadingThread(this.stdOut, output, done);
            outputReaderThread.start();

            var errorReaderThread = CreateReadingThread(this.stdError, errorOutput, done);
            errorReaderThread.start();

            while (!done.get()) {
                Thread.sleep(1);
            }

            errorReaderThread.interrupt();
            outputReaderThread.interrupt();

            var errorResult = errorOutput.toString();
            if (errorResult.length() > 0) {
                this.logger.info("Got error output: " + errorResult);
                return "";
            }

            return output.toString();

        } catch (Exception e) {
            this.logger.error(e);
            e.printStackTrace();
            return "";
        }
    }

    private Thread CreateReadingThread(BufferedReader reader, StringBuilder stringBuilder, AtomicBoolean done) {
        return new Thread(() -> {
            try {
                var nextCharacter = reader.read();
                while (nextCharacter != -1) {
                    if (nextCharacter == '\u0003') {
                        done.set(true);
                        return;
                    }
                    stringBuilder.append((char) nextCharacter);
                    nextCharacter = reader.read();
                }
            } catch (Exception e) {
                this.logger.error(e);
                done.set(true);
            }
        });
    }

    @Override
    public void dispose() {
        this.process.destroy();
    }
}

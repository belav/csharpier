package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.nio.charset.Charset;
import java.util.concurrent.atomic.AtomicBoolean;

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {
    Logger logger = Logger.getInstance(CSharpierProcessPipeMultipleFiles.class);
    String csharpierPath;

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;
    BufferedReader stdError;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        this.csharpierPath = csharpierPath;
        try {
            this.process = new ProcessBuilder("dotnet", csharpierPath, "--pipe-multiple-files")
                    .start();

            String charset = useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

            this.stdin = new OutputStreamWriter(this.process.getOutputStream(), charset);
            this.stdOut = new BufferedReader(new InputStreamReader(this.process.getInputStream(), charset));
            this.stdError = new BufferedReader(new InputStreamReader(this.process.getErrorStream(), charset));
        } catch (Exception e) {
            this.logger.error("error", e);
        }

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

            StringBuilder output = new StringBuilder();
            StringBuilder errorOutput = new StringBuilder();

            AtomicBoolean done = new AtomicBoolean(false);

            // TODO look into ExecutorService.invokeAny to get this cleaned up like the VS version
            // https://docs.oracle.com/javase/8/docs/api/java/util/concurrent/ExecutorService.html#invokeAny-java.util.Collection-
            Thread outputReaderThread = CreateReadingThread(this.stdOut, output, done);
            outputReaderThread.start();

            Thread errorReaderThread = CreateReadingThread(this.stdError, errorOutput, done);
            errorReaderThread.start();

            while (!done.get()) {
                Thread.sleep(1);
            }

            errorReaderThread.interrupt();
            outputReaderThread.interrupt();

            String errorResult = errorOutput.toString();
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

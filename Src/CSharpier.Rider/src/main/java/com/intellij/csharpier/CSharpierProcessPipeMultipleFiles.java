package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.nio.charset.Charset;
import java.util.concurrent.atomic.AtomicBoolean;

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {
    Logger LOG = Logger.getInstance(CSharpierProcessPipeMultipleFiles.class);
    String csharpierPath;

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;
    BufferedReader stdError;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        this.csharpierPath = csharpierPath;
        try {
            process = new ProcessBuilder("dotnet", csharpierPath, "--pipe-multiple-files")
                    .start();

            String charset = useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

            stdin = new OutputStreamWriter(process.getOutputStream(), charset);
            stdOut = new BufferedReader(new InputStreamReader(process.getInputStream(), charset));
            stdError = new BufferedReader(new InputStreamReader(process.getErrorStream(), charset));
        } catch (Exception e) {
            LOG.error("error", e);
        }

        this.formatFile("public class ClassName { }", "Test.cs");
    }

    @Override
    public String formatFile(String content, String filePath) {
        LOG.info("Formatting file at " + filePath);

        try {
            LOG.info(filePath);
            stdin.write(filePath);
            stdin.write('\u0003');
            stdin.write(content);
            stdin.write('\u0003');
            stdin.flush();

            StringBuilder output = new StringBuilder();
            StringBuilder errorOutput = new StringBuilder();

            AtomicBoolean done = new AtomicBoolean(false);

            // TODO look into ExecutorService.invokeAny to get this cleaned up like the VS version
            // https://docs.oracle.com/javase/8/docs/api/java/util/concurrent/ExecutorService.html#invokeAny-java.util.Collection-
            Thread outputReaderThread = CreateReadingThread(stdOut, output, done);
            outputReaderThread.start();

            Thread errorReaderThread = CreateReadingThread(stdError, errorOutput, done);
            errorReaderThread.start();

            while (!done.get()) {
                Thread.sleep(1);
            }

            errorReaderThread.interrupt();
            outputReaderThread.interrupt();

            String errorResult = errorOutput.toString();
            if (errorResult.length() > 0) {
                LOG.info("Got error output: " + errorResult);
                return "";
            }

            return output.toString();

        } catch (Exception e) {
            LOG.error(e);
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
                LOG.error(e);
                done.set(true);
            }
        });
    }

    @Override
    public void dispose() {
        process.destroy();
    }
}

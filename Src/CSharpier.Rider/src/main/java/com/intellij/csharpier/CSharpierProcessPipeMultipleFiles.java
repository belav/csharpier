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
    private final ExecutorService executor;
    Logger logger = CSharpierLogger.getInstance();
    String csharpierPath;

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;
    BufferedReader stdError;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        this.csharpierPath = csharpierPath;
        this.executor = Executors.newCachedThreadPool();
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
        try {
            var output = new StringBuilder();
            var errorOutput = new StringBuilder();

            this.stdin.write(filePath);
            this.stdin.write('\u0003');
            this.stdin.write(content);
            this.stdin.write('\u0003');
            this.stdin.flush();

            var callableTasks = new ArrayList<Callable<Object>>();
            callableTasks.add(Executors.callable(CreateReadingThread(this.stdOut, output)));
            callableTasks.add(Executors.callable(CreateReadingThread(this.stdError, errorOutput)));
            this.executor.invokeAny(callableTasks);

            var result = output.toString();
            var errorResult = errorOutput.toString();
            if (errorResult == null || errorResult.isEmpty())
            {
                if (result == null || result.isEmpty())
                {
                    this.logger.info("File is ignored by .csharpierignore");
                    return "";
                }
                else
                {
                    return output.toString();
                }
            }

            this.logger.info("Got error output: " + errorResult);
            return "";

        } catch (Exception e) {
            this.logger.error(e);
            e.printStackTrace();
            return "";
        }
    }

    private Runnable CreateReadingThread(BufferedReader reader, StringBuilder stringBuilder) {
        var runnable = (Runnable) () -> {
            try {
                var nextCharacter = reader.read();
                while (nextCharacter != -1) {
                    if (nextCharacter == '\u0003') {
                        break;
                    }
                    stringBuilder.append((char) nextCharacter);
                    nextCharacter = reader.read();
                }
            } catch (Exception e) {
                this.logger.error(e);
            }

        };
        return runnable;
    }

    @Override
    public void dispose() {
        this.process.destroy();
        this.executor.shutdown();
        try {
            if (!this.executor.awaitTermination(800, TimeUnit.MILLISECONDS)) {
                this.executor.shutdownNow();
            }
        } catch (InterruptedException e) {
            this.executor.shutdownNow();
        }
    }
}

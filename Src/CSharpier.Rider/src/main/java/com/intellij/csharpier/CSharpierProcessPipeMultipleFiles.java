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
    private final boolean useUtf8;
    private final String csharpierPath;
    Logger logger = CSharpierLogger.getInstance();

    Process process = null;
    OutputStreamWriter stdin;
    BufferedReader stdOut;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath, boolean useUtf8) {
        this.csharpierPath = csharpierPath;
        this.useUtf8 = useUtf8;
        this.startProcess();
    }

    private void startProcess() {
        try {
            var processBuilder = new ProcessBuilder(this.csharpierPath, "--pipe-multiple-files");
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            this.process = processBuilder.start();

            var charset = this.useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

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
        var stringBuilder = new StringBuilder();

        Runnable task = () -> {
            try {
                this.stdin.write(filePath);
                this.stdin.write('\u0003');
                this.stdin.write(content);
                this.stdin.write('\u0003');
                this.stdin.flush();

                var nextCharacter = this.stdOut.read();
                while (nextCharacter != -1) {
                    if (nextCharacter == '\u0003') {
                        break;
                    }
                    stringBuilder.append((char) nextCharacter);
                    nextCharacter = this.stdOut.read();
                }
            } catch (Exception e) {
                this.logger.error(e);
                e.printStackTrace();
            }
        };

        // csharpier will freeze in some instances when "Format on Save" is also installed and the file has compilation errors
        // this detects that and recovers from it
        var thread = new Thread(task);
        thread.start();
        try {
            thread.join(3000);
        } catch (InterruptedException e) {
            // if we interrupt it we shouldn't log it
        }

        if (thread.isAlive()) {
            this.logger.warn("CSharpier process appears to be hung, restarting it.");
            thread.interrupt();
            this.process.destroy();
            this.startProcess();
            return "";
        }

        var result = stringBuilder.toString();

        if (result == null || result.isEmpty()) {
            this.logger.info("File is ignored by .csharpierignore or there was an error");
            return "";
        }

        return result;
    }

    @Override
    public void dispose() {
        if (this.process != null) {
            this.process.destroy();
        }
    }
}

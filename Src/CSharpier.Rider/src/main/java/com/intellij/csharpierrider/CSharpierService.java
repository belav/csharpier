package com.intellij.csharpierrider;

import com.esotericsoftware.minlog.Log;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.util.concurrent.atomic.AtomicBoolean;

// TODO https://plugins.jetbrains.com/docs/intellij/disposers.html#choosing-a-disposable-parent

public class CSharpierService {
    Logger LOG = Logger.getInstance(ReformatWithCSharpierAction.class);
    Process process = null;
    OutputStream stdin = null;
    BufferedReader stdOut = null;
    BufferedReader stdError = null;

    public CSharpierService() {
        try {
            // TODO
            String csharpierPath = "C:\\projects\\csharpier\\Src\\CSharpier.Cli\\bin\\Debug\\net6.0\\dotnet-csharpier.dll";

            process = new ProcessBuilder("dotnet", csharpierPath, "--pipe-multiple-files").start();
        } catch (IOException e) {
            LOG.error("error", e);
            e.printStackTrace();
        }
        stdin = process.getOutputStream();
        stdOut = new BufferedReader(new InputStreamReader(process.getInputStream()));
        stdError = new BufferedReader(new InputStreamReader(process.getErrorStream()));
    }

    @NotNull
    static CSharpierService getInstance(@NotNull Project project) {
        return project.getService(CSharpierService.class);
    }

    public String format(@NotNull String content, @NotNull String filePath) {
        StringBuilder output = new StringBuilder();
        StringBuilder errorOutput = new StringBuilder();

        try {
            LOG.info(filePath);
            // TODO real path
            stdin.write("C:\\projects\\csharpier\\Src\\CSharpier\\DocSerializer.cs".getBytes());
            stdin.write('\u0003');
            stdin.write(content.getBytes());
            stdin.write('\u0003');
            stdin.flush();

            int end = '\u0003';

            AtomicBoolean done = new AtomicBoolean(false);

            Thread outStreamReader = new Thread(() -> {
                try {
                    var nextCharacter = stdOut.read();
                    while (nextCharacter != -1) {
                        LOG.info("Got Output " + nextCharacter);
                        if (nextCharacter == end) {
                            done.set(true);
                            return;
                        }
                        output.append((char)nextCharacter);
                        nextCharacter = stdOut.read();
                    }
                } catch (Exception e) {
                    LOG.error("error", e);
                    e.printStackTrace();
                }
            });
            outStreamReader.start();

            Thread errorStreamReader = new Thread(() -> {
                try {
                    var nextCharacter = stdError.read();
                    while (nextCharacter != -1) {
                        LOG.info("Got Error " + nextCharacter);
                        errorOutput.append((char)nextCharacter);
                        nextCharacter = stdError.read();
                    }
                } catch (Exception e) {
                    LOG.error("error", e);
                    e.printStackTrace();
                }
            });

            errorStreamReader.start();

            int lastLength = errorOutput.length();
            while (!done.get()) {
                Thread.sleep(1);
            }

            errorStreamReader.interrupt();

        } catch (Exception e) {
            LOG.error("error", e);
            e.printStackTrace();
        }

        Log.info("Output:");
        Log.info(output.toString());
        Log.info("Error:");
        Log.info(errorOutput.toString());

        return output.toString();
    }
}

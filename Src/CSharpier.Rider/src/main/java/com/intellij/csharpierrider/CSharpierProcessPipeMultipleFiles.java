package com.intellij.csharpierrider;

import com.esotericsoftware.minlog.Log;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.util.concurrent.atomic.AtomicBoolean;

// TODO only work with c#?
// TODO workflows?

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {
    Logger LOG = Logger.getInstance(CSharpierProcessPipeMultipleFiles.class);
    String csharpierPath;

    Process process = null;
    OutputStream stdin;
    BufferedReader stdOut;
    BufferedReader stdError;

    public CSharpierProcessPipeMultipleFiles(String csharpierPath) {
        this.csharpierPath = csharpierPath;
        try {
            process = new ProcessBuilder("dotnet", csharpierPath, "--pipe-multiple-files")
                    .start();

            stdin = process.getOutputStream();
            stdOut = new BufferedReader(new InputStreamReader(process.getInputStream()));
            stdError = new BufferedReader(new InputStreamReader(process.getErrorStream()));
        } catch (Exception e) {
            LOG.error("error", e);
        }

        this.formatFile("public class ClassName { }", "Test.cs");
    }

    @Override
    public String formatFile(String content, String filePath) {

        try {
            LOG.info(filePath);
            // TODO we need the real file path
            stdin.write("C:\\projects\\csharpier\\Src\\CSharpier\\DocSerializer.cs".getBytes());
            stdin.write('\u0003');
            stdin.write(content.getBytes());
            stdin.write('\u0003');
            stdin.flush();

            StringBuilder output = new StringBuilder();
            StringBuilder errorOutput = new StringBuilder();

            AtomicBoolean done = new AtomicBoolean(false);

            Thread outputReaderThread = CreateReadingThread(stdOut, output, done);
            outputReaderThread.start();

            Thread errorReaderThread = CreateReadingThread(stdError, errorOutput, done);
            errorReaderThread.start();

            while (!done.get()) {
                Thread.sleep(1);
            }

            errorReaderThread.interrupt();
            outputReaderThread.interrupt();

            Log.info("Output:");
            Log.info(output.toString());
            Log.info("Error:");
            Log.info(errorOutput.toString());

            return output.toString();

        } catch (Exception e) {
            LOG.error("error", e);
            e.printStackTrace();
            return "";
        }
    }

    private Thread CreateReadingThread(BufferedReader reader, StringBuilder stringBuilder, AtomicBoolean done) {
        return new Thread(() -> {
            try {
                var nextCharacter = reader.read();
                while (nextCharacter != -1) {
                    LOG.info("Got Error " + nextCharacter);
                    if (nextCharacter == '\u0003') {
                        done.set(true);
                        return;
                    }
                    stringBuilder.append((char) nextCharacter);
                    nextCharacter = reader.read();
                }
            } catch (Exception e) {
                LOG.error("error", e);
                done.set(true);
            }
        });
    }

    @Override
    public void dispose() {
        process.destroy();
    }
}

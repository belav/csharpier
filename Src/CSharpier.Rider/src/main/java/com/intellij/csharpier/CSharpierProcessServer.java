package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

import com.google.gson.Gson;

public class CSharpierProcessServer implements ICSharpierProcess, Disposable {
    private final Gson gson = new Gson();
    private final String csharpierPath;
    private Logger logger = CSharpierLogger.getInstance();
    private int port;
    private Process process = null;
    public boolean processFailedToStart;

    public CSharpierProcessServer(String csharpierPath) {
        this.csharpierPath = csharpierPath;
        this.startProcess();

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.formatFile("public class ClassName { }", "/Temp/Test.cs");
        this.formatFile("public class ClassName { }", "/Temp/Test.cs");
    }

    private void startProcess() {
        try {
            var processBuilder = new ProcessBuilder(this.csharpierPath, "--server");
            processBuilder.redirectErrorStream(true);
            // TODO DOTNET_ROOT
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            this.process = processBuilder.start();

            var reader = new BufferedReader(new InputStreamReader(this.process.getInputStream()));

            var executor = Executors.newSingleThreadExecutor();
            var future = executor.submit(() -> reader.readLine());

            String output = null;
            try {
                output = future.get(2, TimeUnit.SECONDS);
            } catch (TimeoutException e) {
                this.logger.warn("Spawning the csharpier server timed out. Formatting cannot occur.");
                this.process.destroy();
                return;
            }

            if (!this.process.isAlive()) {
                this.logger.warn("Spawning the csharpier server failed because it exited. " + output);
                this.processFailedToStart = true;
                return;
            }

            var portString = output.replace("Started on ", "");
            this.port = Integer.parseInt(portString);

            this.logger.debug("Connecting via port " + portString);

        } catch (Exception e) {
            this.logger.warn("Failed to spawn the needed csharpier server.", e);
            this.processFailedToStart = true;
        }
    }

    @Override
    public String formatFile(String content, String filePath) {
        if (this.processFailedToStart) {
            this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
            return "";
        }

        var data = new FormatFileDto();
        data.fileContents = content;
        data.fileName = filePath;

        var url = "http://localhost:" + this.port + "/format";


        try {
            var url1 = new URL(url);

            var connection = (HttpURLConnection) url1.openConnection();

            connection.setRequestMethod("POST");

            connection.setRequestProperty("Content-Type", "application/json; utf-8");

            connection.setDoOutput(true);
            connection.setDoInput(true);

            var outputStream = connection.getOutputStream();
            var writer = new OutputStreamWriter(outputStream, "UTF-8");
            writer.write(this.gson.toJson(data));
            writer.flush();
            writer.close();
            outputStream.close();

            var responseCode = connection.getResponseCode();
            if (responseCode != 200) {
                connection.disconnect();
                return "";
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream());
            var result = gson.fromJson(reader, FormatFileResult.class);
            reader.close();

            connection.disconnect();

            return result.formattedFile != null ? result.formattedFile : "";

        } catch (Exception e) {
            this.logger.warn("Failed posting to the csharpier server.", e);
        }

        return "";
    }

    @Override
    public void dispose() {
        if (this.process != null) {
            this.process.destroy();
        }
    }

    private class FormatFileDto {
        public String fileContents;
        public String fileName;
    }

    private class FormatFileResult {
        public String formattedFile;
    }
}

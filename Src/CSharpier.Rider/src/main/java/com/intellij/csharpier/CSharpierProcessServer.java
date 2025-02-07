package com.intellij.csharpier;

import com.google.gson.Gson;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import java.io.*;
import java.net.HttpURLConnection;
import java.net.URI;
import java.net.URL;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

public class CSharpierProcessServer implements ICSharpierProcess2, Disposable {

    private final Gson gson = new Gson();
    private final String csharpierPath;
    private final DotNetProvider dotNetProvider;
    private final String version;
    private Logger logger = CSharpierLogger.getInstance();
    private int port;
    private Process process = null;
    private boolean processFailedToStart;

    public CSharpierProcessServer(String csharpierPath, String version, Project project) {
        this.csharpierPath = csharpierPath;
        this.dotNetProvider = DotNetProvider.getInstance(project);
        this.version = version;

        this.startProcess();

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.formatFile("public class ClassName { }", "/Temp/Test.cs");
        this.formatFile("public class ClassName { }", "/Temp/Test.cs");
    }

    private void startProcess() {
        var newCommandsVersion = "1.0.0-alpha1";
        var argument = Semver.gte(this.version, newCommandsVersion) ? "server" : "--server";
        try {
            var processBuilder = new ProcessBuilder(this.csharpierPath, argument);

            // Setting the working directory is important because older versions of csharpier will accidentally
            // install a recursive file system watch on the entire working directory. If this is the repository root
            // it will include extremely large and unnecessary directories like .git, .idea, bin, obj, node_modules.
            processBuilder.directory(new File(this.csharpierPath).getParentFile());
            processBuilder.environment().put("DOTNET_NOLOGO", "1");
            processBuilder.environment().put("DOTNET_ROOT", this.dotNetProvider.getDotNetRoot());
            var csharpierProcess = processBuilder.start();

            var stdoutThread = new Thread(() -> {
                try (
                    var reader = new BufferedReader(
                        new InputStreamReader(csharpierProcess.getInputStream())
                    )
                ) {
                    var line = reader.readLine();
                    if (line == null) {
                        return;
                    }

                    var portString = line.replace("Started on ", "");
                    this.port = Integer.parseInt(portString);

                    this.logger.debug("Connecting via port " + portString);
                    this.process = csharpierProcess;
                } catch (Exception e) {
                    e.printStackTrace();
                }
            });
            stdoutThread.start();
            stdoutThread.join();

            csharpierProcess
                .onExit()
                .thenAccept(p -> {
                    try (
                        var errorReader = new BufferedReader(
                            new InputStreamReader(csharpierProcess.getErrorStream())
                        )
                    ) {
                        var error = new StringBuilder();
                        errorReader.lines().forEach(o -> error.append(o + "\n"));
                        this.logger.error("Process failed to start with " + error);
                    } catch (Exception e) {
                        this.logger.error("Process failed to start with " + e);
                    }

                    this.processFailedToStart = true;
                });
        } catch (Exception e) {
            this.logger.warn("Failed to spawn the needed csharpier server.", e);
            this.processFailedToStart = true;
        }
    }

    @Override
    public FormatFileResult formatFile(FormatFileParameter parameter) {
        if (this.processFailedToStart) {
            this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
            return null;
        }

        var timeWaited = 0;
        while (this.process == null && timeWaited < 15000) {
            try {
                Thread.sleep(100);
            } catch (InterruptedException e) {}

            timeWaited += 100;
        }

        if (this.processFailedToStart || this.process == null) {
            this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
            return null;
        }

        var url = "http://127.0.0.1:" + this.port + "/format";

        try {
            var url1 = URI.create(url).toURL();

            var connection = (HttpURLConnection) url1.openConnection();

            connection.setRequestMethod("POST");

            connection.setRequestProperty("Content-Type", "application/json; utf-8");

            connection.setConnectTimeout(2000);
            connection.setDoOutput(true);
            connection.setDoInput(true);

            var outputStream = connection.getOutputStream();
            var writer = new OutputStreamWriter(outputStream, "UTF-8");
            writer.write(this.gson.toJson(parameter));
            writer.flush();
            writer.close();
            outputStream.close();

            var responseCode = connection.getResponseCode();
            if (responseCode != 200) {
                this.logger.warn(
                        "Csharpier server returned non-200 status code of " + responseCode
                    );
                connection.disconnect();
                return null;
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "UTF-8");
            var result = gson.fromJson(reader, FormatFileResult.class);
            reader.close();

            connection.disconnect();

            return result;
        } catch (Exception e) {
            this.logger.warn("Failed posting to the csharpier server.", e);
        }

        return null;
    }

    @Override
    public String getVersion() {
        return this.version;
    }

    @Override
    public boolean getProcessFailedToStart() {
        return this.processFailedToStart;
    }

    @Override
    public String formatFile(String content, String fileName) {
        var parameter = new FormatFileParameter();
        parameter.fileName = fileName;
        parameter.fileContents = content;

        var result = this.formatFile(parameter);
        return result == null ? null : result.formattedFile;
    }

    @Override
    public void dispose() {
        if (this.process != null) {
            this.process.destroy();
        }
    }
}

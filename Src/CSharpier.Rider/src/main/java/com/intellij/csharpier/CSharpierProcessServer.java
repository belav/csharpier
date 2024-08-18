package com.intellij.csharpier;

import com.google.gson.Gson;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import java.io.*;
import java.net.HttpURLConnection;
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

    if (!this.startProcess()) {
      this.processFailedToStart = true;
      return;
    }

    this.logger.debug("Warm CSharpier with initial format");
    // warm by formatting a file twice, the 3rd time is when it gets really fast
    this.formatFile("public class ClassName { }", "/Temp/Test.cs");
    this.formatFile("public class ClassName { }", "/Temp/Test.cs");
  }

  private boolean startProcess() {
    try {
      var processBuilder = new ProcessBuilder(this.csharpierPath, "--server");
      processBuilder.redirectErrorStream(true);
      processBuilder.environment().put("DOTNET_NOLOGO", "1");
      processBuilder.environment().put("DOTNET_ROOT", this.dotNetProvider.getDotNetRoot());
      this.process = processBuilder.start();

      var reader = new BufferedReader(new InputStreamReader(this.process.getInputStream()));

      var executor = Executors.newSingleThreadExecutor();
      var future = executor.submit(() -> reader.readLine());

      String output;
      try {
        output = future.get(2, TimeUnit.SECONDS);
      } catch (TimeoutException e) {
        this.logger.warn("Spawning the csharpier server timed out. Formatting cannot occur.");
        this.process.destroy();
        return false;
      }

      if (!this.process.isAlive()) {
        this.logger.warn("Spawning the csharpier server failed because it exited. " + output);
        return false;
      }

      var portString = output.replace("Started on ", "");
      this.port = Integer.parseInt(portString);

      this.logger.debug("Connecting via port " + portString);
      return true;
    } catch (Exception e) {
      this.logger.warn("Failed to spawn the needed csharpier server.", e);
      return false;
    }
  }

  @Override
  public FormatFileResult formatFile(FormatFileParameter parameter) {
    if (this.processFailedToStart) {
      this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
      return null;
    }

    var url = "http://127.0.0.1:" + this.port + "/format";

    try {
      var url1 = new URL(url);

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
        this.logger.warn("Csharpier server returned non-200 status code of " + responseCode);
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

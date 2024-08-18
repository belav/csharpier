package com.intellij.csharpier;

import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import java.io.*;
import java.nio.charset.Charset;

public class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess, Disposable {

  private final boolean useUtf8;
  private final String csharpierPath;
  private final DotNetProvider dotNetProvider;
  private final String version;
  private Logger logger = CSharpierLogger.getInstance();

  private Process process = null;
  private OutputStreamWriter stdin;
  private BufferedReader stdOut;
  private boolean processFailedToStart;

  public CSharpierProcessPipeMultipleFiles(
    String csharpierPath,
    boolean useUtf8,
    String version,
    Project project
  ) {
    this.csharpierPath = csharpierPath;
    this.useUtf8 = useUtf8;
    this.dotNetProvider = DotNetProvider.getInstance(project);
    this.version = version;
    this.startProcess();

    this.logger.debug("Warm CSharpier with initial format");
    // warm by formatting a file twice, the 3rd time is when it gets really fast
    this.formatFile("public class ClassName { }", "Test.cs");
    this.formatFile("public class ClassName { }", "Test.cs");
  }

  private void startProcess() {
    try {
      var processBuilder = new ProcessBuilder(this.csharpierPath, "--pipe-multiple-files");
      processBuilder.environment().put("DOTNET_NOLOGO", "1");
      processBuilder.environment().put("DOTNET_ROOT", this.dotNetProvider.getDotNetRoot());
      this.process = processBuilder.start();

      var charset = this.useUtf8 ? "utf-8" : Charset.defaultCharset().toString();

      this.stdin = new OutputStreamWriter(this.process.getOutputStream(), charset);
      this.stdOut = new BufferedReader(
        new InputStreamReader(this.process.getInputStream(), charset)
      );

      // if we don't read the error stream, eventually too much is buffered on it and the plugin hangs
      var errorGobbler = new StreamGobbler(this.process.getErrorStream());
      errorGobbler.start();
    } catch (Exception e) {
      this.logger.warn("Failed to spawn the needed csharpier process. Formatting cannot occur.", e);
      this.processFailedToStart = true;
    }
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
  public String formatFile(String content, String filePath) {
    if (this.processFailedToStart) {
      this.logger.warn("CSharpier proccess failed to start. Formatting cannot occur.");
      return "";
    }

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

  private class StreamGobbler extends Thread {

    InputStream inputStream;

    private StreamGobbler(InputStream inputStream) {
      this.inputStream = inputStream;
    }

    @Override
    public void run() {
      try {
        var streamReader = new InputStreamReader(this.inputStream);
        var reader = new BufferedReader(streamReader);
        while (reader.readLine() != null) {}
      } catch (IOException ioe) {}
    }
  }
}

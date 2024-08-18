package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import java.io.BufferedReader;
import java.io.InputStreamReader;

public class CSharpierProcessSingleFile implements ICSharpierProcess {

  private final DotNetProvider dotNetProvider;
  private final Logger logger = CSharpierLogger.getInstance();
  private final String csharpierPath;
  private final String version;

  public CSharpierProcessSingleFile(
    String csharpierPath,
    String version,
    Project project
  ) {
    this.csharpierPath = csharpierPath;
    this.dotNetProvider = DotNetProvider.getInstance(project);
    this.version = version;
  }

  @Override
  public String getVersion() {
    return this.version;
  }

  @Override
  public boolean getProcessFailedToStart() {
    return false;
  }

  @Override
  public String formatFile(String content, String fileName) {
    try {
      this.logger.debug("Running " + this.csharpierPath + " --write-stdout");
      var processBuilder = new ProcessBuilder(
        this.csharpierPath,
        "--write-stdout"
      );
      processBuilder.environment().put("DOTNET_NOLOGO", "1");
      processBuilder
        .environment()
        .put("DOTNET_ROOT", this.dotNetProvider.getDotNetRoot());
      processBuilder.redirectErrorStream(true);
      var process = processBuilder.start();

      var stdin = process.getOutputStream();
      var stdOut = new BufferedReader(
        new InputStreamReader(process.getInputStream())
      );

      stdin.write(content.getBytes());
      stdin.close();

      var output = new StringBuilder();

      var nextCharacter = stdOut.read();
      while (nextCharacter != -1) {
        output.append((char) nextCharacter);
        nextCharacter = stdOut.read();
      }

      var result = output.toString();

      if (
        process.exitValue() == 0 &&
        !result.contains("Failed to compile so was not formatted.")
      ) {
        return result;
      } else {
        this.logger.error(result);
      }
    } catch (Exception e) {
      this.logger.error("error", e);
    }

    return "";
  }

  @Override
  public void dispose() {}
}

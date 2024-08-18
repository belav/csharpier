package com.intellij.csharpier;

import com.intellij.notification.Notification;
import com.intellij.notification.NotificationAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.util.NlsContexts;
import java.io.File;
import java.nio.file.Path;
import java.util.List;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class InstallLocalAction extends NotificationAction {

  private final DotNetProvider dotNetProvider;
  private Logger logger = CSharpierLogger.getInstance();
  private final IProcessKiller processKiller;
  private final String projectPath;

  public InstallLocalAction(
    @Nullable @NlsContexts.NotificationContent String text,
    IProcessKiller processKiller,
    Project project
  ) {
    super(text);
    this.projectPath = project.getBasePath();
    this.processKiller = processKiller;
    this.dotNetProvider = DotNetProvider.getInstance(project);
  }

  @Override
  public void actionPerformed(@NotNull AnActionEvent e, @NotNull Notification notification) {
    var manifestPath = Path.of(this.projectPath, ".config/dotnet-tools.json").toString();
    this.logger.info("Installing csharpier in " + manifestPath);
    if (!new File(manifestPath).exists()) {
      var command = List.of("new", "tool-manifest");
      this.dotNetProvider.execDotNet(command, new File(this.projectPath));
    }

    var command2 = List.of("tool", "install", "csharpier");
    this.dotNetProvider.execDotNet(command2, new File(this.projectPath));
    this.processKiller.killRunningProcesses();

    notification.expire();
  }
}

package com.intellij.csharpier;

import com.intellij.notification.Notification;
import com.intellij.notification.NotificationAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.util.NlsContexts;
import java.util.List;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class InstallGlobalAction extends NotificationAction {

  private final IProcessKiller processKiller;
  private final DotNetProvider dotNetProvider;

  public InstallGlobalAction(
    @Nullable @NlsContexts.NotificationContent String text,
    IProcessKiller processKiller,
    Project project
  ) {
    super(text);
    this.processKiller = processKiller;
    this.dotNetProvider = DotNetProvider.getInstance(project);
  }

  @Override
  public void actionPerformed(@NotNull AnActionEvent e, @NotNull Notification notification) {
    var command = List.of("tool", "install", "-g", "csharpier");
    this.dotNetProvider.execDotNet(command, null);
    this.processKiller.killRunningProcesses();

    notification.expire();
  }
}

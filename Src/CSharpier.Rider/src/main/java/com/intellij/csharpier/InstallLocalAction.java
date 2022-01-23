package com.intellij.csharpier;

import com.intellij.notification.Notification;
import com.intellij.notification.NotificationAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.util.NlsContexts;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.io.File;
import java.nio.file.Path;

public class InstallLocalAction extends NotificationAction {
    private Logger logger = CSharpierLogger.getInstance();
    private final IProcessKiller processKiller;
    private final String projectPath;

    public InstallLocalAction(
        @Nullable @NlsContexts.NotificationContent String text,
        String projectPath,
        IProcessKiller processKiller
    ) {
        super(text);
        this.projectPath = projectPath;
        this.processKiller = processKiller;
    }

    @Override
    public void actionPerformed(@NotNull AnActionEvent e, @NotNull Notification notification) {
        String manifestPath = Path.of(this.projectPath, ".config/dotnet-tools.json").toString();
        this.logger.info("Installing csharpier in " + manifestPath);
        if (!new File(manifestPath).exists())
        {
            String[] command = { "dotnet", "new", "tool-manifest" };
            ProcessHelper.ExecuteCommand(command, null, new File(this.projectPath));
        }

        String[] command2 = { "dotnet", "tool", "install", "csharpier" };
        ProcessHelper.ExecuteCommand(command2, null, new File(this.projectPath));

        this.processKiller.killRunningProcesses();
    }
}

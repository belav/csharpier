package com.intellij.csharpier;

import com.intellij.notification.Notification;
import com.intellij.notification.NotificationAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.util.NlsContexts;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class InstallGlobalAction extends NotificationAction {

    private final IProcessKiller processKiller;

    public InstallGlobalAction(
        @Nullable @NlsContexts.NotificationContent String text,
        IProcessKiller processKiller
    ) {
        super(text);
        this.processKiller = processKiller;
    }

    @Override
    public void actionPerformed(@NotNull AnActionEvent e, @NotNull Notification notification) {
        var command2 = new String[]{"dotnet", "tool", "install", "-g", "csharpier"};
        ProcessHelper.ExecuteCommand(command2, null, null);
        this.processKiller.killRunningProcesses();

        notification.expire();
    }
}


package com.intellij.csharpier;

import com.intellij.notification.Notification;
import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import org.apache.commons.lang3.StringUtils;
import org.jetbrains.annotations.NotNull;

public class InstallerService {
    private final Logger logger = CSharpierLogger.getInstance();
    private final Project project;
    private boolean warnedAlready;

    public InstallerService(@NotNull Project project) {
        this.project = project;
    }

    @NotNull
    static InstallerService getInstance(@NotNull Project project) {
        return project.getService(InstallerService.class);
    }

    public void displayInstallNeededMessage(String directoryThatContainsFile, IProcessKiller processKiller) {
        if (this.warnedAlready || ignoreDirectory(directoryThatContainsFile)) {
            return;
        }
        this.warnedAlready = true;
        this.logger.warn("CSharpier was not found so files may not be formatted.");

        var isOnlyGlobal = !directoryThatContainsFile.replace('\\', '/').startsWith(this.project.getBasePath());

        var message = isOnlyGlobal
                ? ("CSharpier needs to be installed globally to format files in " + directoryThatContainsFile)
                : "CSharpier needs to be installed to support formatting files";

        var notification = NotificationGroupManager.getInstance().getNotificationGroup("CSharpier")
                .createNotification(message, NotificationType.WARNING);

        notification.addAction(new InstallGlobalAction("Install CSharpier Globally", processKiller, this.project));
        if (!isOnlyGlobal) {
            notification.addAction(
                new InstallLocalAction("Install CSharpier Locally", processKiller, project)
            );
        }

        notification.notify(this.project);
    }

    private boolean ignoreDirectory(String directoryThatContainsFile) {
        var normalizedPath = directoryThatContainsFile.replace('\\', '/');
        return StringUtils.containsIgnoreCase(normalizedPath, "resharper-host/DecompilerCache")
            || StringUtils.containsIgnoreCase(normalizedPath, "resharper-host/SourcesCache")
            || directoryThatContainsFile.equals("/");
    }
}

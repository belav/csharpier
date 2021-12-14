package com.intellij.csharpier;

import com.esotericsoftware.minlog.Log;
import com.intellij.notification.Notification;
import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import org.apache.maven.artifact.versioning.ComparableVersion;
import org.jetbrains.annotations.NotNull;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.Scanner;

public class CSharpierService {
    Logger LOG = Logger.getInstance(ReformatWithCSharpierAction.class);
    String csharpierPath;
    ICSharpierProcess csharpierProcess;
    Project project;

    public CSharpierService(@NotNull Project project) {
        this.project = project;
        csharpierPath = getCSharpierPath();

        Log.info("Using command dotnet " + csharpierPath);

        csharpierProcess = setupCSharpierProcess();
    }

    public String getCSharpierPath() {
        try {
            String csharpierDebugPath = "C:\\projects\\csharpier\\Src\\CSharpier.Cli\\bin\\Debug\\net6.0\\dotnet-csharpier.dll";
            String csharpierReleasePath = csharpierDebugPath.replace("Debug", "Release");

            if (new File(csharpierDebugPath).exists()) {
                return csharpierDebugPath;
            } else if (new File(csharpierReleasePath).exists()) {
                return csharpierReleasePath;
            }
        } catch (Exception ex) {
            Log.debug("Could not find local csharpier " + ex.getMessage());
        }

        return "csharpier";
    }

    @NotNull
    static CSharpierService getInstance(@NotNull Project project) {
        return project.getService(CSharpierService.class);
    }

    public String execCmd(String cmd) {
        String result = null;
        try (InputStream inputStream = Runtime.getRuntime().exec(cmd).getInputStream();
             Scanner s = new Scanner(inputStream).useDelimiter("\\A")
        ) {
            result = s.hasNext() ? s.next() : null;
        } catch (IOException e) {
            Log.error(e.getMessage());
            e.printStackTrace();
        }
        return result;
    }

    private ICSharpierProcess setupCSharpierProcess() {
        try {
            String version = execCmd("dotnet " + this.csharpierPath + " --version");
            Log.info("CSharpier version: " + version);
            if (version == null) {
                this.displayInstallNeededMessage();
            }
            else {
                ComparableVersion installedVersion = new ComparableVersion(version);
                ComparableVersion pipeFilesVersion = new ComparableVersion("0.12.0");
                if (installedVersion.compareTo(pipeFilesVersion) < 0) {
                    String content = "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                    NotificationGroupManager.getInstance().getNotificationGroup("CSharpier")
                            .createNotification(content, NotificationType.INFORMATION)
                            .notify(project);

                    return new CSharpierProcessSingleFile(this.csharpierPath);
                }
                return new CSharpierProcessPipeMultipleFiles(this.csharpierPath);
            }
        } catch (Exception ex) {
            LOG.error(ex);
        }

        return new NullCSharpierProcess();
    }

    private void displayInstallNeededMessage() {
        Notification notification = NotificationGroupManager.getInstance().getNotificationGroup("CSharpier")
                .createNotification("CSharpier must be installed globally to support formatting.", NotificationType.WARNING);

        // TODO why can't an action be displayed in this???
//        notification.addAction(new EditAction());

        notification.notify(project);
    }

    public String format(@NotNull String content, @NotNull String filePath) {
        return this.csharpierProcess.formatFile(content, filePath);
    }
}
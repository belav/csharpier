package com.intellij.csharpier;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.editor.EditorFactory;
import com.intellij.openapi.editor.event.DocumentEvent;
import com.intellij.openapi.editor.event.DocumentListener;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileEditor;
import com.intellij.openapi.fileEditor.FileEditorManager;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.VirtualFile;
import org.apache.maven.artifact.versioning.ComparableVersion;
import org.jetbrains.annotations.NotNull;

import java.io.File;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.HashMap;
import java.util.Map;

public class CSharpierProcessProvider implements DocumentListener, Disposable, IProcessKiller {
    private final CustomPathInstaller customPathInstaller = new CustomPathInstaller();
    private final Logger logger = CSharpierLogger.getInstance();
    private final Project project;

    private boolean warnedForOldVersion;
    private final HashMap<String, Boolean> warmingByDirectory = new HashMap<>();
    private final HashMap<String, String> csharpierVersionByDirectory = new HashMap<>();
    private final HashMap<String, ICSharpierProcess> csharpierProcessesByVersion = new HashMap<>();

    public CSharpierProcessProvider(@NotNull Project project) {
        this.project = project;

        for (var fileEditor : FileEditorManager.getInstance(project).getAllEditors()) {
            var path = fileEditor.getFile().getPath();
            if (path.toLowerCase().endsWith(".cs")) {
                this.findAndWarmProcess(path);
            }
        }

        EditorFactory.getInstance().getEventMulticaster().addDocumentListener(this, this);
    }

    @NotNull
    static CSharpierProcessProvider getInstance(@NotNull Project project) {
        return project.getService(CSharpierProcessProvider.class);
    }

    @Override
    public void documentChanged(@NotNull DocumentEvent event) {
        var document = event.getDocument();
        var file = FileDocumentManager.getInstance().getFile(document);

        if (file == null
                || file.getExtension() == null
                || !file.getExtension().equalsIgnoreCase("cs")
        ) {
            return;
        }
        var filePath = file.getPath();
        this.findAndWarmProcess(filePath);
    }

    private void findAndWarmProcess(String filePath) {
        var directory = Path.of(filePath).getParent().toString();
        if (this.warmingByDirectory.getOrDefault(directory, false)) {
            return;
        }
        this.logger.debug("Ensure there is a csharpier process for " + directory);
        this.warmingByDirectory.put(directory, true);
        var version = this.csharpierVersionByDirectory.getOrDefault(directory, null);
        if (version == null) {
            version = this.getCSharpierVersion(directory);
            if (version == null || version.isEmpty()) {
                InstallerService.getInstance(this.project).displayInstallNeededMessage(directory, this);
            }
            this.csharpierVersionByDirectory.put(directory, version);
        }

        if (!this.csharpierProcessesByVersion.containsKey(version)) {
            this.csharpierProcessesByVersion.put(version, this.setupCSharpierProcess(
                    directory,
                    version
            ));
        }

        this.warmingByDirectory.remove(directory);
    }

    public ICSharpierProcess getProcessFor(String filePath) {
        this.logger.debug(filePath);
        var directory = Path.of(filePath).getParent().toString();
        var version = this.csharpierVersionByDirectory.getOrDefault(directory, null);
        if (version == null) {
            this.findAndWarmProcess(filePath);
            version = this.csharpierVersionByDirectory.get(directory);
        }

        if (version == null || !this.csharpierProcessesByVersion.containsKey(version)) {
            // this shouldn't really happen, but just in case
            return new NullCSharpierProcess();
        }

        return this.csharpierProcessesByVersion.get(version);
    }

    private String getCSharpierVersion(String directoryThatContainsFile) {
        var currentDirectory = Path.of(directoryThatContainsFile);
        try {
            while (true) {
                var configPath = Path.of(currentDirectory.toString(), ".config/dotnet-tools.json");
                var dotnetToolsPath = configPath.toString();
                var file = new File(dotnetToolsPath);
                this.logger.debug("Looking for " + dotnetToolsPath);
                if (file.exists()) {
                    var data = new String(Files.readAllBytes(configPath));
                    var configData = new Gson().fromJson(data, JsonObject.class);
                    var tools = configData.getAsJsonObject("tools");
                    if (tools != null) {
                        var csharpier = tools.getAsJsonObject("csharpier");
                        if (csharpier != null) {
                            var version = csharpier.get("version").getAsString();
                            if (version != null) {
                                this.logger.debug("Found version " + version + " in " + dotnetToolsPath);
                                return version;
                            }
                        }
                    }
                }

                if (currentDirectory.getParent() == null) {
                    break;
                }
                currentDirectory = currentDirectory.getParent();
            }
        } catch (Exception ex) {
            this.logger.error(ex);
        }

        this.logger.debug(
                "Unable to find dotnet-tools.json, falling back to running dotnet csharpier --version"
        );

        Map<String, String> env = new HashMap<>();
        env.put("DOTNET_NOLOGO", "1");

        var command = new String[]{"dotnet", "csharpier", "--version"};
        var version = ProcessHelper.ExecuteCommand(command, env, new File(directoryThatContainsFile));

        this.logger.debug("dotnet csharpier --version output: " + version);

        return version == null ? "" : version;
    }

    private ICSharpierProcess setupCSharpierProcess(String directory, String version) {
        if (version == null || version.equals("")) {
            return new NullCSharpierProcess();
        }

        this.customPathInstaller.ensureVersionInstalled(version);
        var customPath = this.customPathInstaller.getPathForVersion(version);
        try {
            this.logger.debug("Adding new version " + version + " process for " + directory);

            var installedVersion = new ComparableVersion(version);
            var pipeFilesVersion = new ComparableVersion("0.12.0");
            var utf8Version = new ComparableVersion("0.14.0");

            if (installedVersion.compareTo(pipeFilesVersion) < 0) {
                if (!this.warnedForOldVersion) {
                    var content = "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                    NotificationGroupManager.getInstance().getNotificationGroup("CSharpier")
                            .createNotification(content, NotificationType.INFORMATION)
                            .notify(this.project);

                    this.warnedForOldVersion = true;
                }


                return new CSharpierProcessSingleFile(customPath);
            }

            var useUtf8 = installedVersion.compareTo(utf8Version) >= 0;

            return new CSharpierProcessPipeMultipleFiles(customPath, useUtf8);

        } catch (Exception ex) {
            this.logger.error(ex);
        }

        return new NullCSharpierProcess();
    }

    @Override
    public void dispose() {
        this.killRunningProcesses();
    }

    public void killRunningProcesses() {
        for (var key : this.csharpierProcessesByVersion.keySet()) {
            this.logger.debug(
                    "disposing of process for version " + (key == "" ? "null" : key)
            );
            this.csharpierProcessesByVersion.get(key).dispose();
        }
        this.warmingByDirectory.clear();
        this.csharpierVersionByDirectory.clear();
        this.csharpierProcessesByVersion.clear();
    }
}

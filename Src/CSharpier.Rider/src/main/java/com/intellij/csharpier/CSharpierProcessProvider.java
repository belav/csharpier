package com.intellij.csharpier;

import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.EditorFactory;
import com.intellij.openapi.editor.event.DocumentEvent;
import com.intellij.openapi.editor.event.DocumentListener;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileEditorManager;
import com.intellij.openapi.fileEditor.FileEditorManagerEvent;
import com.intellij.openapi.fileEditor.FileEditorManagerListener;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.openapi.vfs.VirtualFileManager;
import com.intellij.openapi.vfs.newvfs.BulkFileListener;
import com.intellij.openapi.vfs.newvfs.events.VFileEvent;
import java.io.File;
import java.nio.file.Path;
import java.time.Instant;
import java.util.Arrays;
import java.util.List;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathFactory;
import org.jetbrains.annotations.NotNull;
import org.w3c.dom.Node;

public class CSharpierProcessProvider implements DocumentListener, Disposable, IProcessKiller {

    private static final Set<String> supportedExtensions = Set.of(
        "cs",
        "csx",
        "config",
        "csproj",
        "props",
        "slnx",
        "targets",
        "axaml",
        "xaml",
        "xml"
    );

    private final CustomPathInstaller customPathInstaller;
    private final Logger logger = CSharpierLogger.getInstance();
    private final Project project;

    private boolean warnedForOldVersion;
    private final ConcurrentHashMap<String, Long> lastWarmedByDirectory = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, String> csharpierVersionByDirectory =
        new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, ICSharpierProcess> csharpierProcessesByVersion =
        new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, String> toolVersionByManifest =
        new ConcurrentHashMap<>();
    private final Set<String> versionsBeingSetUp = ConcurrentHashMap.newKeySet();

    public CSharpierProcessProvider(@NotNull Project project) {
        this.project = project;
        this.customPathInstaller = new CustomPathInstaller(project);

        EditorFactory.getInstance().getEventMulticaster().addDocumentListener(this, this);
        project
            .getMessageBus()
            .connect(this)
            .subscribe(
                FileEditorManagerListener.FILE_EDITOR_MANAGER,
                new FileEditorManagerListener() {
                    @Override
                    public void fileOpened(
                        @NotNull FileEditorManager source,
                        @NotNull VirtualFile file
                    ) {
                        CSharpierProcessProvider.this.warmProcessFor(file);
                    }

                    @Override
                    public void selectionChanged(@NotNull FileEditorManagerEvent event) {
                        CSharpierProcessProvider.this.warmProcessFor(event.getNewFile());
                    }
                }
            );

        ApplicationManager.getApplication()
            .getMessageBus()
            .connect(this)
            .subscribe(
                VirtualFileManager.VFS_CHANGES,
                new BulkFileListener() {
                    @Override
                    public void after(@NotNull List<? extends @NotNull VFileEvent> events) {
                        if (
                            events
                                .stream()
                                .anyMatch(o -> o.getPath().endsWith("/dotnet-tools.json"))
                        ) {
                            ApplicationManager.getApplication()
                                .executeOnPooledThread(
                                    CSharpierProcessProvider.this::toolManifestChanged
                                );
                        }
                    }
                }
            );
    }

    private void toolManifestChanged() {
        this.logger.debug("A dotnet-tools.json changed, clearing cached csharpier versions");
        synchronized (this) {
            this.lastWarmedByDirectory.clear();
            this.csharpierVersionByDirectory.clear();
            this.toolVersionByManifest.clear();
        }
        this.warmOpenFiles();
    }

    @NotNull
    static CSharpierProcessProvider getInstance(@NotNull Project project) {
        return project.getService(CSharpierProcessProvider.class);
    }

    @Override
    public void documentChanged(@NotNull DocumentEvent event) {
        var document = event.getDocument();
        this.warmProcessFor(FileDocumentManager.getInstance().getFile(document));
    }

    public void warmOpenFiles() {
        for (var file : FileEditorManager.getInstance(this.project).getOpenFiles()) {
            this.warmProcessFor(file);
        }
    }

    private void warmProcessFor(VirtualFile file) {
        if (
            file == null ||
            !file.isInLocalFileSystem() ||
            file.getExtension() == null ||
            !supportedExtensions.contains(file.getExtension().toLowerCase())
        ) {
            return;
        }

        this.warmProcessInBackground(file.getPath());
    }

    private void warmProcessInBackground(String filePath) {
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return;
        }
        var directory = Path.of(filePath).getParent().toString();
        if (this.hasProcessFor(directory) || this.wasWarmedRecently(directory)) {
            return;
        }

        ApplicationManager.getApplication()
            .executeOnPooledThread(() -> this.findAndWarmProcess(filePath));
    }

    private boolean hasProcessFor(String directory) {
        var version = this.csharpierVersionByDirectory.get(directory);
        return version != null && this.csharpierProcessesByVersion.containsKey(version);
    }

    private boolean wasWarmedRecently(String directory) {
        var lastWarmed = this.lastWarmedByDirectory.getOrDefault(directory, Long.valueOf(0));
        return lastWarmed + 5000 > Instant.now().toEpochMilli();
    }

    private synchronized void findAndWarmProcess(String filePath) {
        // if we didn't find dotnet bail out so we don't get extra errors about being unable to format
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return;
        }
        var directory = Path.of(filePath).getParent().toString();
        if (this.wasWarmedRecently(directory)) {
            return;
        }
        this.logger.debug("Ensure there is a csharpier process for " + filePath);
        this.lastWarmedByDirectory.put(directory, Instant.now().toEpochMilli());
        var version = this.csharpierVersionByDirectory.get(directory);
        if (version == null) {
            version = this.getCSharpierVersion(directory);
            if (version == null || version.isEmpty()) {
                InstallerService.getInstance(this.project).displayInstallNeededMessage(
                    directory,
                    this
                );
            }
            this.csharpierVersionByDirectory.put(directory, version);
        }

        if (
            !this.csharpierProcessesByVersion.containsKey(version) &&
            this.versionsBeingSetUp.add(version)
        ) {
            var finalVersion = version;
            Runnable task = () -> {
                try {
                    this.csharpierProcessesByVersion.put(
                            finalVersion,
                            this.setupCSharpierProcess(directory, finalVersion)
                        );
                } finally {
                    this.versionsBeingSetUp.remove(finalVersion);
                }
            };

            var thread = new Thread(task);
            thread.start();
        }
    }

    public boolean canFormatWithoutBlocking(String filePath) {
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return false;
        }

        var directory = Path.of(filePath).getParent().toString();
        var version = this.csharpierVersionByDirectory.get(directory);
        var process = version == null ? null : this.csharpierProcessesByVersion.get(version);
        if (process == null) {
            this.warmProcessInBackground(filePath);
            return false;
        }

        return !NullCSharpierProcess.Instance.equals(process);
    }

    public ICSharpierProcess getProcessFor(String filePath) {
        // if we didn't find dotnet bail out so we don't get extra errors about being unable to format
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return NullCSharpierProcess.Instance;
        }

        var directory = Path.of(filePath).getParent().toString();
        var version = this.csharpierVersionByDirectory.get(directory);
        if (version == null) {
            this.findAndWarmProcess(filePath);
            version = this.csharpierVersionByDirectory.get(directory);
        }

        if (version == null || !this.csharpierProcessesByVersion.containsKey(version)) {
            // this shouldn't really happen, but just in case
            return NullCSharpierProcess.Instance;
        }

        return this.csharpierProcessesByVersion.get(version);
    }

    private String getCSharpierVersion(String directoryThatContainsFile) {
        var csharpierVersion = FunctionRunner.runUntilNonNull(
            () -> this.findVersionInCsProjOfParentsDirectories(directoryThatContainsFile),
            () -> this.findCSharpierVersionInToolOutput(directoryThatContainsFile, false),
            () -> this.findCSharpierVersionInToolOutput(directoryThatContainsFile, true)
        );

        if (csharpierVersion == null) {
            return "";
        }

        var versionWithoutHash = csharpierVersion.split(Pattern.quote("+"))[0];
        this.logger.debug("Using " + versionWithoutHash + " as the version number.");
        return versionWithoutHash;
    }

    private String findCSharpierVersionInToolOutput(
        String directoryThatContainsFile,
        boolean isGlobal
    ) {
        var cacheKey = isGlobal ? "global" : this.findToolManifest(directoryThatContainsFile);
        if (cacheKey == null) {
            this.logger.debug(
                    "No dotnet-tools.json found in or above " + directoryThatContainsFile
                );
            return null;
        }

        var cachedVersion = this.toolVersionByManifest.get(cacheKey);
        if (cachedVersion != null) {
            this.logger.debug("Using cached tool version '" + cachedVersion + "' for " + cacheKey);
            return cachedVersion.isEmpty() ? null : cachedVersion;
        }

        var version = this.runToolList(directoryThatContainsFile, isGlobal);
        this.toolVersionByManifest.put(cacheKey, version == null ? "" : version);
        return version;
    }

    private String findToolManifest(String directoryThatContainsFile) {
        var currentDirectory = Path.of(directoryThatContainsFile);
        while (currentDirectory != null) {
            for (var relativePath : List.of(".config/dotnet-tools.json", "dotnet-tools.json")) {
                var manifest = currentDirectory.resolve(relativePath).toFile();
                if (manifest.isFile()) {
                    return manifest.getAbsolutePath();
                }
            }
            currentDirectory = currentDirectory.getParent();
        }

        return null;
    }

    private String runToolList(String directoryThatContainsFile, boolean isGlobal) {
        var command = isGlobal ? List.of("tool", "list", "-g") : List.of("tool", "list");
        var output = DotNetProvider.getInstance(this.project).execDotNet(
            command,
            new File(directoryThatContainsFile)
        );

        this.logger.debug(
                "Running 'dotnet tool list" + (isGlobal ? "-g" : "") + "' to look for version"
            );
        this.logger.debug("Output was: \n " + output);

        if (output == null) {
            return null;
        }

        var lines = Arrays.stream(output.split("\n"))
            .map(String::trim)
            .filter(line -> !line.isEmpty())
            .collect(Collectors.toList());

        // The first two lines are headers, so we start at index 2
        for (int i = 2; i < lines.size(); i++) {
            String[] columns = lines.get(i).split("\\s{2,}"); // Split by 2 or more spaces
            if (columns.length >= 2) {
                if (columns[0].equalsIgnoreCase("csharpier")) {
                    return columns[1];
                }
            }
        }

        return null;
    }

    private String findVersionInCsProjOfParentsDirectories(String directoryThatContainsFile) {
        this.logger.debug(
                "Looking for csproj in or above " +
                directoryThatContainsFile +
                " that references CSharpier.MsBuild"
            );
        var currentDirectory = Path.of(directoryThatContainsFile);
        try {
            while (true) {
                var csProjVersion = this.findVersionInCsProj(currentDirectory);
                if (csProjVersion != null) {
                    return csProjVersion;
                }

                if (currentDirectory.getParent() == null) {
                    break;
                }
                currentDirectory = currentDirectory.getParent();
            }
        } catch (Exception ex) {
            this.logger.error(ex);
        }

        return null;
    }

    private String findVersionInCsProj(Path currentDirectory) {
        this.logger.debug("Looking for " + currentDirectory + "/*.csproj");
        File[] csProjFiles = currentDirectory
            .toFile()
            .listFiles((dir, name) -> name.toLowerCase().endsWith(".csproj"));
        if (csProjFiles == null) {
            this.logger.debug(
                    "Unable to list files in directory: " +
                    currentDirectory +
                    " (directory may not exist, not be a directory, or be temporarily inaccessible)"
                );
            return null;
        }
        for (var pathToCsProj : csProjFiles) {
            try {
                var xmlDocument = DocumentBuilderFactory.newInstance()
                    .newDocumentBuilder()
                    .parse(pathToCsProj);

                var selector = XPathFactory.newInstance().newXPath();
                var node = (Node) selector
                    .compile("//PackageReference[@Include='CSharpier.MsBuild']")
                    .evaluate(xmlDocument, XPathConstants.NODE);
                if (node == null) {
                    continue;
                }

                var versionOfMsBuildPackage = node
                    .getAttributes()
                    .getNamedItem("Version")
                    .getNodeValue();
                if (versionOfMsBuildPackage != null) {
                    this.logger.debug(
                            "Found version " + versionOfMsBuildPackage + " in " + pathToCsProj
                        );
                    return versionOfMsBuildPackage;
                }
            } catch (Exception e) {
                this.logger.warn(
                        "The csproj at " +
                        pathToCsProj +
                        " failed to load with the following exception " +
                        e.getMessage()
                    );
            }
        }

        return null;
    }

    private ICSharpierProcess setupCSharpierProcess(String directory, String version) {
        if (version == null || version.equals("")) {
            return NullCSharpierProcess.Instance;
        }

        try {
            if (!this.customPathInstaller.ensureVersionInstalled(version, directory)) {
                this.displayFailureMessage();
                return NullCSharpierProcess.Instance;
            }

            var customPath = this.customPathInstaller.getPathForVersion(version);

            this.logger.debug("Adding new version " + version + " process for " + directory);

            var serverVersion = "0.29.0";

            ICSharpierProcess csharpierProcess;
            if (
                Semver.gte(version, serverVersion) &&
                !CSharpierSettings.getInstance(this.project).getDisableCSharpierServer()
            ) {
                csharpierProcess = new CSharpierProcessServer(customPath, version, this.project);
            } else if (Semver.gte(version, "0.12.0")) {
                var useUtf8 = Semver.gte(version, "0.14.0");

                if (
                    Semver.gte(version, serverVersion) &&
                    CSharpierSettings.getInstance(this.project).getDisableCSharpierServer()
                ) {
                    this.logger.debug(
                            "CSharpier server is disabled, falling back to piping via stdin"
                        );
                }

                csharpierProcess = new CSharpierProcessPipeMultipleFiles(
                    customPath,
                    useUtf8,
                    version,
                    this.project
                );
            } else {
                if (!this.warnedForOldVersion) {
                    var content =
                        "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.";
                    NotificationGroupManager.getInstance()
                        .getNotificationGroup("CSharpier")
                        .createNotification(content, NotificationType.INFORMATION)
                        .notify(this.project);

                    this.warnedForOldVersion = true;
                }

                csharpierProcess = new CSharpierProcessSingleFile(
                    customPath,
                    version,
                    this.project
                );
            }

            if (csharpierProcess.getProcessFailedToStart()) {
                this.displayFailureMessage();
            }

            return csharpierProcess;
        } catch (Exception ex) {
            this.logger.error(ex);
        }

        return NullCSharpierProcess.Instance;
    }

    private void displayFailureMessage() {
        var title = "CSharpier unable to format files";
        var message =
            "CSharpier could not be set up properly so formatting is not currently supported. See log file for more details.";
        var notification = NotificationGroupManager.getInstance()
            .getNotificationGroup("CSharpier")
            .createNotification(title, message, NotificationType.WARNING);
        notification.addAction(
            new OpenUrlAction("Read More", "https://csharpier.com/docs/EditorsTroubleshooting")
        );
        notification.notify(this.project);
    }

    @Override
    public void dispose() {
        this.killRunningProcesses();
    }

    public void killRunningProcesses() {
        for (var key : this.csharpierProcessesByVersion.keySet()) {
            this.logger.debug("disposing of process for version " + (key == "" ? "null" : key));
            this.csharpierProcessesByVersion.get(key).dispose();
        }
        this.lastWarmedByDirectory.clear();
        this.csharpierVersionByDirectory.clear();
        this.toolVersionByManifest.clear();
        this.csharpierProcessesByVersion.clear();
    }
}

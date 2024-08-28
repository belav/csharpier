package com.intellij.csharpier;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.Disposable;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.EditorFactory;
import com.intellij.openapi.editor.event.DocumentEvent;
import com.intellij.openapi.editor.event.DocumentListener;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileEditorManager;
import com.intellij.openapi.project.Project;
import java.io.File;
import java.nio.file.Files;
import java.nio.file.Path;
import java.time.Instant;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathFactory;
import org.jetbrains.annotations.NotNull;
import org.w3c.dom.Node;

public class CSharpierProcessProvider implements DocumentListener, Disposable, IProcessKiller {

    private final CustomPathInstaller customPathInstaller;
    private final Logger logger = CSharpierLogger.getInstance();
    private final Project project;

    private boolean warnedForOldVersion;
    private final HashMap<String, Long> lastWarmedByDirectory = new HashMap<>();
    private final HashMap<String, String> csharpierVersionByDirectory = new HashMap<>();
    private final HashMap<String, ICSharpierProcess> csharpierProcessesByVersion = new HashMap<>();

    public CSharpierProcessProvider(@NotNull Project project) {
        this.project = project;
        this.customPathInstaller = new CustomPathInstaller(project);

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

        if (
            file == null ||
            file.getExtension() == null ||
            !file.getExtension().equalsIgnoreCase("cs")
        ) {
            return;
        }
        var filePath = file.getPath();
        this.findAndWarmProcess(filePath);
    }

    private void findAndWarmProcess(String filePath) {
        // if we didn't find dotnet bail out so we don't get extra errors about being unable to format
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return;
        }
        var directory = Path.of(filePath).getParent().toString();
        var now = Instant.now().toEpochMilli();
        var lastWarmed = this.lastWarmedByDirectory.getOrDefault(directory, Long.valueOf(0));
        if (lastWarmed + 5000 > now) {
            return;
        }
        this.logger.debug("Ensure there is a csharpier process for " + directory);
        this.lastWarmedByDirectory.put(directory, now);
        var version = this.csharpierVersionByDirectory.getOrDefault(directory, null);
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

        if (!this.csharpierProcessesByVersion.containsKey(version)) {
            this.csharpierProcessesByVersion.put(
                    version,
                    this.setupCSharpierProcess(directory, version)
                );
        }
    }

    public ICSharpierProcess getProcessFor(String filePath) {
        // if we didn't find dotnet bail out so we don't get extra errors about being unable to format
        if (!DotNetProvider.getInstance(this.project).foundDotNet()) {
            return NullCSharpierProcess.Instance;
        }

        var directory = Path.of(filePath).getParent().toString();
        var version = this.csharpierVersionByDirectory.getOrDefault(directory, null);
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
        for (var pathToCsProj : currentDirectory
            .toFile()
            .listFiles((dir, name) -> name.toLowerCase().endsWith(".csproj"))) {
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
            if (!this.customPathInstaller.ensureVersionInstalled(version)) {
                this.displayFailureMessage();
                return NullCSharpierProcess.Instance;
            }

            var customPath = this.customPathInstaller.getPathForVersion(version);

            this.logger.debug("Adding new version " + version + " process for " + directory);

            // ComparableVersion was unhappy in rider 2023, this code should probably just go away
            // but there are still 0.12 and 0.14 downloads happening
            var installedVersion = version.split("\\.");
            var versionWeCareAbout = Integer.parseInt(installedVersion[1]);
            var serverVersion = 29;

            ICSharpierProcess csharpierProcess;
            if (
                versionWeCareAbout >= serverVersion &&
                !CSharpierSettings.getInstance(this.project).getDisableCSharpierServer()
            ) {
                csharpierProcess = new CSharpierProcessServer(customPath, version, this.project);
            } else if (versionWeCareAbout >= 12) {
                var useUtf8 = versionWeCareAbout >= 14;

                if (
                    versionWeCareAbout >= serverVersion &&
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
        this.csharpierProcessesByVersion.clear();
    }
}

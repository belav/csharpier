package com.intellij.csharpier;

import com.intellij.notification.NotificationGroupManager;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.project.Project;
import com.jetbrains.rider.runtime.RiderDotNetActiveRuntimeHost;
import java.io.File;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import org.jetbrains.annotations.NotNull;

public class DotNetProvider {

    private final Logger logger = CSharpierLogger.getInstance();
    private final Project project;
    private String dotNetRoot;
    private String cliExePath;

    public DotNetProvider(@NotNull Project project) {
        this.project = project;
    }

    static DotNetProvider getInstance(@NotNull Project project) {
        return project.getService(DotNetProvider.class);
    }

    void initialize() {
        var foundDotNet = this.findDotNet();
        if (!foundDotNet) {
            var title = "CSharpier unable to run dotnet commands";
            var message =
                "CSharpier was unable to determine how to run dotnet commands. Ensure that '.NET CLI executable path' is set properly in your settings or dotnet is available on PATH and restart.";
            var notification = NotificationGroupManager.getInstance()
                .getNotificationGroup("CSharpier")
                .createNotification(title, message, NotificationType.WARNING);
            notification.notify(this.project);
        }
    }

    private boolean findDotNet() {
        try {
            var dotNetCoreRuntime = RiderDotNetActiveRuntimeHost.Companion.getInstance(project)
                .getDotNetCoreRuntime()
                .getValue();

            if (dotNetCoreRuntime != null && dotNetCoreRuntime.getCliExePath() != null) {
                this.logger.debug(
                        "Using dotnet found from RiderDotNetActiveRuntimeHost at " +
                        dotNetCoreRuntime.getCliExePath()
                    );
                this.cliExePath = dotNetCoreRuntime.getCliExePath();
            } else {
                return false;
            }

            this.dotNetRoot = Paths.get(this.cliExePath).getParent().toString();

            return true;
        } catch (Exception ex) {
            logger.error(ex);

            return false;
        }
    }

    /// Detects the .Net SDK architecture by running `dotnet --info` and looks for the relevant
    /// line, for example.
    ///
    /// ```
    /// ...
    /// Host:
    ///   Version:      9.0.1
    ///   Architecture: arm64
    ///   Commit:       c8acea2262
    /// ...
    /// ```
    ///
    /// The use case is to help {@link CustomPathInstaller} installs csharpier into different
    /// location per architecture. So that users who need to use multiple SDKs of different
    /// architectures doesn't need to reinstall it everytime.
    ///
    /// @return e.g. x64, arm64, x86, ...
    public Optional<String> getArchitecture() {
        String[] lines = execDotNet(List.of("--info"), null).split(System.lineSeparator());
        for (String line : lines) {
            if (line.contains(" Architecture: ")) {
                var parts = line.split(":");
                return Optional.of(parts.length > 1 ? parts[1].trim() : line);
            }
        }
        return Optional.empty();
    }

    public String execDotNet(List<String> command, File workingDirectory) {
        var commands = new ArrayList<>(command);
        commands.add(0, this.cliExePath);

        var env = Map.of(
            "DOTNET_NOLOGO",
            "1",
            "DOTNET_CLI_TELEMETRY_OPTOUT",
            "1",
            "DOTNET_SKIP_FIRST_TIME_EXPERIENCE",
            "1"
        );

        return ProcessHelper.executeCommand(commands, env, workingDirectory);
    }

    public String getDotNetRoot() {
        return this.dotNetRoot;
    }

    public boolean foundDotNet() {
        return this.cliExePath != null;
    }
}

package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class DotNetFinder {

    public static String findOnPath(Logger logger) {
        logger.debug("Trying to find dotnet on PATH");

        var env = Map.of(
                "DOTNET_CLI_UI_LANGUAGE", "en-US",
                "DOTNET_NOLOGO", "1",
                "DOTNET_CLI_TELEMETRY_OPTOUT", "1",
                "DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "1");
        var dotnetInfo = ProcessHelper.executeCommand(List.of("dotnet", "--info"), env, null);
        if (dotnetInfo == null) {
            return null;
        }

        String version = null;

        var lines = dotnetInfo.split("\\r?\\n");
        for (var line : lines) {
            var pattern = Pattern.compile("^\\s*Version:\\s*([^\\s].*)$");
            var matcher = pattern.matcher(line);

            if (matcher.find()) {
                version = matcher.group(1);
            }
        }

        if (version == null) {
            return null;
        }

        var runtimesOutput = ProcessHelper.executeCommand(List.of("dotnet", "--list-runtimes"), env, null);
        var runtimeVersions = new HashMap<String, List<RuntimeInfo>>();
        lines = runtimesOutput.split("\\r?\\n");
        for (var line : lines) {
            var pattern = Pattern.compile("^([\\w.]+) ([^ ]+) \\[([^\\]]+)\\]$");
            var matcher = pattern.matcher(line);

            if (matcher.find()) {
                var runtime = matcher.group(1);
                var runtimeVersion = matcher.group(2);
                var path = matcher.group(3);

                if (!runtimeVersions.containsKey(runtime)) {
                    var versions = new ArrayList<RuntimeInfo>();
                    runtimeVersions.put(runtime, versions);
                }

                var runtimeInfo = new RuntimeInfo();
                runtimeInfo.Version = runtimeVersion;
                runtimeInfo.Path = path;
                runtimeVersions.get(runtime).add(runtimeInfo);
            }
        }

        return findDotNetFromRuntimes(runtimeVersions, logger);
    }

    private static String findDotNetFromRuntimes(HashMap<String, List<RuntimeInfo>> runtimes, Logger logger) {
        var requiredRuntimeVersion = "6.0.0";

        var coreRuntimeVersions = runtimes.get("Microsoft.NETCore.App");
        RuntimeInfo matchingRuntime = null;
        for (var runtime : coreRuntimeVersions) {
            // We consider a match if the runtime is greater than or equal to the required version since we roll forward.
            if (compareVersions(runtime.Version, requiredRuntimeVersion) > 0) {
                logger.debug("Using " + runtime.Path + " with version " + runtime.Version);
                matchingRuntime = runtime;
                break;
            }
        }

        if (matchingRuntime == null) {
            return null;
        }

        // The .NET install layout is a well known structure on all platforms.
        // See https://github.com/dotnet/designs/blob/main/accepted/2020/install-locations.md#net-core-install-layout
        //
        // Therefore we know that the runtime path is always in <install root>/shared/<runtime name>
        // and the dotnet executable is always at <install root>/dotnet(.exe).
        //
        // Since dotnet --list-runtimes will always use the real assembly path to output the runtime folder (no symlinks!)
        // we know the dotnet executable will be two folders up in the install root.
        var runtimeFolderPath = matchingRuntime.Path;
        var installFolder = Paths.get(runtimeFolderPath).getParent().getParent().toString();
        var dotnetExecutablePath = Paths.get(installFolder, System.getProperty("os.name").contains("Windows") ? "dotnet.exe" : "dotnet").toString();

        if (!Files.exists(Paths.get(dotnetExecutablePath))) {
            throw new RuntimeException(String.format("dotnet executable path does not exist: %s, dotnet installation may be corrupt.", dotnetExecutablePath));
        }

        return dotnetExecutablePath;
    }

    private static int compareVersions(String version1, String version2) {
        String[] parts1 = version1.split("\\.");
        String[] parts2 = version2.split("\\.");

        int minLength = Math.min(parts1.length, parts2.length);

        for (int i = 0; i < minLength; i++) {
            int part1 = Integer.parseInt(parts1[i]);
            int part2 = Integer.parseInt(parts2[i]);

            if (part1 < part2) {
                return -1;
            } else if (part1 > part2) {
                return 1;
            }
        }

        return Integer.compare(parts1.length, parts2.length);
    }
}

class RuntimeInfo {
    public String Version;
    public String Path;
}

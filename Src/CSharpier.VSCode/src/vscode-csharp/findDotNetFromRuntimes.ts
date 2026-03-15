/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

import { DotnetInfo, RuntimeInfo } from "./dotnetinfo";
import * as semver from "semver";
import * as path from "path";
import { existsSync } from "fs";

export function findDotNetFromRuntimes(dotnetInfo: DotnetInfo) {
    let requiredRuntimeVersion = "6.0.0";

    let coreRuntimeVersions = dotnetInfo.Runtimes["Microsoft.NETCore.App"];
    let matchingRuntime: RuntimeInfo | undefined = undefined;
    for (let runtime of coreRuntimeVersions) {
        // We consider a match if the runtime is greater than or equal to the required version since we roll forward.
        if (semver.gte(runtime.Version, requiredRuntimeVersion)) {
            matchingRuntime = runtime;
            break;
        }
    }

    if (!matchingRuntime) {
        throw new Error(
            `No compatible .NET runtime found. Minimum required version is ${requiredRuntimeVersion}.`,
        );
    }

    // The .NET install layout is a well known structure on all platforms.
    // See https://github.com/dotnet/designs/blob/main/accepted/2020/install-locations.md#net-core-install-layout
    //
    // Therefore we know that the runtime path is always in <install root>/shared/<runtime name>
    // and the dotnet executable is always at <install root>/dotnet(.exe).
    //
    // Since dotnet --list-runtimes will always use the real assembly path to output the runtime folder (no symlinks!)
    // we know the dotnet executable will be two folders up in the install root.
    let runtimeFolderPath = matchingRuntime.Path;
    let installFolder = path.dirname(path.dirname(runtimeFolderPath));
    let dotnetExecutablePath = path.join(
        installFolder,
        process.platform === "win32" ? "dotnet.exe" : "dotnet",
    );
    if (!existsSync(dotnetExecutablePath)) {
        throw new Error(
            `dotnet executable path does not exist: ${dotnetExecutablePath}, dotnet installation may be corrupt.`,
        );
    }
    return dotnetExecutablePath;
}

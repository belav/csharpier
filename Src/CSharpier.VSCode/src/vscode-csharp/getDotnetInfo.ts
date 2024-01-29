/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

import * as semver from "semver";
import { join } from "path";
import { EOL } from "os";
import { DotnetInfo, RuntimeInfo } from "./dotnetinfo";
import * as fs from "fs";
import * as cp from "child_process";

// This function calls `dotnet --info` and returns the result as a DotnetInfo object.
export async function getDotnetInfo(dotNetCliPaths: string[]): Promise<DotnetInfo> {
    const dotnetExecutablePath = getDotNetExecutablePath(dotNetCliPaths);

    const data = await runDotnetInfo(dotnetExecutablePath);
    const dotnetInfo = await parseDotnetInfo(data, dotnetExecutablePath);
    return dotnetInfo;
}

export function getDotNetExecutablePath(dotNetCliPaths: string[]): string | undefined {
    const dotnetExeName = process.platform === "win32" ? "dotnet.exe" : "dotnet";
    let dotnetExecutablePath: string | undefined;

    for (const dotnetPath of dotNetCliPaths) {
        const dotnetFullPath = join(dotnetPath, dotnetExeName);
        if (existsSync(dotnetFullPath)) {
            dotnetExecutablePath = dotnetFullPath;
            break;
        }
    }
    return dotnetExecutablePath;
}

async function runDotnetInfo(dotnetExecutablePath: string | undefined): Promise<string> {
    try {
        const env = {
            ...process.env,
            DOTNET_CLI_UI_LANGUAGE: "en-US",
        };
        const command = dotnetExecutablePath ? `"${dotnetExecutablePath}"` : "dotnet";
        const data = await execChildProcess(`${command} --info`, process.cwd(), env);
        return data;
    } catch (error) {
        const message = error instanceof Error ? error.message : `${error}`;
        throw new Error(`Error running dotnet --info: ${message}`);
    }
}

async function parseDotnetInfo(
    dotnetInfo: string,
    dotnetExecutablePath: string | undefined,
): Promise<DotnetInfo> {
    try {
        const cliPath = dotnetExecutablePath;
        const fullInfo = dotnetInfo;

        let version: string | undefined;
        let runtimeId: string | undefined;
        let architecture: string | undefined;

        let lines = dotnetInfo.replace(/\r/gm, "").split("\n");
        for (const line of lines) {
            let match: RegExpMatchArray | null;
            if ((match = /^\s*Version:\s*([^\s].*)$/.exec(line))) {
                version = match[1];
            } else if ((match = /^ RID:\s*([\w\-.]+)$/.exec(line))) {
                runtimeId = match[1];
            } else if ((match = /^\s*Architecture:\s*(.*)/.exec(line))) {
                architecture = match[1];
            }
        }

        const runtimeVersions: { [runtime: string]: RuntimeInfo[] } = {};
        const command = dotnetExecutablePath ? `"${dotnetExecutablePath}"` : "dotnet";
        const listRuntimes = await execChildProcess(
            `${command} --list-runtimes`,
            process.cwd(),
            process.env,
        );
        lines = listRuntimes.split(/\r?\n/);
        for (const line of lines) {
            let match: RegExpMatchArray | null;
            if ((match = /^([\w.]+) ([^ ]+) \[([^\]]+)\]$/.exec(line))) {
                const runtime = match[1];
                const runtimeVersion = match[2];
                if (runtime in runtimeVersions) {
                    runtimeVersions[runtime].push({
                        Version: semver.parse(runtimeVersion)!,
                        Path: match[3],
                    });
                } else {
                    runtimeVersions[runtime] = [
                        {
                            Version: semver.parse(runtimeVersion)!,
                            Path: match[3],
                        },
                    ];
                }
            }
        }

        if (version !== undefined) {
            const dotnetInfo: DotnetInfo = {
                CliPath: cliPath,
                FullInfo: fullInfo,
                Version: version,
                RuntimeId: runtimeId,
                Architecture: architecture,
                Runtimes: runtimeVersions,
            };
            return dotnetInfo;
        }

        throw new Error("Failed to parse dotnet version information");
    } catch (error) {
        const message = error instanceof Error ? error.message : `${error}`;
        throw new Error(
            `Error parsing dotnet --info: ${message}, raw info was:${EOL}${dotnetInfo}`,
        );
    }
}

function existsSync(path: string): boolean {
    try {
        fs.accessSync(path, fs.constants.F_OK);
        return true;
    } catch (err) {
        const error = err as NodeJS.ErrnoException;
        if (error.code === "ENOENT" || error.code === "ENOTDIR") {
            return false;
        } else {
            throw Error(error.code);
        }
    }
}

async function execChildProcess(
    command: string,
    workingDirectory: string,
    env: NodeJS.ProcessEnv = {},
): Promise<string> {
    return new Promise<string>((resolve, reject) => {
        cp.exec(
            command,
            { cwd: workingDirectory, maxBuffer: 500 * 1024, env: env },
            (error: any, stdout: any, stderr: any) => {
                if (error) {
                    reject(
                        new Error(`${error}
${stdout}
${stderr}`),
                    );
                } else if (stderr && !stderr.includes("screen size is bogus")) {
                    reject(new Error(stderr));
                } else {
                    resolve(stdout);
                }
            },
        );
    });
}

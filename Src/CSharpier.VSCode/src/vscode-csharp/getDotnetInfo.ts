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
import { Logger } from "../Logger";

// This function calls `dotnet --info` and returns the result as a DotnetInfo object.
export async function getDotnetInfo(dotNetCliPaths: string[], logger: Logger): Promise<DotnetInfo> {
    let dotnetExecutablePath = getDotNetExecutablePath(dotNetCliPaths);

    let data = await runDotnetInfo(dotnetExecutablePath, logger);
    return await parseDotnetInfo(data, dotnetExecutablePath);
}

export function getDotNetExecutablePath(dotNetCliPaths: string[]): string | undefined {
    let dotnetExeName = process.platform === "win32" ? "dotnet.exe" : "dotnet";
    let dotnetExecutablePath: string | undefined;

    for (let dotnetPath of dotNetCliPaths) {
        let dotnetFullPath = join(dotnetPath, dotnetExeName);
        if (existsSync(dotnetFullPath)) {
            dotnetExecutablePath = dotnetFullPath;
            break;
        }
    }
    return dotnetExecutablePath;
}

async function runDotnetInfo(
    dotnetExecutablePath: string | undefined,
    logger: Logger,
): Promise<string> {
    if (dotnetExecutablePath == undefined) {
        logger.debug("Trying to find dotnet on PATH using 'dotnet --info' ");
    }

    let env = {
        ...process.env,
        DOTNET_CLI_UI_LANGUAGE: "en-US",
    };

    try {
        let command = dotnetExecutablePath ? `"${dotnetExecutablePath}"` : "dotnet";
        return await execChildProcess(`${command} --info`, process.cwd(), env);
    } catch (error) {
        logger.error(error);

        if (process.platform !== "win32") {
            logger.debug("Trying to find dotnet on PATH using 'sh -c \"dotnet --info\"'");
            try {
                return await execChildProcess(`sh -c "dotnet --info"`, process.cwd(), env);
            } catch (error) {
                let message = error instanceof Error ? error.message : `${error}`;
                throw new Error(`Error running dotnet --info: ${message}`);
            }
        } else {
            let message = error instanceof Error ? error.message : `${error}`;
            throw new Error(`Error running dotnet --info: ${message}`);
        }
    }
}

async function parseDotnetInfo(
    dotnetInfo: string,
    dotnetExecutablePath: string | undefined,
): Promise<DotnetInfo> {
    try {
        let cliPath = dotnetExecutablePath;

        let version: string | undefined;

        let lines = dotnetInfo.replace(/\r/gm, "").split("\n");
        for (let line of lines) {
            let match: RegExpMatchArray | null;
            if ((match = /^\s*Version:\s*([^\s].*)$/.exec(line))) {
                version = match[1];
            }
        }

        let runtimeVersions: { [runtime: string]: RuntimeInfo[] } = {};
        let command = dotnetExecutablePath ? `"${dotnetExecutablePath}"` : "dotnet";
        let listRuntimes = await execChildProcess(
            `${command} --list-runtimes`,
            process.cwd(),
            process.env,
        );
        lines = listRuntimes.split(/\r?\n/);
        for (let line of lines) {
            let match: RegExpMatchArray | null;
            if ((match = /^([\w.]+) ([^ ]+) \[([^\]]+)\]$/.exec(line))) {
                let runtime = match[1];
                let runtimeVersion = match[2];
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
            return {
                CliPath: cliPath,
                Runtimes: runtimeVersions,
            };
        }

        throw new Error("Failed to parse dotnet version information");
    } catch (error) {
        let message = error instanceof Error ? error.message : `${error}`;
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
        let error = err as NodeJS.ErrnoException;
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

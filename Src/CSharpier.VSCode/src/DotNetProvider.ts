import { execSync, ExecSyncOptionsWithBufferEncoding } from "child_process";
import { options } from "./Options";
import { Logger } from "./Logger";
import { getDotnetInfo } from "./vscode-csharp/getDotnetInfo";
import { findDotNetFromRuntimes } from "./vscode-csharp/findDotNetFromRuntimes";
import * as path from "path";

export type ExecDotNet = (command: string, cwd?: string | undefined) => Buffer;

let dotNetRoot = "";
export let getDotNetRoot = () => dotNetRoot;

let exec: ExecDotNet = () => {
    throw new Error("Did not call setExecDotNet");
};
export let execDotNet = (command: string, cwd?: string | undefined) => {
    return exec(command, cwd);
};

export let findDotNet = async (logger: Logger) => {
    let dotnetPathOption = options.dotnetPath;
    let dotNetCliPaths = options.dotNetCliPaths;
    let paths = [dotnetPathOption, ...dotNetCliPaths];

    try {
        let dotnetInfo = await getDotnetInfo(paths, logger);

        let dotnetExecutablePath = dotnetInfo.CliPath;
        if (dotnetExecutablePath !== undefined) {
            logger.debug("Using dotnet found from settings at " + dotnetExecutablePath);
        } else {
            dotnetExecutablePath = findDotNetFromRuntimes(dotnetInfo);
            logger.debug("Using dotnet found from PATH at " + dotnetExecutablePath);
        }

        dotNetRoot = path.dirname(dotnetExecutablePath);

        exec = (command: string, cwd: string | undefined): Buffer => {
            let options = {
                cwd,
                env: { ...process.env, DOTNET_NOLOGO: "1" },
            };

            return execSync(`"${dotnetExecutablePath}" ${command}`, options);
        };

        return true;
    } catch (error) {
        logger.error(error);

        return false;
    }
};

export let getArchitecture = () => {
    let lines = execDotNet("--info").toString().trim().split(/\r?\n/);
    for (let line of lines) {
        if (line.includes(" Architecture: ")) {
            let parts = line.split(":");
            return parts.length > 1 ? parts[1].trim() : line;
        }
    }

    return null;
};

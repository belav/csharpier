import { execSync, ExecSyncOptionsWithBufferEncoding } from "child_process";
import { options } from "./Options";
import { Logger } from "./Logger";
import { getDotnetInfo } from "./vscode-csharp/getDotnetInfo";
import { findDotNetFromRuntimes } from "./vscode-csharp/findDotNetFromRuntimes";
import * as path from "path";

export type ExecDotNet = (command: string, cwd?: string | undefined) => Buffer;

let dotNetRoot = "";
export const getDotNetRoot = () => dotNetRoot;

let exec: ExecDotNet = () => {
    throw new Error("Did not call setExecDotNet");
};
export const execDotNet = (command: string, cwd?: string | undefined) => {
    return exec(command, cwd);
};

export const findDotNet = async (logger: Logger) => {
    const dotnetPathOption = options.dotnetPath;
    const dotNetCliPaths = options.dotNetCliPaths;
    const paths = [dotnetPathOption, ...dotNetCliPaths];

    try {
        const dotnetInfo = await getDotnetInfo(paths, logger);

        let dotnetExecutablePath = dotnetInfo.CliPath;
        if (dotnetExecutablePath !== undefined) {
            logger.debug("Using dotnet found from settings at " + dotnetExecutablePath);
        } else {
            dotnetExecutablePath = findDotNetFromRuntimes(dotnetInfo);
            logger.debug("Using dotnet found from PATH at " + dotnetExecutablePath);
        }

        dotNetRoot = path.dirname(dotnetExecutablePath);

        exec = (command: string, cwd: string | undefined): Buffer => {
            const options = {
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

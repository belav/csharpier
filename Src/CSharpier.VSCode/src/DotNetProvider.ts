import { execSync, ExecSyncOptionsWithBufferEncoding } from "child_process";
import { options } from "./Options";
import { Logger } from "./Logger";
import { getDotnetInfo } from "./vscode-csharp/getDotnetInfo";
import { findDotNetFromRuntimes } from "./vscode-csharp/findDotNetFromRuntimes";

export type ExecDotNet = (
    command: string,
    options?: ExecSyncOptionsWithBufferEncoding | undefined,
) => Buffer;

export const getExecDotNet = async (logger: Logger): Promise<ExecDotNet | null> => {
    return await doDotNetInfoWay(logger);
    //
    //
    // const fileName = process.platform === "win32" ? "dotnet.exe" : "dotnet";
    // const env = { ...process.env };
    //
    // // TODO test without this but with a sym link
    // // TODO what about using the sh stuff?
    // const dotnetPathOption = options.dotnetPath;
    // if (dotnetPathOption.length > 0) {
    //     env.PATH = dotnetPathOption + path.delimiter + env.PATH;
    //     logger.debug("including " + options.dotnetPath + " in the path to test for dotnet");
    // }
    // const dotNetCliPaths = options.dotNetCliPaths;
    //
    // for (const dotnetPath of dotNetCliPaths) {
    //     env.PATH = env.PATH + path.delimiter + dotnetPath;
    //     logger.debug("including " + dotnetPath + " in the path to test for dotnet");
    // }
    //
    // let result: { stdout: string; stderr: string } = { stdout: "", stderr: "" };
    //
    // try {
    //     result = await promisify(exec)(`${fileName} --version`, { env });
    // } catch (exception) {
    //     result.stderr = exception as any;
    // }
    //
    // let useSH = false;
    // let foundDotnet = true;
    // if (result.stderr) {
    //     logger.warn(`Unable to read dotnet version information. \n ${result.stderr}`);
    //     useSH = true;
    //     try {
    //         result = await promisify(exec)(`sh -c "${fileName} --version"`, { env });
    //     } catch (exception) {
    //         result.stderr = exception as any;
    //     }
    //     if (result.stderr) {
    //         logger.warn(
    //             `Unable to read dotnet version information using "sh -c". Error ${result.stderr}`,
    //         );
    //         foundDotnet = false;
    //     }
    // }
    //
    // if (!foundDotnet) {
    //     return null;
    // }
    //
    // return (command: string, options?: ExecSyncOptionsWithBufferEncoding): Buffer => {
    //     if (useSH) {
    //         command = `sh -c "${command}"`;
    //     }
    //
    //     if (options === undefined) {
    //         options = {};
    //     }
    //
    //     if (options.env === undefined) {
    //         options.env = { ...process.env };
    //     }
    //
    //     options.env.PATH = env.PATH;
    //
    //     return execSync(command, options);
    // };
};

// TODO we need to find dotnet via sh if these two options are not set and we can't find it, probably with runDotnetInfo
async function doDotNetInfoWay(logger: Logger) {
    const dotNetCliPaths = options.dotNetCliPaths;
    const dotnetPathOption = options.dotnetPath;

    const dotnetInfo = await getDotnetInfo([dotnetPathOption, ...dotNetCliPaths]);
    logger.info(JSON.stringify(dotnetInfo, null, 4));

    let dotnetExecutablePath = dotnetInfo.CliPath;
    if (!dotnetExecutablePath) {
        dotnetExecutablePath = findDotNetFromRuntimes(dotnetInfo);
    }

    return (command: string, options?: ExecSyncOptionsWithBufferEncoding): Buffer => {
        if (options === undefined) {
            options = {};
        }

        return execSync(dotnetExecutablePath + " " + command, options);
    };
}

/*
bela@ubuntu-two:~/.dotnet$ find /usr -name "dotnet" -type f
bela@ubuntu-two:~/.dotnet$ find ~ -name "dotnet" -type f

whereis

which

install-script put it at with install script seems to go to ~/.dotnet
gh user has it in /usr/bin, which seems like it should be standard

after running
sudo ln -s ~/.dotnet/dotnet /usr/bin/dotnet
then I get
You must install .NET to run this application.

but it installed just fine
global install has the same problem

You must install .NET to run this application.

was missing DOTNET_ROOT, should try to set it automatically? roslynLanguageServer does

*/

import { execSync, ExecSyncOptionsWithBufferEncoding } from "child_process";
import { options } from "./Options";
import { Logger } from "./Logger";
import { getDotnetInfo } from "./vscode-csharp/getDotnetInfo";
import { findDotNetFromRuntimes } from "./vscode-csharp/findDotNetFromRuntimes";

/*
Notes

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

// this makes use of logic from vscode-csharp along with the options it provides to find where
// dotnet lives so that we can reliably execute dotnet commands
export type ExecDotNet = (
    command: string,
    options?: ExecSyncOptionsWithBufferEncoding | undefined,
) => Buffer;

// TODO test with options set
// TODO also test with dotnet in the path
// TODO ask the user from the one issue that used arch to test the sh version + path
// TODO just import this where needed instead of passing it into constructors
// TODO need to make sure nothing in the vscode-csharp stuff throws
// TODO we need to find dotnet via sh if these two options are not set and we can't find it, probably with runDotnetInfo
export const getExecDotNet = async (logger: Logger): Promise<ExecDotNet | null> => {
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
};

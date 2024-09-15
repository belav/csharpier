import { Logger } from "./Logger";
import { Extension, window, workspace } from "vscode";
import * as path from "path";
import * as fs from "fs";
import * as vscode from "vscode";
import { execDotNet } from "./DotNetProvider";

export class InstallerService {
    rejectedError = false;
    rejectedGlobalError = false;
    errorVisible = false;
    globalErrorVisible = false;
    logger: Logger;
    killRunningProcesses: () => void;
    extension: Extension<unknown>;
    warnedAlready = false;

    constructor(logger: Logger, killRunningProcesses: () => void, extension: Extension<unknown>) {
        this.logger = logger;
        this.killRunningProcesses = killRunningProcesses;
        this.extension = extension;
    }

    public displayInstallNeededMessage = (directoryThatContainsFile: string) => {
        if (this.warnedAlready || this.ignoreDirectory(directoryThatContainsFile)) {
            return;
        }

        this.warnedAlready = true;
        this.logger.error("CSharpier was not found so files may not be formatted.");

        this.logger.info(this.extension.extensionKind);

        if (this.extension.extensionKind === vscode.ExtensionKind.Workspace) {
            window.showErrorMessage(
                "CSharpier must be installed as a local dotnet tool in ./.config/dotnet-tools.json using `dotnet tool install csharpier` and the devcontainer rebuilt to support formatting files.",
            );
            return;
        }

        const globalButton = "Install as global tool";
        const localButton = "Install as local tool";

        const items = [globalButton];
        let solutionRoot: string;
        if (workspace.workspaceFolders) {
            for (const folder of workspace.workspaceFolders) {
                if (directoryThatContainsFile.startsWith(folder.uri.fsPath)) {
                    solutionRoot = folder.uri.fsPath;
                    items.unshift(localButton);
                    break;
                }
            }
        }

        const isOnlyGlobal = items.length === 1;
        let message: string;

        if (isOnlyGlobal) {
            if (this.globalErrorVisible || this.rejectedGlobalError) {
                return;
            }
            this.globalErrorVisible = true;
            message =
                "CSharpier needs to be installed globally to format files in " +
                directoryThatContainsFile;
        } else {
            if (this.errorVisible || this.rejectedError) {
                return;
            }

            this.errorVisible = true;
            message = "CSharpier needs to be installed to support formatting files";
        }

        window.showErrorMessage(message, ...items).then(
            selection => {
                if (selection === globalButton) {
                    const command = "dotnet tool install -g csharpier";
                    this.logger.info("Installing csharpier globally with " + command);
                    const output = execDotNet(command).toString();
                    this.logger.info(output);
                } else if (selection === localButton) {
                    if (solutionRoot) {
                        try {
                            const manifestPath = path.join(
                                solutionRoot,
                                ".config/dotnet-tools.json",
                            );
                            this.logger.info("Installing csharpier in " + manifestPath);
                            if (!fs.existsSync(manifestPath)) {
                                execDotNet("new tool-manifest", solutionRoot);
                            }
                            execDotNet("tool install csharpier", solutionRoot);
                        } catch (error) {
                            this.logger.error("Installing failed with ", error);
                        }
                    }
                } else {
                    if (isOnlyGlobal) {
                        this.rejectedError = true;
                    } else {
                        this.rejectedGlobalError = true;
                    }

                    this.logger.debug("rejected");
                }

                if (isOnlyGlobal) {
                    this.globalErrorVisible = false;
                } else {
                    this.errorVisible = false;
                }

                this.killRunningProcesses();
            },
            () => {
                if (isOnlyGlobal) {
                    this.globalErrorVisible = false;
                    this.rejectedGlobalError = true;
                } else {
                    this.errorVisible = false;
                    this.rejectedError = true;
                }
            },
        );
    };

    private ignoreDirectory(directoryThatContainsFile: string) {
        const normalizedPath = directoryThatContainsFile.replace(/\\/g, "/");
        return (
            normalizedPath.indexOf("/DecompilationMetadataAsSourceFileProvider/") >= 0 ||
            normalizedPath === "/"
        );
    }
}

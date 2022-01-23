import { Logger } from "./Logger";
import { window, workspace } from "vscode";
import { execSync } from "child_process";
import * as path from "path";
import * as fs from "fs";

export class InstallerService {
    rejectedError = false;
    rejectedGlobalError = false;
    errorVisible = false;
    globalErrorVisible = false;
    logger: Logger;
    killRunningProcesses: () => void;

    constructor(logger: Logger, killRunningProcesses: () => void) {
        this.logger = logger;
        this.killRunningProcesses = killRunningProcesses;
    }

    public displayInstallNeededMessage = (directoryThatContainsFile: string) => {
        this.logger.error("CSharpier was not found so files may not be formatted.");

        const items = ["Install CSharpier Globally"];
        let solutionRoot: string;
        if (workspace.workspaceFolders) {
            for (const folder of workspace.workspaceFolders) {
                if (directoryThatContainsFile.startsWith(folder.uri.fsPath)) {
                    solutionRoot = folder.uri.fsPath;
                    items.unshift("Install CSharpier Locally");
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
                if (selection === "Install CSharpier Globally") {
                    const command = "dotnet tool install -g csharpier";
                    this.logger.info("Installing csharpier globally with " + command);
                    const output = execSync(command).toString();
                    this.logger.info(output);
                } else if (selection === "Install CSharpier Locally") {
                    if (solutionRoot) {
                        try {
                            const manifestPath = path.join(
                                solutionRoot,
                                ".config/dotnet-tools.json",
                            );
                            this.logger.info("Installing csharpier in " + manifestPath);
                            if (!fs.existsSync(manifestPath)) {
                                execSync("dotnet new tool-manifest", { cwd: solutionRoot });
                            }
                            execSync("dotnet tool install csharpier", { cwd: solutionRoot });
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
}

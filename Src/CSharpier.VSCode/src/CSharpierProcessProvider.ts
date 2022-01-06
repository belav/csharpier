import { Disposable, TextEditor, window, workspace } from "vscode";
import { LoggingService } from "./LoggingService";
import * as path from "path";
import * as semver from "semver";
import { execSync } from "child_process";
import { ICSharpierProcess, NullCSharpierProcess } from "./CSharpierProcess";
import { CSharpierProcessSingleFile } from "./CSharpierProcessSingleFile";
import { CSharpierProcessPipeMultipleFiles } from "./CSharpierProcessPipeMultipleFiles";

export class CSharpierProcessProvider implements Disposable {

    loggingService: LoggingService;
    csharpierPath: string;
    warmingByDirectory: Record<string, boolean | undefined> = {};
    csharpierVersionByDirectory: Record<string, string | undefined> = {};
    csharpierProcessesByVersion: Record<string, ICSharpierProcess | undefined> = {};

    constructor(loggingService: LoggingService) {
        this.loggingService = loggingService;

        let timeoutHandle: NodeJS.Timeout;
        const setupKillRunningProcesses = () => {
            // TODO we can't detect when the terminal gets focused
            // see https://github.com/microsoft/vscode/issues/117980
            // and we can't detect when the text editor itself loses focus
            // so in order to make sure someone can use the terminal to update csharpier
            // we have to kill off the background process after a set amount of time
            clearTimeout(timeoutHandle);
            timeoutHandle = setTimeout(this.killRunningProcesses, 15000);
        }

        window.onDidChangeWindowState(event => {
            if (!event.focused) {
                this.killRunningProcesses();
            }
        });
        window.onDidChangeActiveTextEditor((event: TextEditor | undefined) => {
            if (event?.document?.languageId !== "csharp") {
                return;
            }
            this.findAndWarmProcess(event.document.fileName);
            setupKillRunningProcesses();
        });
        workspace.onDidChangeTextDocument(event => {
            if (event.document.languageId !== "csharp") {
                return;
            }
            this.loggingService.logDebug(JSON.stringify(window.activeTerminal));
            this.findAndWarmProcess(event.document.fileName);
            clearTimeout(timeoutHandle);
            setupKillRunningProcesses();
        });

        this.csharpierPath = this.getCSharpierPath();

        this.loggingService.logDebug("Using command dotnet " + this.csharpierPath);
    }

    private findAndWarmProcess(fileName: string) {
        const directory = path.parse(fileName).dir;
        this.loggingService.logDebug("Find and warm for " + directory);
        if (this.warmingByDirectory[directory]) {
            return;
        }
        this.warmingByDirectory[directory] = true;
        let version = this.csharpierVersionByDirectory[directory];
        if (!version) {
            version = this.getCSharpierVersion(directory);
            if (!semver.valid(version)) {
                this.displayInstallNeededMessage();
            }
            this.csharpierVersionByDirectory[directory] = version;
        }

        if (!this.csharpierProcessesByVersion[version]) {
            this.csharpierProcessesByVersion[version] = this.setupCSharpierProcess(
                directory,
                version,
            );
            this.loggingService.logDebug("Adding new process for " + directory);
        }
        delete this.warmingByDirectory[directory];
    }

    private getCSharpierPath = () => {
        let csharpierPath = "csharpier";

        // const csharpierDebugPath = path.resolve(
        //     __dirname,
        //     "../../CSharpier.Cli/bin/Debug/net6.0/dotnet-csharpier.dll",
        // );
        // const csharpierReleasePath = csharpierDebugPath.replace("Debug", "Release");
        //
        // if (fs.existsSync(csharpierDebugPath)) {
        //     csharpierPath = csharpierDebugPath;
        // } else if (fs.existsSync(csharpierReleasePath)) {
        //     csharpierPath = csharpierReleasePath;
        // }
        return csharpierPath;
    };

    getProcessFor(fileName: string) {
        const directory = path.parse(fileName).dir;
        let version = this.csharpierVersionByDirectory[directory];
        if (!version) {
            this.findAndWarmProcess(fileName);
            version = this.csharpierVersionByDirectory[directory];
        }

        if (!version || !this.csharpierProcessesByVersion[version])
        {
            // this shouldn't really happen, but just in case
            return new NullCSharpierProcess();
        }

        return this.csharpierProcessesByVersion[version]!;
    }

    private getCSharpierVersion = (directory: string) => {
        const version = execSync("dotnet " + this.csharpierPath + " --version", {
            cwd: directory,
        })
            .toString()
            .trim();
        return version;
    };

    private setupCSharpierProcess = (directory: string, version: string) => {
        try {
            if (!semver.valid(version)) {
                return new NullCSharpierProcess();
            }
            if (semver.lt(version, "0.12.0")) {
                // TODO this should happen once
                window.showInformationMessage(
                    "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.",
                );
                return new CSharpierProcessSingleFile(this.loggingService, this.csharpierPath);
            } else {
                return new CSharpierProcessPipeMultipleFiles(
                    this.loggingService,
                    this.csharpierPath,
                    directory,
                );
            }
        } catch (ex: any) {
            this.loggingService.logError(ex.output.toString());
            return new NullCSharpierProcess();
        }
    };

    private displayInstallNeededMessage = () => {
        this.loggingService.logError("CSharpier not found");

        // TODO deal with this somehow
        // window
        //     .showErrorMessage("CSharpier must be installed globally.", "Install CSharpier")
        //     .then(selection => {
        //         if (selection === "Install CSharpier") {
        //             const command = "dotnet tool install -g csharpier";
        //             this.loggingService.logInfo("Running " + command);
        //             const output = execSync(command).toString();
        //             this.loggingService.logInfo(output);
        //             this.csharpierProcess = this.setupCSharpierProcess();
        //         }
        //     });
    };

    dispose() {
        this.killRunningProcesses();
    }

    killRunningProcesses() {
        for (const key in this.csharpierProcessesByVersion) {
            this.loggingService.logDebug("disposing of process for version " + key);
            this.csharpierProcessesByVersion[key]?.dispose();
        }
        this.warmingByDirectory = {};
        this.csharpierVersionByDirectory = {};
        this.csharpierProcessesByVersion = {};
    }
}

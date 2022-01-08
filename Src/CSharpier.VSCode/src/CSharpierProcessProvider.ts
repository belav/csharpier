import { Disposable, TextEditor, window, workspace } from "vscode";
import { LoggingService } from "./LoggingService";
import * as path from "path";
import * as semver from "semver";
import { execSync } from "child_process";
import { ICSharpierProcess, NullCSharpierProcess } from "./CSharpierProcess";
import { CSharpierProcessSingleFile } from "./CSharpierProcessSingleFile";
import { CSharpierProcessPipeMultipleFiles } from "./CSharpierProcessPipeMultipleFiles";
import * as fs from "fs";
import { InstallerService } from "./InstallerService";

export class CSharpierProcessProvider implements Disposable {
    warnedForOldVersion = false;
    loggingService: LoggingService;
    installerService: InstallerService;
    csharpierPath: string;
    warmingByDirectory: Record<string, boolean | undefined> = {};
    csharpierVersionByDirectory: Record<string, string | undefined> = {};
    csharpierProcessesByVersion: Record<string, ICSharpierProcess | undefined> = {};

    constructor(loggingService: LoggingService) {
        this.loggingService = loggingService;
        this.installerService = new InstallerService(
            this.loggingService,
            this.killRunningProcesses,
        );

        let timeoutHandle: NodeJS.Timeout;
        let setupKillRunningProcesses = () => {
            // TODO we can't detect when the terminal gets focused
            // see https://github.com/microsoft/vscode/issues/117980
            // and we can't detect when the text editor itself loses focus
            // so in order to make sure someone can use the terminal to update csharpier
            // we have to kill off the background process after a set amount of time
            clearTimeout(timeoutHandle);
            timeoutHandle = setTimeout(this.killRunningProcesses, 15000);
        };

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
            this.findAndWarmProcess(event.document.fileName);
            clearTimeout(timeoutHandle);
            setupKillRunningProcesses();
        });

        this.csharpierPath = this.getCSharpierPath();

        this.loggingService.logDebug("Using command dotnet " + this.csharpierPath);
    }

    private findAndWarmProcess(filePath: string) {
        let directory = path.parse(filePath).dir;
        if (this.warmingByDirectory[directory]) {
            return;
        }
        this.loggingService.logDebug("Ensure there is a csharpier process for " + directory);
        this.warmingByDirectory[directory] = true;
        let version = this.csharpierVersionByDirectory[directory];
        if (!version) {
            version = this.getCSharpierVersion(directory);
            if (!semver.valid(version)) {
                this.installerService.displayInstallNeededMessage(directory);
            }
            this.csharpierVersionByDirectory[directory] = version;
        }

        if (!this.csharpierProcessesByVersion[version]) {
            this.csharpierProcessesByVersion[version] = this.setupCSharpierProcess(
                directory,
                version,
            );
        }
        delete this.warmingByDirectory[directory];
    }

    private getCSharpierPath = () => {
        let csharpierPath = "csharpier";

        // let csharpierDebugPath = path.resolve(
        //     __dirname,
        //     "../../CSharpier.Cli/bin/Debug/net6.0/dotnet-csharpier.dll",
        // );
        // let csharpierReleasePath = csharpierDebugPath.replace("Debug", "Release");
        //
        // if (fs.existsSync(csharpierDebugPath)) {
        //     csharpierPath = csharpierDebugPath;
        // } else if (fs.existsSync(csharpierReleasePath)) {
        //     csharpierPath = csharpierReleasePath;
        // }
        return csharpierPath;
    };

    public getProcessFor = (filePath: string) => {
        let directory = path.parse(filePath).dir;
        let version = this.csharpierVersionByDirectory[directory];
        if (!version) {
            this.findAndWarmProcess(filePath);
            version = this.csharpierVersionByDirectory[directory];
        }

        if (!version || !this.csharpierProcessesByVersion[version]) {
            // this shouldn't really happen, but just in case
            return new NullCSharpierProcess();
        }

        return this.csharpierProcessesByVersion[version]!;
    };

    private getCSharpierVersion = (directoryThatContainsFile: string): string => {
        let currentDirectory = directoryThatContainsFile;
        while (true) {
            let dotnetToolsPath = path.join(currentDirectory, ".config/dotnet-tools.json");
            this.loggingService.logDebug(`Looking for ${dotnetToolsPath}`);
            if (fs.existsSync(dotnetToolsPath)) {
                let data = JSON.parse(fs.readFileSync(dotnetToolsPath).toString());
                let version = data.tools.csharpier?.version;
                if (version) {
                    this.loggingService.logDebug(
                        "Found version " + version + " in " + dotnetToolsPath,
                    );
                    return version;
                }
            }

            let nextDirectory = path.join(currentDirectory, "..");
            if (nextDirectory === currentDirectory) {
                break;
            }
            currentDirectory = nextDirectory;
        }

        this.loggingService.logDebug(
            "Unable to find dotnet-tools.json, falling back to running dotnet csharpier --version",
        );

        let outputFromCsharpier: string;

        try {
            outputFromCsharpier = execSync(`dotnet ${this.csharpierPath} --version`, {
                cwd: directoryThatContainsFile,
            }).toString();
        } catch (error: any) {
            this.loggingService.logDebug(
                "dotnet csharpier --version failed with " + error.stderr.toString(),
            );
            return "";
        }

        this.loggingService.logDebug(`dotnet csharpier --version output ${outputFromCsharpier}`);

        let lines = outputFromCsharpier.split(/\r?\n/);

        // sometimes .net outputs more than just the version
        for (let x = lines.length - 1; x >= 0; x--) {
            let version = lines[x].trim();
            if (version !== "") {
                return version;
            }
        }

        this.loggingService.logDebug(
            "Could not find version in output from dotnet csharpier --version in cwd " +
                directoryThatContainsFile +
                ". Output was \n" +
                outputFromCsharpier,
        );

        return "";
    };

    private setupCSharpierProcess = (directory: string, version: string) => {
        try {
            if (!semver.valid(version)) {
                return new NullCSharpierProcess();
            }

            this.loggingService.logDebug(`Adding new version ${version} process for ${directory}`);

            if (semver.lt(version, "0.12.0")) {
                if (!this.warnedForOldVersion) {
                    window.showInformationMessage(
                        "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.",
                    );
                    this.warnedForOldVersion = true;
                }
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

    public dispose = () => {
        this.killRunningProcesses();
    };

    private killRunningProcesses = () => {
        for (let key in this.csharpierProcessesByVersion) {
            this.loggingService.logDebug(
                "disposing of process for version " + (key === "" ? "null" : key),
            );
            this.csharpierProcessesByVersion[key]?.dispose();
        }
        this.warmingByDirectory = {};
        this.csharpierVersionByDirectory = {};
        this.csharpierProcessesByVersion = {};
    };
}

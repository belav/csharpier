import { Disposable, TextEditor, window, workspace } from "vscode";
import { Logger } from "./Logger";
import * as path from "path";
import * as semver from "semver";
import { execSync } from "child_process";
import { ICSharpierProcess, NullCSharpierProcess } from "./CSharpierProcess";
import { CSharpierProcessSingleFile } from "./CSharpierProcessSingleFile";
import { CSharpierProcessPipeMultipleFiles } from "./CSharpierProcessPipeMultipleFiles";
import * as fs from "fs";
import { InstallerService } from "./InstallerService";
import { CustomPathInstaller } from "./CustomPathInstaller";

export class CSharpierProcessProvider implements Disposable {
    warnedForOldVersion = false;
    logger: Logger;
    customPathInstaller: CustomPathInstaller;
    installerService: InstallerService;
    warmingByDirectory: Record<string, boolean | undefined> = {};
    csharpierVersionByDirectory: Record<string, string | undefined> = {};
    csharpierProcessesByVersion: Record<string, ICSharpierProcess | undefined> = {};

    constructor(logger: Logger) {
        this.logger = logger;
        this.customPathInstaller = new CustomPathInstaller(logger);
        this.installerService = new InstallerService(this.logger, this.killRunningProcesses);

        window.onDidChangeActiveTextEditor((event: TextEditor | undefined) => {
            if (event?.document?.languageId !== "csharp") {
                return;
            }
            this.findAndWarmProcess(event.document.fileName);
        });
    }

    private findAndWarmProcess(filePath: string) {
        const directory = path.parse(filePath).dir;
        if (this.warmingByDirectory[directory]) {
            return;
        }
        this.logger.debug("Ensure there is a csharpier process for " + directory);
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

    public getProcessFor = (filePath: string) => {
        const directory = path.parse(filePath).dir;
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
            const dotnetToolsPath = path.join(currentDirectory, ".config/dotnet-tools.json");
            this.logger.debug(`Looking for ${dotnetToolsPath}`);
            if (fs.existsSync(dotnetToolsPath)) {
                const data = JSON.parse(fs.readFileSync(dotnetToolsPath).toString());
                const version = data.tools.csharpier?.version;
                if (version) {
                    this.logger.debug("Found version " + version + " in " + dotnetToolsPath);
                    return version;
                }
            }

            const nextDirectory = path.join(currentDirectory, "..");
            if (nextDirectory === currentDirectory) {
                break;
            }
            currentDirectory = nextDirectory;
        }

        this.logger.debug(
            "Unable to find dotnet-tools.json, falling back to running dotnet csharpier --version",
        );

        let outputFromCsharpier: string;

        try {
            outputFromCsharpier = execSync(`dotnet csharpier --version`, {
                cwd: directoryThatContainsFile,
                env: { ...process.env, DOTNET_NOLOGO: "1" },
            }).toString();
        } catch (error: any) {
            this.logger.debug("dotnet csharpier --version failed with " + error.stderr.toString());
            return "";
        }

        this.logger.debug(`dotnet csharpier --version output ${outputFromCsharpier}`);

        const lines = outputFromCsharpier.split(/\r?\n/);

        // sometimes .net outputs more than just the version
        for (let x = lines.length - 1; x >= 0; x--) {
            const version = lines[x].trim();
            if (version !== "") {
                return version;
            }
        }

        this.logger.debug(
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

            this.customPathInstaller.ensureVersionInstalled(version);
            const customPath = this.customPathInstaller.getPathForVersion(version);

            this.logger.debug(`Adding new version ${version} process for ${directory}`);

            if (semver.lt(version, "0.12.0")) {
                if (!this.warnedForOldVersion) {
                    window.showInformationMessage(
                        "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.",
                    );
                    this.warnedForOldVersion = true;
                }
                return new CSharpierProcessSingleFile(this.logger, customPath);
            } else {
                return new CSharpierProcessPipeMultipleFiles(this.logger, customPath, directory);
            }
        } catch (ex: any) {
            this.logger.error(ex.output.toString());
            return new NullCSharpierProcess();
        }
    };

    public dispose = () => {
        this.killRunningProcesses();
    };

    private killRunningProcesses = () => {
        for (const key in this.csharpierProcessesByVersion) {
            this.logger.debug("disposing of process for version " + (key === "" ? "null" : key));
            this.csharpierProcessesByVersion[key]?.dispose();
        }
        this.warmingByDirectory = {};
        this.csharpierVersionByDirectory = {};
        this.csharpierProcessesByVersion = {};
    };
}

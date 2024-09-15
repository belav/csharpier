import { Disposable, Extension, TextEditor, window, workspace } from "vscode";
import { Logger } from "./Logger";
import * as path from "path";
import * as semver from "semver";
import * as convert from "xml-js";
import { ICSharpierProcess } from "./ICSharpierProcess";
import { CSharpierProcessSingleFile } from "./CSharpierProcessSingleFile";
import { CSharpierProcessPipeMultipleFiles } from "./CSharpierProcessPipeMultipleFiles";
import * as fs from "fs";
import { InstallerService } from "./InstallerService";
import { CustomPathInstaller } from "./CustomPathInstaller";
import { execDotNet } from "./DotNetProvider";
import { NullCSharpierProcess } from "./NullCSharpierProcess";
import { CSharpierProcessServer } from "./CSharpierProcessServer";
import { ICSharpierProcess2 } from "./ICSharpierProcess";
import { runFunctionsUntilResultFound } from "./RunFunctionsUntilResultFound";

export class CSharpierProcessProvider implements Disposable {
    warnedForOldVersion = false;
    logger: Logger;
    customPathInstaller: CustomPathInstaller;
    installerService: InstallerService;
    warmingByDirectory: Record<string, boolean | undefined> = {};
    csharpierVersionByDirectory: Record<string, string | undefined> = {};
    csharpierProcessesByVersion: Record<
        string,
        ICSharpierProcess | ICSharpierProcess2 | undefined
    > = {};
    disableCSharpierServer: boolean;

    constructor(logger: Logger, extension: Extension<unknown>) {
        this.logger = logger;
        this.customPathInstaller = new CustomPathInstaller(logger);
        this.installerService = new InstallerService(
            this.logger,
            this.killRunningProcesses,
            extension,
        );

        this.disableCSharpierServer =
            workspace.getConfiguration("csharpier").get<boolean>("dev.disableCSharpierServer") ??
            false;

        window.onDidChangeActiveTextEditor((event: TextEditor | undefined) => {
            if (event?.document?.languageId !== "csharp") {
                return;
            }
            this.findAndWarmProcess(event.document.fileName);
        });
    }

    private findAndWarmProcess(filePath: string) {
        const directory = this.getDirectoryOfFile(filePath);
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

    public getProcessFor = (filePath: string): ICSharpierProcess | ICSharpierProcess2 => {
        const directory = this.getDirectoryOfFile(filePath);
        let version = this.csharpierVersionByDirectory[directory];
        if (!version) {
            this.findAndWarmProcess(filePath);
            version = this.csharpierVersionByDirectory[directory];
        }

        if (!version || !this.csharpierProcessesByVersion[version]) {
            // this shouldn't really happen, but just in case
            this.logger.debug(
                `returning NullCSharpierProcess because there was no csharpierProcessesByVersion for ${version}`,
            );
            return NullCSharpierProcess.instance;
        }

        return this.csharpierProcessesByVersion[version]!;
    };

    private getDirectoryOfFile = (filePath: string) => {
        this.logger.debug(__dirname);
        let directory = path.parse(filePath).dir;
        if (directory) {
            return directory;
        }

        if (workspace.workspaceFolders && workspace.workspaceFolders.length > 0) {
            directory = workspace.workspaceFolders[0].uri.fsPath;
            this.logger.debug(
                "Unsaved document has no directory, falling back to workspace folder: " + directory,
            );
        } else {
            directory = __dirname;
            this.logger.debug(
                "Unsaved document has no directory, falling back to __dirname: " + directory,
            );
        }

        return directory;
    };

    private getCSharpierVersion = (directoryThatContainsFile: string): string => {
        const csharpierVersion = runFunctionsUntilResultFound(
            () => this.findVersionInCsProjOfParentsDirectories(directoryThatContainsFile),
            () => this.findCSharpierVersionInToolOutput(directoryThatContainsFile, false),
            () => this.findCSharpierVersionInToolOutput(directoryThatContainsFile, true),
        );

        if (!csharpierVersion) {
            return "";
        }

        const versionWithoutHash = csharpierVersion.split("+")[0];
        this.logger.debug(`Using ${versionWithoutHash} as the version number.`);
        return versionWithoutHash;
    };

    private findCSharpierVersionInToolOutput = (
        directoryThatContainsFile: string,
        isGlobal: boolean,
    ) => {
        const command = `tool list${isGlobal ? " -g" : ""}`;
        const output = execDotNet(command, directoryThatContainsFile).toString().trim();

        this.logger.debug(`Running 'dotnet ${command}' to look for version`);
        this.logger.debug(`Output was: \n${output}`);

        const lines = output
            .split("\n")
            .map(line => line.trim())
            .filter(line => line.length > 0);

        // The first two lines are headers
        for (let i = 2; i < lines.length; i++) {
            const columns = lines[i].split(/\s{2,}/);
            if (columns.length >= 2) {
                if (columns[0].toLowerCase() === "csharpier") {
                    return columns[1];
                }
            }
        }
    };

    private findVersionInCsProjOfParentsDirectories = (directoryThatContainsFile: string) => {
        this.logger.debug(
            `Looking for csproj in or above ${directoryThatContainsFile} that references CSharpier.MsBuild`,
        );
        let currentDirectory = directoryThatContainsFile;
        let parentNumber = 0;
        while (parentNumber < 30) {
            const csProjVersion = this.findVersionInCsProj(currentDirectory);
            if (csProjVersion) {
                return csProjVersion;
            }

            const nextDirectory = path.join(currentDirectory, "..");
            if (nextDirectory === currentDirectory) {
                break;
            }
            currentDirectory = nextDirectory;
            parentNumber++;
        }
    };

    private findVersionInCsProj = (currentDirectory: string) => {
        this.logger.debug(`Looking for ${currentDirectory}/*.csproj`);
        const csProjFileNames = fs
            .readdirSync(currentDirectory)
            .filter(o => o.toLowerCase().endsWith(".csproj"));
        for (const csProjFileName of csProjFileNames) {
            const csProjPath = path.join(currentDirectory, csProjFileName);
            try {
                this.logger.debug(`Looking at ${csProjPath}`);
                const uglyProject = JSON.parse(
                    convert.xml2json(fs.readFileSync(csProjPath).toString()),
                );
                for (const project of uglyProject.elements) {
                    if (project.name !== "Project") {
                        continue;
                    }
                    for (const itemGroup of project.elements) {
                        if (itemGroup.name !== "ItemGroup") {
                            continue;
                        }

                        for (const packageReference of itemGroup.elements) {
                            if (packageReference.name !== "PackageReference") {
                                continue;
                            }

                            if (packageReference.attributes["Include"] !== "CSharpier.MsBuild") {
                                continue;
                            }

                            const version = packageReference.attributes["Version"];
                            if (version) {
                                this.logger.debug(`Found version ${version} in ${csProjPath}`);
                                return version;
                            }
                        }
                    }
                }
            } catch (error) {
                this.logger.error("Failed while trying to read " + csProjPath);
                this.logger.error(error);
            }
        }
    };

    private setupCSharpierProcess = (directory: string, version: string) => {
        try {
            if (!semver.valid(version)) {
                this.logger.debug(
                    `returning NullCSharpierProcess because version is not a valid version number.`,
                );
                return NullCSharpierProcess.instance;
            }

            if (!this.customPathInstaller.ensureVersionInstalled(version)) {
                this.logger.debug(`Unable to validate install of version ${version}`);
                this.displayFailureMessage();
                return NullCSharpierProcess.instance;
            }

            const customPath = this.customPathInstaller.getPathForVersion(version);

            this.logger.debug(`Adding new version ${version} process for ${directory}`);

            let csharpierProcess: ICSharpierProcess;

            const serverVersion = "0.29.0";

            if (semver.gte(version, serverVersion) && !this.disableCSharpierServer) {
                csharpierProcess = new CSharpierProcessServer(
                    this.logger,
                    customPath,
                    directory,
                    version,
                );
            } else if (semver.gte(version, "0.12.0")) {
                if (semver.gte(version, serverVersion) && this.disableCSharpierServer) {
                    this.logger.debug(
                        "CSharpier server is disabled, falling back to piping via stdin",
                    );
                }

                csharpierProcess = new CSharpierProcessPipeMultipleFiles(
                    this.logger,
                    customPath,
                    directory,
                    version,
                );
            } else {
                if (!this.warnedForOldVersion) {
                    window.showInformationMessage(
                        "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.",
                    );
                    this.warnedForOldVersion = true;
                }
                csharpierProcess = new CSharpierProcessSingleFile(this.logger, customPath, version);
            }

            if (csharpierProcess.getProcessFailedToStart()) {
                this.displayFailureMessage();
            }

            return csharpierProcess;
        } catch (ex: any) {
            this.logger.error(ex.output.toString());
            this.logger.debug(
                `returning NullCSharpierProcess because of the previous error when trying to set up a csharpier process`,
            );
            return NullCSharpierProcess.instance;
        }
    };

    public dispose = () => {
        this.killRunningProcesses();
    };

    private killRunningProcesses = () => {
        for (const key in this.csharpierProcessesByVersion) {
            this.logger.debug("disposing of process for version " + (key || "null"));
            this.csharpierProcessesByVersion[key]?.dispose();
        }
        this.warmingByDirectory = {};
        this.csharpierVersionByDirectory = {};
        this.csharpierProcessesByVersion = {};
    };

    private displayFailureMessage() {
        window.showErrorMessage(
            "CSharpier could not be set up properly so formatting is not currently supported. See Output - CSharpier for details.",
        );
    }
}

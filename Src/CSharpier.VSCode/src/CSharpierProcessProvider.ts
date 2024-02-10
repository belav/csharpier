import { Disposable, Extension, TextEditor, window, workspace } from "vscode";
import { Logger } from "./Logger";
import * as path from "path";
import * as semver from "semver";
import * as convert from "xml-js";
import { ICSharpierProcess, NullCSharpierProcess } from "./CSharpierProcess";
import { CSharpierProcessSingleFile } from "./CSharpierProcessSingleFile";
import { CSharpierProcessPipeMultipleFiles } from "./CSharpierProcessPipeMultipleFiles";
import * as fs from "fs";
import { InstallerService } from "./InstallerService";
import { CustomPathInstaller } from "./CustomPathInstaller";
import { ExecDotNet } from "./DotNetProvider";

export class CSharpierProcessProvider implements Disposable {
    warnedForOldVersion = false;
    logger: Logger;
    customPathInstaller: CustomPathInstaller;
    installerService: InstallerService;
    warmingByDirectory: Record<string, boolean | undefined> = {};
    csharpierVersionByDirectory: Record<string, string | undefined> = {};
    csharpierProcessesByVersion: Record<string, ICSharpierProcess | undefined> = {};
    execDotNet: ExecDotNet;

    constructor(logger: Logger, extension: Extension<unknown>, execDotNet: ExecDotNet) {
        this.logger = logger;
        this.execDotNet = execDotNet;
        this.customPathInstaller = new CustomPathInstaller(logger, execDotNet);
        this.installerService = new InstallerService(
            this.logger,
            this.killRunningProcesses,
            extension,
            execDotNet,
        );

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

    public getProcessFor = (filePath: string) => {
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
        let currentDirectory = directoryThatContainsFile;
        let parentNumber = 0;
        while (parentNumber < 30) {
            const csProjVersion = this.findVersionInCsProj(currentDirectory);
            if (csProjVersion) {
                return csProjVersion;
            }

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
            parentNumber++;
        }

        this.logger.debug(
            "Unable to find dotnet-tools.json, falling back to running dotnet csharpier --version",
        );

        let outputFromCsharpier: string;

        try {
            outputFromCsharpier = this.execDotNet(`csharpier --version`, {
                cwd: directoryThatContainsFile,
                env: { ...process.env, DOTNET_NOLOGO: "1" },
            })
                .toString()
                .trim();

            this.logger.debug(`dotnet csharpier --version output: ${outputFromCsharpier}`);
            const versionWithoutHash = outputFromCsharpier.split("+")[0];
            this.logger.debug(`Using ${versionWithoutHash} as the version number.`);
            return versionWithoutHash;
        } catch (error: any) {
            const message = !error.stderr ? error.toString() : error.stderr.toString();

            this.logger.debug("dotnet csharpier --version failed with " + message);
            return "";
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

            if (semver.lt(version, "0.12.0")) {
                if (!this.warnedForOldVersion) {
                    window.showInformationMessage(
                        "Please upgrade to CSharpier >= 0.12.0 for bug fixes and improved formatting speed.",
                    );
                    this.warnedForOldVersion = true;
                }
                return new CSharpierProcessSingleFile(this.logger, customPath);
            } else {
                const csharpierProcess = new CSharpierProcessPipeMultipleFiles(
                    this.logger,
                    customPath,
                    directory,
                );
                if (csharpierProcess.processFailedToStart) {
                    this.displayFailureMessage();
                }

                return csharpierProcess;
            }
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

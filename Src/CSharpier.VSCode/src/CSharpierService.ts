import { Disposable, env, Uri, window } from "vscode";
import { LoggingService } from "./LoggingService";
import * as path from "path";
import * as fs from "fs";
import * as semver from "semver";
import { execSync } from "child_process";
import {
    LongRunningCSharpierProcess,
    ICSharpierProcess,
    NullCSharpierProcess,
    ObsoleteCSharpierProcess,
} from "./CSharpierProcess";

export class CSharpierService implements Disposable {
    loggingService: LoggingService;
    csharpierPath: string;
    callbacks: ((result: string) => void)[] = [];
    csharpierProcess: ICSharpierProcess;

    constructor(loggingService: LoggingService) {
        this.loggingService = loggingService;

        this.csharpierPath = this.getCSharpierPath();

        this.loggingService.logDebug("Using command dotnet " + this.csharpierPath);

        this.csharpierProcess = this.setupCSharpierProcess();
    }

    private getCSharpierPath = () => {
        let csharpierPath = "csharpier";

        const csharpierDebugPath = path.resolve(
            __dirname,
            "../../CSharpier.Cli/bin/Debug/net6.0/dotnet-csharpier.dll",
        );
        const csharpierReleasePath = csharpierDebugPath.replace("Debug", "Release");

        if (fs.existsSync(csharpierDebugPath)) {
            csharpierPath = csharpierDebugPath;
        } else if (fs.existsSync(csharpierReleasePath)) {
            csharpierPath = csharpierReleasePath;
        }
        return csharpierPath;
    };

    public formatInPlace = (content: string, fileName: string) => {
        return this.csharpierProcess.formatFile(content, fileName);
    };

    private setupCSharpierProcess = () => {
        try {
            const version = execSync("dotnet " + this.csharpierPath + " --version")
                .toString()
                .trim();
            this.loggingService.logInfo("Version: " + version);
            if (!semver.valid(version)) {
                this.displayInstallNeededMessage();
                return new NullCSharpierProcess();
            }

            if (semver.lt(version, "0.12.0")) {
                window.showInformationMessage(
                    "Please upgrade to CSharpier >= 0.12.0 for improved formatting speed.",
                );
                return new ObsoleteCSharpierProcess(this.loggingService, this.csharpierPath);
            } else {
                return new LongRunningCSharpierProcess(this.loggingService, this.csharpierPath);
            }
        } catch (ex: any) {
            this.loggingService.logDebug(ex.output.toString());
            this.displayInstallNeededMessage();
            return new NullCSharpierProcess();
        }
    };

    private displayInstallNeededMessage = () => {
        this.loggingService.logError("CSharpier not found");

        window
            .showErrorMessage("CSharpier must be installed globally.", "Install CSharpier")
            .then(selection => {
                if (selection === "Install CSharpier") {
                    const command = "dotnet tool install -g csharpier";
                    this.loggingService.logInfo("Running " + command);
                    const output = execSync(command).toString();
                    this.loggingService.logInfo(output);
                    this.csharpierProcess = this.setupCSharpierProcess();
                }
            });
    };

    dispose() {
        this.csharpierProcess.dispose();
    }
}

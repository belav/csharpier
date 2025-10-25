import { Logger } from "./Logger";
import * as path from "path";
import * as fs from "fs";
import { workspace } from "vscode";
import { getDotNetRoot, execDotNet } from "./DotNetProvider";
import { execSync } from "child_process";
import * as semver from "semver";

export class CustomPathInstaller {
    private readonly logger: Logger;
    private readonly customPath: string = "";

    constructor(logger: Logger) {
        this.logger = logger;
        if (workspace.getConfiguration("csharpier").get<boolean>("dev.useCustomPath")) {
            this.customPath =
                workspace.getConfiguration("csharpier").get<string>("dev.customPath") ?? "";
        }
    }

    public ensureVersionInstalled(version: string, directory: string): boolean {
        if (!version) {
            return false;
        }
        if (this.customPath !== "") {
            this.logger.debug("Using csharpier at a custom path of " + this.customPath);
            return true;
        }

        let pathToDirectoryForVersion = this.getDirectoryForVersion(version);
        if (fs.existsSync(pathToDirectoryForVersion)) {
            this.logger.debug("Validating install at " + pathToDirectoryForVersion);
            if (this.validateInstall(pathToDirectoryForVersion, version)) {
                return true;
            }

            this.logger.debug(
                `Removing directory at ${pathToDirectoryForVersion} because it appears to be corrupted`,
            );
            fs.rmSync(pathToDirectoryForVersion, { recursive: true, force: true });
        }

        let command = `dotnet tool install csharpier --version ${version} --tool-path "${pathToDirectoryForVersion}"`;
        this.logger.debug("Running " + command);
        execDotNet(command, directory);

        return this.validateInstall(pathToDirectoryForVersion, version);
    }

    private validateInstall(pathToDirectoryForVersion: string, version: string): boolean {
        let pathForVersion = this.getPathForVersion(version);
        try {
            let output = execSync(`"${pathForVersion}" --version`, {
                env: { ...process.env, DOTNET_ROOT: getDotNetRoot() },
            })
                .toString()
                .trim();

            this.logger.debug(`"${pathForVersion}" --version output: ${output}`);
            let versionWithoutHash = output.split("+")[0];
            this.logger.debug(`Using ${versionWithoutHash} as the version number.`);

            if (versionWithoutHash === version) {
                this.logger.debug("CSharpier at " + pathToDirectoryForVersion + " already exists");
                return true;
            } else {
                this.logger.warn(
                    "Version of " +
                        versionWithoutHash +
                        " did not match expected version of " +
                        version,
                );
            }
        } catch (error: any) {
            let message = !error.stderr ? error.toString() : error.stderr.toString();
            this.logger.warn(`Exception while running '${pathForVersion} --version'`, message);
        }

        return false;
    }

    private getDirectoryForVersion(version: string) {
        if (this.customPath !== "") {
            return this.customPath;
        }

        let result =
            process.platform !== "win32"
                ? path.resolve(process.env.HOME!, ".cache/csharpier", version)
                : path.resolve(process.env.LOCALAPPDATA!, "CSharpier", version);
        return result.toString();
    }

    public getPathForVersion(version: string) {
        let newCommandsVersion = "1.0.0-alpha1";
        let filename = semver.gte(version, newCommandsVersion) ? "csharpier" : "dotnet-csharpier";
        return path.resolve(this.getDirectoryForVersion(version), filename);
    }
}

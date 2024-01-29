import { Logger } from "./Logger";
import * as path from "path";
import * as fs from "fs";
import { workspace } from "vscode";
import { ExecDotNet } from "./DotNetProvider";

export class CustomPathInstaller {
    logger: Logger;
    customPath: string;
    execDotNet: ExecDotNet;

    constructor(logger: Logger, execDotNet: ExecDotNet) {
        this.logger = logger;
        this.execDotNet = execDotNet;
        this.customPath =
            workspace.getConfiguration("csharpier").get<string>("dev.customPath") ?? "";
    }

    public ensureVersionInstalled(version: string): boolean {
        if (!version) {
            return false;
        }
        if (this.customPath !== "") {
            this.logger.debug("Using csharpier at a custom path of " + this.customPath);
            return true;
        }

        const pathToDirectoryForVersion = this.getDirectoryForVersion(version);
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

        const command = `dotnet tool install csharpier --version ${version} --tool-path "${pathToDirectoryForVersion}"`;
        this.logger.debug("Running " + command);
        this.execDotNet(command);

        return this.validateInstall(pathToDirectoryForVersion, version);
    }

    private validateInstall(pathToDirectoryForVersion: string, version: string): boolean {
        try {
            // TODO this fails if we use dotnetPath, we really need to set it as DOTNET_ROOT
            const output = this.execDotNet(`"${this.getPathForVersion(version)}" --version`, {
                env: { ...process.env, DOTNET_NOLOGO: "1" },
            })
                .toString()
                .trim();

            this.logger.debug(`"${this.getPathForVersion(version)}" --version output: ${output}`);
            const versionWithoutHash = output.split("+")[0];
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
            const message = !error.stderr ? error.toString() : error.stderr.toString();
            this.logger.warn(
                "Exception while running 'dotnet-csharpier --version' in " +
                    pathToDirectoryForVersion,
                message,
            );
        }

        return false;
    }

    private getDirectoryForVersion(version: string) {
        if (this.customPath !== "") {
            return this.customPath;
        }

        const result =
            process.platform !== "win32"
                ? path.resolve(process.env.HOME!, ".cache/csharpier", version)
                : path.resolve(process.env.LOCALAPPDATA!, "CSharpier", version);
        return result.toString();
    }

    public getPathForVersion(version: string) {
        return path.resolve(this.getDirectoryForVersion(version), "dotnet-csharpier");
    }
}

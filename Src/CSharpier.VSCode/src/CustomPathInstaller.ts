import { Logger } from "./Logger";
import * as path from "path";
import * as fs from "fs";
import { execSync } from "child_process";

export class CustomPathInstaller {
    logger: Logger;

    constructor(logger: Logger) {
        this.logger = logger;
    }

    public ensureVersionInstalled(version: string) {
        if (!version) {
            return;
        }
        const pathToDirectoryForVersion = this.getDirectoryForVersion(version);
        if (fs.existsSync(pathToDirectoryForVersion)) {
            try {
                const output = execSync(`${this.getPathForVersion(version)} --version`, {
                    env: { ...process.env, DOTNET_NOLOGO: "1" },
                }).toString();

                this.logger.debug("dotnet csharpier --version output: " + output);

                if (output === version) {
                    this.logger.debug(
                        "CSharpier at " + pathToDirectoryForVersion + " already exists",
                    );
                    return;
                }
            } catch (error: any) {
                const message = !error.stderr ? error.toString() : error.stderr.toString();
                this.logger.warn(
                    "Exception while running 'dotnet csharpier --version' in " +
                        pathToDirectoryForVersion,
                    message,
                );
            }

            // if we got here something isn't right in the current directory
            fs.rmdirSync(pathToDirectoryForVersion, { recursive: true });
        }

        const command = `dotnet tool install csharpier --version ${version} --tool-path "${pathToDirectoryForVersion}"`;
        execSync(command);
    }

    private getDirectoryForVersion(version: string) {
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

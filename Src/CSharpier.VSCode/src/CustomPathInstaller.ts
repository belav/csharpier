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
        const directoryForVersion = this.getDirectoryForVersion(version);
        if (fs.existsSync(directoryForVersion)) {
            this.logger.debug(`File at ${directoryForVersion} already exists`);
            return;
        }

        const command = `dotnet tool install csharpier --version ${version} --tool-path ${directoryForVersion}`;
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

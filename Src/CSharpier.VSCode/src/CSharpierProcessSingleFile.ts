import { Logger } from "./Logger";
import { spawn } from "child_process";
import { ICSharpierProcess } from "./ICSharpierProcess";
import * as path from "path";
import { getDotNetRoot } from "./DotNetProvider";
import * as process from "process";

export class CSharpierProcessSingleFile implements ICSharpierProcess {
    private readonly csharpierPath: string;
    private logger: Logger;
    private version: string;

    constructor(logger: Logger, csharpierPath: string, version: string) {
        this.logger = logger;
        this.csharpierPath = csharpierPath;
        this.version = version;
    }

    getProcessFailedToStart(): boolean {
        return false;
    }

    formatFile(content: string, filePath: string): Promise<string> {
        let directory = path.parse(filePath).dir;
        return new Promise((resolve, reject) => {
            let csharpier = spawn(this.csharpierPath, ["--write-stdout"], {
                stdio: "pipe",
                cwd: directory,
                env: { ...process.env, DOTNET_NOLOGO: "1", DOTNET_ROOT: getDotNetRoot() },
            });

            let output = "";
            csharpier.stdout.on("data", chunk => {
                output += chunk.toString();
            });
            csharpier.on("exit", () => {
                if (output.indexOf("Failed to compile so was not formatted") > 0) {
                    resolve("");
                } else {
                    resolve(output);
                }
            });

            csharpier.stdin.write(content);
            csharpier.stdin.end();
        });
    }

    dispose() {}

    getVersion(): string {
        return this.version;
    }
}

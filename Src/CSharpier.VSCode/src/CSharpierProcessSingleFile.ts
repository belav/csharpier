import { Logger } from "./Logger";
import { spawn } from "child_process";
import { ICSharpierProcess } from "./CSharpierProcess";
import * as path from "path";

export class CSharpierProcessSingleFile implements ICSharpierProcess {
    private readonly csharpierPath: string;
    private logger: Logger;

    constructor(loggingService: Logger, csharpierPath: string) {
        this.logger = loggingService;
        this.csharpierPath = csharpierPath;
    }

    formatFile(content: string, filePath: string): Promise<string> {
        const directory = path.parse(filePath).dir;
        return new Promise((resolve, reject) => {
            const csharpier = spawn("dotnet", [this.csharpierPath, "--write-stdout"], {
                stdio: "pipe",
                cwd: directory,
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
}

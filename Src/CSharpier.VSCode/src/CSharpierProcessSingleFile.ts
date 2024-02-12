import { Logger } from "./Logger";
import { spawn } from "child_process";
import { ICSharpierProcess } from "./CSharpierProcess";
import * as path from "path";
import { getDotNetRoot } from "./DotNetProvider";

export class CSharpierProcessSingleFile implements ICSharpierProcess {
    private readonly csharpierPath: string;
    private logger: Logger;

    constructor(logger: Logger, csharpierPath: string) {
        this.logger = logger;
        this.csharpierPath = csharpierPath;
    }

    formatFile(content: string, filePath: string): Promise<string> {
        const directory = path.parse(filePath).dir;
        return new Promise((resolve, reject) => {
            const csharpier = spawn(this.csharpierPath, ["--write-stdout"], {
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
}

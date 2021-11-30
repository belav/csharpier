import { LoggingService } from "./LoggingService";
import { spawn } from "child_process";
import { ICSharpierProcess } from "./CSharpierProcess";

export class CSharpierProcessSingleFile implements ICSharpierProcess {
    private readonly csharpierPath: string;
    private loggingService: LoggingService;

    constructor(loggingService: LoggingService, csharpierPath: string) {
        this.loggingService = loggingService;
        this.csharpierPath = csharpierPath;
    }

    formatFile(content: string, fileName: string): Promise<string> {
        return new Promise((resolve, reject) => {
            const csharpier = spawn("dotnet", [this.csharpierPath, "--write-stdout"], {
                stdio: "pipe",
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

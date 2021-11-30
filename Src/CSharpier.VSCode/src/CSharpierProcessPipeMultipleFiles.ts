import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import { LoggingService } from "./LoggingService";
import { ICSharpierProcess } from "./CSharpierProcess";

export class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess {
    private process: ChildProcessWithoutNullStreams;
    private callbacks: ((result: string) => void)[] = [];
    private loggingService: LoggingService;

    constructor(loggingService: LoggingService, csharpierPath: string) {
        this.loggingService = loggingService;
        this.process = this.spawnProcess(csharpierPath);

        this.loggingService.logInfo("Warm CSharpier with initial format");
        this.formatFile("public class ClassName { }", "Test.cs");
    }

    private spawnProcess = (csharpierPath: string) => {
        const csharpierProcess = spawn("dotnet", [csharpierPath, "--pipe-multiple-files"], {
            stdio: "pipe",
        });

        csharpierProcess.stderr.on("data", chunk => {
            this.loggingService.logInfo("Got error: " + chunk.toString());
            const callback = this.callbacks.shift();
            if (callback) {
                callback("");
            }
        });

        csharpierProcess.stdout.on("data", chunk => {
            this.loggingService.logInfo("Got chunk");
            const callback = this.callbacks.shift();
            if (callback) {
                let content: string = chunk.toString();
                if (content.endsWith("\u0003")) {
                    content = content.substring(0, content.length - 1);
                }
                callback(content);
            }
        });

        return csharpierProcess;
    };

    formatFile(content: string, fileName: string): Promise<string> {
        this.process.stdin.write(fileName);
        this.process.stdin.write("\u0003");
        this.process.stdin.write(content);
        this.process.stdin.write("\u0003");
        return new Promise<string>(resolve => {
            this.callbacks.push(resolve);
        });
    }

    dispose() {
        (this.process.stdin as any).pause();
        this.process.kill();
    }
}

import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import { Logger } from "./Logger";
import { ICSharpierProcess } from "./CSharpierProcess";

export class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess {
    private process: ChildProcessWithoutNullStreams;
    private callbacks: ((result: string) => void)[] = [];
    private logger: Logger;
    private nextFile: string = "";

    constructor(logger: Logger, csharpierPath: string, workingDirectory: string) {
        this.logger = logger;
        this.process = this.spawnProcess(csharpierPath, workingDirectory);

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.formatFile("public class ClassName { }", "Test.cs").then(() => {
            this.formatFile("public class ClassName { }", "Test.cs");
        });
    }

    private spawnProcess = (csharpierPath: string, workingDirectory: string) => {
        const csharpierProcess = spawn(csharpierPath, ["--pipe-multiple-files"], {
            stdio: "pipe",
            cwd: workingDirectory,
            env: { ...process.env, DOTNET_NOLOGO: "1" },
        });

        csharpierProcess.stdout.on("data", chunk => {
            this.logger.debug("Got chunk of size " + chunk.length);
            this.nextFile += chunk;
            const number = this.nextFile.indexOf("\u0003");
            if (number >= 0) {
                this.logger.debug("Got last chunk with ETX at " + number);
                const result = this.nextFile.substring(0, number);
                this.nextFile = this.nextFile.substring(number + 1);
                const callback = this.callbacks.shift();
                if (callback) {
                    if (!result) {
                        this.logger.info("File is ignored by .csharpierignore or there was an error");
                    }

                    callback(result);
                }
            }
        });

        return csharpierProcess;
    };

    formatFile(content: string, filePath: string): Promise<string> {
        this.process.stdin.write(filePath);
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

import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import { Logger } from "./Logger";
import { ICSharpierProcess } from "./ICSharpierProcess";
import { getDotNetRoot } from "./DotNetProvider";
import * as process from "process";
import * as semver from "semver";

export class CSharpierProcessPipeMultipleFiles implements ICSharpierProcess {
    private process: ChildProcessWithoutNullStreams;
    private callbacks: ((result: string) => void)[] = [];
    private logger: Logger;
    private nextFile: string = "";
    private processFailedToStart = false;
    private version: string;

    constructor(logger: Logger, csharpierPath: string, workingDirectory: string, version: string) {
        this.logger = logger;
        this.version = version;
        this.process = this.spawnProcess(csharpierPath, workingDirectory);

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        this.formatFile("public class ClassName { }", "Test.cs").then(() => {
            this.formatFile("public class ClassName { }", "Test.cs");
        });
    }

    getProcessFailedToStart(): boolean {
        return this.processFailedToStart;
    }

    private spawnProcess = (csharpierPath: string, workingDirectory: string) => {
        let newCommandsVersion = "1.0.0-alpha1";
        let argument = semver.gte(this.version, newCommandsVersion)
            ? "pipe-files"
            : "--pipe-multiple-files";

        let csharpierProcess = spawn(csharpierPath, [argument], {
            stdio: "pipe",
            cwd: workingDirectory,
            env: { ...process.env, DOTNET_NOLOGO: "1", DOTNET_ROOT: getDotNetRoot() },
        });

        csharpierProcess.on("error", data => {
            this.logger.warn(
                "Failed to spawn the needed csharpier process. Formatting cannot occur.",
                data,
            );
            this.processFailedToStart = true;
            while (this.callbacks.length > 0) {
                let callback = this.callbacks.shift();
                if (callback) {
                    callback("");
                }
            }
        });

        csharpierProcess.stderr.on("data", chunk => {
            this.logger.warn(
                "Received data on stderr from the running charpier process",
                chunk.toString(),
            );
        });

        csharpierProcess.stdout.on("data", chunk => {
            this.nextFile += chunk;
            let number = this.nextFile.indexOf("\u0003");
            while (number >= 0) {
                let result = this.nextFile.substring(0, number);
                this.nextFile = this.nextFile.substring(number + 1);
                let callback = this.callbacks.shift();
                if (callback) {
                    if (!result) {
                        this.logger.info(
                            "File is ignored by .csharpierignore or there was an error",
                        );
                    }

                    callback(result);
                }

                number = this.nextFile.indexOf("\u0003");
            }
        });

        return csharpierProcess;
    };

    formatFile(content: string, filePath: string): Promise<string> {
        if (this.processFailedToStart) {
            this.logger.warn("CSharpier proccess failed to start. Formatting cannot occur.");
            return new Promise<string>(resolve => {
                resolve("");
            });
        }

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

    getVersion(): string {
        return this.version;
    }
}

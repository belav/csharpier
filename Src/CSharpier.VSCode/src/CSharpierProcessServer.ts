import { ChildProcessWithoutNullStreams, spawn } from "child_process";
import { Logger } from "./Logger";
import { FormatFileParameter, FormatFileResult, ICSharpierProcess2 } from "./ICSharpierProcess";
import fetch from "node-fetch";
import { getDotNetRoot } from "./DotNetProvider";

export class CSharpierProcessServer implements ICSharpierProcess2 {
    private csharpierPath: string;
    private logger: Logger;
    private port: number = 0;
    private process: ChildProcessWithoutNullStreams | undefined;
    private processFailedToStart = false;
    private version: string;

    constructor(logger: Logger, csharpierPath: string, workingDirectory: string, version: string) {
        this.logger = logger;
        this.csharpierPath = csharpierPath;
        this.spawnProcess(csharpierPath, workingDirectory);
        this.version = version;

        this.logger.debug("Warm CSharpier with initial format");
        // warm by formatting a file twice, the 3rd time is when it gets really fast
        // make sure we give a path that should not exist to avoid any errors when trying to find config files etc.
        this.formatFile("public class ClassName { }", `/${Date.now()}/Test.cs`).then(() => {
            this.formatFile("public class ClassName { }", `/${Date.now()}/Test.cs`);
        });
    }

    getProcessFailedToStart(): boolean {
        return this.processFailedToStart;
    }

    private spawnProcess(csharpierPath: string, workingDirectory: string) {
        const csharpierProcess = spawn(csharpierPath, ["--server"], {
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
        });

        csharpierProcess.on("exit", () => {
            if (csharpierProcess.exitCode !== null && csharpierProcess.exitCode > 0) {
                this.processFailedToStart = true;
            }
        });

        csharpierProcess.stderr.on("data", data => {
            this.logger.warn("CSharpier process return the following error: ", data.toString());
        });

        let output = "";
        const regex = /^Started on (\d+)/;

        csharpierProcess.stdout.on("data", chunk => {
            output += chunk;
            if (regex.test(output) && this.port === 0) {
                this.port = parseInt(output.match(regex)![1], 10);
                this.logger.debug("Connecting via port " + this.port);
                this.process = csharpierProcess;
            }
        });
    }

    public async formatFile(content: string, filePath: string): Promise<string> {
        const parameter = {
            fileName: filePath,
            fileContents: content,
        };
        const result = await this.formatFile2(parameter);
        return result?.formattedFile ?? "";
    }

    public async formatFile2(parameter: FormatFileParameter): Promise<FormatFileResult | null> {
        if (this.processFailedToStart) {
            this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
            return null;
        }

        if (typeof this.process === "undefined") {
            await new Promise(r => setTimeout(r, 1000));
        }

        if (this.processFailedToStart || typeof this.process === "undefined") {
            this.logger.warn("CSharpier process failed to start. Formatting cannot occur.");
            return null;
        }

        try {
            const url = "http://localhost:" + this.port + "/format";

            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(parameter),
            });

            if (response.status !== 200) {
                this.logger.warn(
                    "Csharpier server returned non-200 status code of " + response.status,
                );
                return null;
            }

            return await response.json();
        } catch (e) {
            this.logger.warn("Failed posting to the csharpier server. " + e);
        }

        return null;
    }

    dispose() {
        if (typeof this.process !== "undefined") {
            (this.process.stdin as any).pause();
            this.process.kill();
        }
    }

    getVersion(): string {
        return this.version;
    }
}

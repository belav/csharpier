import { spawn } from "child_process";
import { performance } from "perf_hooks";
import { Disposable, languages, Range, TextDocument, TextEdit, window } from "vscode";
import { LoggingService } from "./LoggingService";
import * as path from "path";
import * as fs from "fs";

export class FormattingService implements Disposable {
    longRunner: import("child_process").ChildProcessWithoutNullStreams;
    callbacks: ((result: string) => void)[] = [];
    loggingService: LoggingService;

    constructor(loggingService: LoggingService) {
        this.loggingService = loggingService;

        this.loggingService.logInfo("Initializing.");

        let csharpierPath = "csharpier";
        
        if (fs.existsSync(path.resolve(__dirname, "../../CSharpier.Cli")))
        {
            csharpierPath = path.resolve(
                __dirname,
                "../../CSharpier.Cli/bin/debug/net6.0/dotnet-csharpier.dll",
            );
        }

        this.longRunner = spawn("dotnet", [csharpierPath, "--pipe-multiple-files"], {
            stdio: "pipe",
        });

        this.longRunner.stderr.on("data", chunk => {
            this.loggingService.logInfo("Got error: " + chunk.toString());
            const callback = this.callbacks.shift();
            if (callback) {
                callback("");
            }
        });

        this.longRunner.stdout.on("data", chunk => {
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

        this.loggingService.logInfo("Warm CSharpier with initial format");
        this.formatInPlace("public class ClassName { }", "Test.cs");

        languages.registerDocumentFormattingEditProvider("csharp", {
            provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
        });
    }

    private provideDocumentFormattingEdits = async (document: TextDocument) => {
        this.loggingService.logInfo("Formatting started for " + document.fileName + ".");
        const startTime = performance.now();
        // TODO this will be "Untitled-1" if it hasn't been saved
        const result = await this.formatInPlace(document.getText(), document.fileName);
        if (!result) {
            this.loggingService.logInfo("Formatting failed.");
            return [];
        }

        const endTime = performance.now();
        this.loggingService.logInfo("Formatted in " + (endTime - startTime) + "ms");

        return [TextEdit.replace(FormattingService.fullDocumentRange(document), result)];
    };

    private static fullDocumentRange(document: TextDocument): Range {
        const lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }

    private formatInPlace = async (content: string, fileName: string) => {
        this.longRunner.stdin.write(fileName);
        this.longRunner.stdin.write("\u0003");
        this.longRunner.stdin.write(content);
        this.longRunner.stdin.write("\u0003");
        return new Promise<string>(resolve => {
            this.callbacks.push(resolve);
        });
    };

    dispose() {
        (this.longRunner.stdin as any).pause();
        this.longRunner.kill();
    }
}

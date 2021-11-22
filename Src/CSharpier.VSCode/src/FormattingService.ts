import { spawn } from "child_process";
import { performance } from "perf_hooks";
import { types } from "util";
import { Disposable, languages, Range, TextDocument, TextEdit, window } from "vscode";
import { LoggingService } from "./LoggingService";

export class FormattingService implements Disposable {
    longRunner: import("child_process").ChildProcessWithoutNullStreams;
    // TODO fix this any
    callbacks: any = [];
    loggingService: LoggingService;

    constructor(loggingService: LoggingService) {
        this.loggingService = loggingService;
        this.longRunner = spawn(
            "C:\\projects\\csharpier\\Src\\CSharpier.Cli\\bin\\debug\\net6.0\\dotnet-csharpier.exe",
            ["--pipe-multiple-files"],
            {
                stdio: "pipe",
            },
        );

        this.longRunner.stdout.on("data", chunk => {
            const callback = this.callbacks.shift();
            if (callback) {
                callback(chunk.toString());
            }
        });

        // warm up csharpier so the first real format is fast
        this.formatInPlace("public class ClassName { }", "Test.cs");

        languages.registerDocumentFormattingEditProvider("csharp", {
            provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
        });
    }

    provideDocumentFormattingEdits = async (document: TextDocument) => {
        this.loggingService.logInfo(`Formatting started.`);

        // TODO what is this? it does not seem to matter
        // if (document.isUntitled) {
        //     return this.formatByTextEdit(document);
        // }

        this.loggingService.logInfo("Formatting in place");
        const startTime = performance.now();
        // TODO if the file fails to compile, we lose it
        const result = await this.formatInPlace(document.getText(), document.fileName);
        const endTime = performance.now();
        this.loggingService.logInfo("Formatted in " + (endTime - startTime) + "ms");

        return [TextEdit.replace(this.fullDocumentRange(document), result)];
    };

    private fullDocumentRange(document: TextDocument): Range {
        const lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }

    formatInPlace = async (content: string, fileName: string) => {
        this.longRunner.stdin.write(fileName);
        this.longRunner.stdin.write("\u0003");
        this.longRunner.stdin.write(content);
        this.longRunner.stdin.write("\u0003");
        return new Promise<string>(resolve => {
            this.callbacks.push(resolve);
        });
    };

    // TODO figure this out
    formatByTextEdit = async (document: TextDocument) => {
        return this.formatInPlace(document.getText(), document.fileName);
    };

    dispose() {
        // TODO do we need pause?
        (this.longRunner.stdin as any).pause();
        this.longRunner.kill();
    }
}

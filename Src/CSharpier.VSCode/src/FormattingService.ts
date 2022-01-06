import { performance } from "perf_hooks";
import { languages, Range, TextDocument, TextEdit } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { LoggingService } from "./LoggingService";

export class FormattingService {
    loggingService: LoggingService;
    csharpierProcessProvider: CSharpierProcessProvider;

    constructor(loggingService: LoggingService, csharpierProcessProvider: CSharpierProcessProvider) {
        this.loggingService = loggingService;
        this.csharpierProcessProvider = csharpierProcessProvider;

        languages.registerDocumentFormattingEditProvider("csharp", {
            provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
        });
    }

    private provideDocumentFormattingEdits = async (document: TextDocument) => {
        this.loggingService.logInfo("Formatting started for " + document.fileName + ".");
        const startTime = performance.now();
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
        return this.csharpierProcessProvider.getProcessFor(fileName).formatFile(content, fileName);
    };
}

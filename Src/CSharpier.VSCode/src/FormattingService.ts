import { performance } from "perf_hooks";
import { languages, Range, TextDocument, TextEdit } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { Logger } from "./Logger";

export class FormattingService {
    logger: Logger;
    csharpierProcessProvider: CSharpierProcessProvider;

    constructor(logger: Logger, csharpierProcessProvider: CSharpierProcessProvider) {
        this.logger = logger;
        this.csharpierProcessProvider = csharpierProcessProvider;

        languages.registerDocumentFormattingEditProvider("csharp", {
            provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
        });
    }

    private provideDocumentFormattingEdits = async (document: TextDocument) => {
        this.logger.info("Formatting started for " + document.fileName + ".");
        const startTime = performance.now();
        const text = document.getText();
        const newText = await this.format(text, document.fileName);
        const endTime = performance.now();
        this.logger.info("Formatted in " + (endTime - startTime) + "ms");
        if (!newText || newText === text) {
            this.logger.debug(
                "Skipping write because " + !newText
                    ? "result is empty"
                    : "current document equals result",
            );
            return [];
        }

        return [TextEdit.replace(FormattingService.fullDocumentRange(document), newText)];
    };

    private static fullDocumentRange(document: TextDocument): Range {
        const lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }

    private format = async (content: string, filePath: string) => {
        return this.csharpierProcessProvider.getProcessFor(filePath).formatFile(content, filePath);
    };
}

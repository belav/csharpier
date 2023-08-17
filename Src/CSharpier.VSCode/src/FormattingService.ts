import { performance } from "perf_hooks";
import { languages, Range, TextDocument, TextEdit, window } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { Logger } from "./Logger";

export class FormattingService {
    logger: Logger;
    csharpierProcessProvider: CSharpierProcessProvider;

    constructor(logger: Logger, csharpierProcessProvider: CSharpierProcessProvider) {
        this.logger = logger;
        this.csharpierProcessProvider = csharpierProcessProvider;

        languages.registerDocumentRangeFormattingEditProvider("csharp", {
            provideDocumentRangeFormattingEdits: this.provideDocumentRangeFormattingEdits,
        });
    }

    private provideDocumentRangeFormattingEdits = async (document: TextDocument, range: Range) => {
        this.logger.info("Formatting started for " + document.fileName + ".");
        const startTime = performance.now();
        const text = document.getText(range);
        const newText = await this.format(text, document.fileName);
        const endTime = performance.now();
        this.logger.info("Formatted in " + (endTime - startTime) + "ms");
        if (!newText || newText === text) {
            const errorMessage = "Skipping write because " + !newText
                    ? "File is empty or selected text is an incomplete code region"
                    : "current document equals result";
            
            console.warn("Error formatting document: " + errorMessage);
            return [];
        }

        return [TextEdit.replace(range, newText)];
    };

    private format = async (content: string, filePath: string) => {
        return this.csharpierProcessProvider.getProcessFor(filePath).formatFile(content, filePath);
    };
}
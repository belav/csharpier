import { performance } from "perf_hooks";
import { languages, Range, TextDocument, TextEdit, TextEditor, window } from "vscode";
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

        const editor = window.activeTextEditor;
        const nonEmptyLine = editor?.document.lineAt(range.start.line);
        if (!nonEmptyLine) {
            return [];
        }
        const indentation = nonEmptyLine.text.match(/^\s*/)?.[0] ?? "";

        const fullRange = new Range(nonEmptyLine.range.start, range.end);

        const text = document.getText(fullRange);
        const newText = await this.format(text, document.fileName);
        const formattedText = newText.replace(/^(?!$)/gm, indentation);

        const endTime = performance.now();
        this.logger.info("Formatted in " + (endTime - startTime) + "ms");
        if (!newText || newText === text) {
            const errorMessage = "Skipping write because " + !newText
                    ? "File is empty or selected text is an incomplete code region"
                    : "current document equals result";
            
            console.warn("Error formatting document: " + errorMessage);
            return [];
        }

        if (formattedText === text) {
            return [];
        }

        return [TextEdit.replace(fullRange, formattedText)];
    };

    private format = async (content: string, filePath: string) => {
        return this.csharpierProcessProvider.getProcessFor(filePath).formatFile(content, filePath);
    };
}
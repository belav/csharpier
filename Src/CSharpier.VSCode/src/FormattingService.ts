import {
    CancellationToken,
    FormattingOptions,
    languages,
    Position,
    Range,
    TextDocument,
    TextEdit,
} from "vscode";
import { Difference, generateDifferences } from "prettier-linter-helpers";
import { FormatDocumentProvider } from "./FormatDocumentProvider";

export class FormattingService {
    constructor(private readonly formatDocumentProvider: FormatDocumentProvider) {
        languages.registerDocumentFormattingEditProvider("csharp", {
            provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
        });

        languages.registerDocumentRangeFormattingEditProvider("csharp", {
            provideDocumentRangeFormattingEdits: this.provideDocumentRangeFormattingEdits,
        });
    }

    private provideDocumentRangeFormattingEdits = async (
        document: TextDocument,
        range: Range,
    ): Promise<TextEdit[]> => {
        const differences = await this.getDifferences(document);
        const edits: TextEdit[] = [];

        for (const difference of differences) {
            const diffRange = this.getRange(document, difference);
            if (range.contains(diffRange)) {
                const textEdit = this.getTextEdit(diffRange, difference);
                if (textEdit) {
                    edits.push(textEdit);
                }
            }
        }

        return edits;
    };

    private getTextEdit(range: Range, difference: Difference) {
        if (difference.operation === generateDifferences.INSERT) {
            return TextEdit.insert(
                new Position(range.start.line, range.start.character),
                difference.insertText!,
            );
        } else if (difference.operation === generateDifferences.REPLACE) {
            return TextEdit.replace(range, difference.insertText!);
        } else if (difference.operation === generateDifferences.DELETE) {
            return TextEdit.delete(range);
        }
    }

    private async getDifferences(document: TextDocument) {
        const source = document.getText();
        const formattedSource =
            (await this.formatDocumentProvider.formatDocument(document)) ?? source;
        return generateDifferences(source, formattedSource);
    }

    private getRange(document: TextDocument, difference: Difference): Range {
        if (difference.operation === generateDifferences.INSERT) {
            const start = document.positionAt(difference.offset);
            return new Range(start.line, start.character, start.line, start.character);
        }
        const start = document.positionAt(difference.offset);
        const end = document.positionAt(difference.offset + difference.deleteText!.length);
        return new Range(start.line, start.character, end.line, end.character);
    }

    private provideDocumentFormattingEdits = async (document: TextDocument) => {
        const updateText = (newText: string) => {
            return [TextEdit.replace(FormattingService.fullDocumentRange(document), newText)];
        };

        const formattedSource = await this.formatDocumentProvider.formatDocument(document);

        if (formattedSource) {
            return updateText(formattedSource);
        }

        return [];
    };

    private static fullDocumentRange(document: TextDocument): Range {
        const lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }
}

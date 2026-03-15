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
import { Logger } from "./Logger";

export class FormattingService {
    constructor(
        private readonly formatDocumentProvider: FormatDocumentProvider,
        supportedLanguageIds: string[],
    ) {
        for (let languageId of supportedLanguageIds) {
            languages.registerDocumentFormattingEditProvider(languageId, {
                provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
            });

            languages.registerDocumentRangeFormattingEditProvider(languageId, {
                provideDocumentRangeFormattingEdits: this.provideDocumentRangeFormattingEdits,
            });
        }
    }

    private provideDocumentRangeFormattingEdits = async (
        document: TextDocument,
        range: Range,
    ): Promise<TextEdit[]> => {
        let differences = await this.getDifferences(document);
        let edits: TextEdit[] = [];

        for (let difference of differences) {
            let diffRange = this.getRange(document, difference);
            if (range.contains(diffRange)) {
                let textEdit = this.getTextEdit(diffRange, difference);
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
        let source = document.getText();
        let formattedSource =
            (await this.formatDocumentProvider.formatDocument(document)) ?? source;
        return generateDifferences(source, formattedSource);
    }

    private getRange(document: TextDocument, difference: Difference): Range {
        if (difference.operation === generateDifferences.INSERT) {
            let start = document.positionAt(difference.offset);
            return new Range(start.line, start.character, start.line, start.character);
        }
        let start = document.positionAt(difference.offset);
        let end = document.positionAt(difference.offset + difference.deleteText!.length);
        return new Range(start.line, start.character, end.line, end.character);
    }

    private provideDocumentFormattingEdits = async (document: TextDocument) => {
        let formattedSource = await this.formatDocumentProvider.formatDocument(document);

        if (formattedSource) {
            return [this.minimalEdit(document, formattedSource)];
        }

        return [];
    };

    private minimalEdit(document: TextDocument, newText: string) {
        let existingText = document.getText();
        
        let i = 0;
        while (i < existingText.length && i < newText.length && existingText[i] === newText[i]) {
            ++i;
        }

        let j = 0;
        while (
            i + j < existingText.length &&
            i + j < newText.length &&
            existingText[existingText.length - j - 1] === newText[newText.length - j - 1]
        ) {
            ++j;
        }
        let minimalChangedText = newText.substring(i, newText.length - j);
        let pos0 = document.positionAt(i);
        let pos1 = document.positionAt(existingText.length - j);

        return TextEdit.replace(new Range(pos0, pos1), minimalChangedText);
    }

    private static fullDocumentRange(document: TextDocument): Range {
        let lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }
}

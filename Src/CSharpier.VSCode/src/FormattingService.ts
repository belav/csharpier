import { languages, Range, TextDocument, TextEdit } from "vscode";
import { FormatDocumentProvider } from "./FormatDocumentProvider";

export class FormattingService {
    constructor(
        private readonly formatDocumentProvider: FormatDocumentProvider,
        supportedLanguageIds: string[],
    ) {
        for (let languageId of supportedLanguageIds) {
            languages.registerDocumentFormattingEditProvider(languageId, {
                provideDocumentFormattingEdits: this.provideDocumentFormattingEdits,
            });
        }
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
}

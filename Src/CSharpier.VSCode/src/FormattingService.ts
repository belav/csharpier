import { performance } from "perf_hooks";
import { languages, Range, TextDocument, TextEdit } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { Logger } from "./Logger";
import { Status } from "./ICSharpierProcess";

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
        const csharpierProcess = this.csharpierProcessProvider.getProcessFor(document.fileName);

        this.logger.info(
            "Formatting started for " +
                document.fileName +
                " using CSharpier " +
                csharpierProcess.getVersion(),
        );
        const startTime = performance.now();
        const text = document.getText();

        const updateText = (newText: string) => {
            return [TextEdit.replace(FormattingService.fullDocumentRange(document), newText)];
        };

        if ("formatFile2" in csharpierProcess) {
            const parameter = {
                fileContents: text,
                fileName: document.fileName,
            };
            const result = await csharpierProcess.formatFile2(parameter);

            this.logger.info("Formatted in " + (performance.now() - startTime) + "ms");

            if (result == null) {
                return;
            }

            switch (result.status) {
                case "Formatted":
                    return updateText(result.formattedFile);
                case "Ignored":
                    this.logger.info("File is ignored by csharpier cli.");
                    break;
                case "Failed":
                    this.logger.warn(
                        "CSharpier cli failed to format the file and returned the following error: " +
                            result.errorMessage,
                    );
                    break;
                default:
                    this.logger.warn("Didn't handle " + result.status);
                    break;
            }
        } else {
            const newText = await csharpierProcess.formatFile(text, document.fileName);
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

            return updateText(newText);
        }

        return [];
    };

    private static fullDocumentRange(document: TextDocument): Range {
        const lastLineId = document.lineCount - 1;
        return new Range(0, 0, lastLineId, document.lineAt(lastLineId).text.length);
    }
}

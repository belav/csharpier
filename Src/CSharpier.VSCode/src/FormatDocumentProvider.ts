import { TextDocument } from "vscode";
import { Logger } from "./Logger";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { performance } from "perf_hooks";

export class FormatDocumentProvider {
    constructor(
        private logger: Logger,
        private csharpierProcessProvider: CSharpierProcessProvider,
    ) {}

    async formatDocument(
        document: TextDocument,
        // the diagnostic service calls format on almost every keypress, avoid logging those formats so we don't spawn the output
        writeLogs: boolean = true,
    ): Promise<string | null> {
        let csharpierProcess = this.csharpierProcessProvider.getProcessFor(document.fileName);
        let text = document.getText();
        let startTime = performance.now();

        if ("formatFile2" in csharpierProcess) {
            let parameter = {
                fileContents: text,
                fileName: document.fileName,
            };
            let result = await csharpierProcess.formatFile2(parameter);

            if (result == null) {
                return null;
            }

            switch (result.status) {
                case "Formatted":
                    if (writeLogs) {
                        this.logger.info("Formatted in " + (performance.now() - startTime) + "ms");
                    }
                    return result.formattedFile;
                case "Ignored":
                    if (writeLogs) {
                        this.logger.info("File is ignored by csharpier cli.");
                    }
                    break;
                case "Failed":
                    if (writeLogs) {
                        this.logger.warn(
                            "CSharpier cli failed to format the file and returned the following error: " +
                                result.errorMessage,
                        );
                    }
                    break;
                case "UnsupportedFile":
                    if (writeLogs) {
                        this.logger.debug(
                            "CSharpier does not support formatting the file " + document.fileName,
                        );
                    }
                    break;
                default:
                    if (writeLogs) {
                        this.logger.warn("Didn't handle " + result.status);
                    }
                    break;
            }
        } else {
            let newText = await csharpierProcess.formatFile(text, document.fileName);
            let endTime = performance.now();
            if (writeLogs) {
                this.logger.info("Formatted in " + (endTime - startTime) + "ms");
            }
            if (!newText || newText === text) {
                if (writeLogs) {
                    this.logger.debug(
                        "Skipping write because " + !newText
                            ? "result is empty"
                            : "current document equals result",
                    );
                }
                return null;
            }

            return newText;
        }

        return null;
    }
}

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
        writeLogs: boolean = true,
    ): Promise<string | null> {
        const csharpierProcess = this.csharpierProcessProvider.getProcessFor(document.fileName);
        const text = document.getText();
        const startTime = performance.now();

        if ("formatFile2" in csharpierProcess) {
            const parameter = {
                fileContents: text,
                fileName: document.fileName,
            };
            const result = await csharpierProcess.formatFile2(parameter);

            if (writeLogs) {
                this.logger.info("Formatted in " + (performance.now() - startTime) + "ms");
            }

            if (result == null) {
                return null;
            }

            switch (result.status) {
                case "Formatted":
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
            const newText = await csharpierProcess.formatFile(text, document.fileName);
            const endTime = performance.now();
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

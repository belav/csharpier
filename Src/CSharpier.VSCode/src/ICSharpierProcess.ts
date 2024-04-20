import { Disposable } from "vscode";

export interface ICSharpierProcess extends Disposable {
    getVersion(): string;
    getProcessFailedToStart(): boolean;
    formatFile(content: string, filePath: string): Promise<string>;
}

export interface ICSharpierProcess2 extends ICSharpierProcess {
    formatFile2(parameter: FormatFileParameter): Promise<FormatFileResult | null>;
}

export interface FormatFileParameter {
    fileContents: string;
    fileName: string;
}

export interface FormatFileResult {
    formattedFile: string;
    status: Status;
    errorMessage: string;
}

export type Status = "Formatted" | "Ignored" | "Failed";

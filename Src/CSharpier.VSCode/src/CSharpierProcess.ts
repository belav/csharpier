import { Disposable } from "vscode";

export interface ICSharpierProcess extends Disposable {
    formatFile(content: string, filePath: string): Promise<string>;
}

export class NullCSharpierProcess implements ICSharpierProcess {
    formatFile(content: string, filePath: string): Promise<string> {
        return Promise.resolve("");
    }

    dispose() {}
}

import { Disposable } from "vscode";

export interface ICSharpierProcess extends Disposable {
    formatFile(content: string, fileName: string): Promise<string>;
}

export class NullCSharpierProcess implements ICSharpierProcess {
    formatFile(content: string, fileName: string): Promise<string> {
        return Promise.resolve("");
    }

    dispose() {}
}

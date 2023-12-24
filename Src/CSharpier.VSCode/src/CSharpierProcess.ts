import { Disposable } from "vscode";
import { Logger } from "./Logger";

export interface ICSharpierProcess extends Disposable {
    formatFile(content: string, filePath: string): Promise<string>;
}

export class NullCSharpierProcess implements ICSharpierProcess {
    static instance: NullCSharpierProcess;
    private logger: Logger;

    static create(logger: Logger) {
        this.instance = new NullCSharpierProcess(logger);
    }

    private constructor(logger: Logger) {
        this.logger = logger;
    }

    formatFile(content: string, filePath: string): Promise<string> {
        this.logger.debug("Skipping formatting because this is a NullCSharpierProcess. This generally indicates there was a problem starting the CSharpier process");
        return Promise.resolve("");
    }

    dispose() {}
}

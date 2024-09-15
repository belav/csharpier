import { Logger } from "./Logger";
import { ICSharpierProcess } from "./ICSharpierProcess";

export class NullCSharpierProcess implements ICSharpierProcess {
    static instance: NullCSharpierProcess;
    private logger: Logger;

    static create(logger: Logger) {
        this.instance = new NullCSharpierProcess(logger);
    }

    private constructor(logger: Logger) {
        this.logger = logger;
    }

    getProcessFailedToStart(): boolean {
        return false;
    }

    formatFile(content: string, filePath: string): Promise<string> {
        this.logger.debug(
            "Skipping formatting because this is a NullCSharpierProcess. This generally indicates there was a problem starting the CSharpier process",
        );
        return Promise.resolve("");
    }

    dispose() {}

    getVersion(): string {
        return "NULL";
    }
}

import { window } from "vscode";

type LogLevel = "DEBUG" | "INFO" | "WARN" | "ERROR" | "NONE";

export class LoggingService {
    private outputChannel = window.createOutputChannel("CSharpier");

    constructor(enableDebugLogs: boolean) {
        if (enableDebugLogs) {
            this.logLevel = "DEBUG";
        }
    }

    private logLevel: LogLevel = "INFO";

    public setOutputLevel(logLevel: LogLevel) {
        this.logLevel = logLevel;
    }

    public logDebug(message: string, data?: unknown): void {
        if (
            this.logLevel === "NONE" ||
            this.logLevel === "INFO" ||
            this.logLevel === "WARN" ||
            this.logLevel === "ERROR"
        ) {
            return;
        }
        this.logMessage(message, "DEBUG");
        if (data) {
            this.logObject(data);
        }
    }

    public logInfo(message: string, data?: unknown): void {
        if (this.logLevel === "NONE" || this.logLevel === "WARN" || this.logLevel === "ERROR") {
            return;
        }
        this.logMessage(message, "INFO");
        if (data) {
            this.logObject(data);
        }
    }

    public logWarning(message: string, data?: unknown): void {
        if (this.logLevel === "NONE" || this.logLevel === "ERROR") {
            return;
        }
        this.logMessage(message, "WARN");
        if (data) {
            this.logObject(data);
        }
    }

    public logError(message: string, error?: unknown) {
        if (this.logLevel === "NONE") {
            return;
        }
        this.logMessage(message, "ERROR");
        if (typeof error === "string") {
            // Errors as a string usually only happen with
            // plugins that don't return the expected error.
            this.outputChannel.appendLine(error);
        } else if (error instanceof Error) {
            if (error?.message) {
                this.logMessage(error.message, "ERROR");
            }
            if (error?.stack) {
                this.outputChannel.appendLine(error.stack);
            }
        } else if (error) {
            this.logObject(error);
        }
    }

    public show() {
        this.outputChannel.show();
    }

    private logObject(data: unknown): void {
        const message = JSON.stringify(data, null, 2);

        this.outputChannel.appendLine(message);
    }

    private logMessage(message: string, logLevel: LogLevel): void {
        const title = new Date().toLocaleTimeString();
        this.outputChannel.appendLine(`["${logLevel}" - ${title}] ${message}`);
        console.log(message);
    }
}

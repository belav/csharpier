import { window } from "vscode";

type LogLevel = "DEBUG" | "INFO" | "WARN" | "ERROR" | "NONE";

export class Logger {
    private outputChannel = window.createOutputChannel("CSharpier");
    private readonly logLevel: LogLevel = "INFO";

    constructor(enableDebugLogs: boolean) {
        if (enableDebugLogs) {
            this.logLevel = "DEBUG";
        }
    }

    public debug(message: any, data?: unknown): void {
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

    public info(message: any, data?: unknown): void {
        if (this.logLevel === "NONE" || this.logLevel === "WARN" || this.logLevel === "ERROR") {
            return;
        }
        this.logMessage(message, "INFO");
        if (data) {
            this.logObject(data);
        }
    }

    public warn(message: any, data?: unknown): void {
        if (this.logLevel === "NONE" || this.logLevel === "ERROR") {
            return;
        }
        this.logMessage(message, "WARN");
        if (data) {
            this.logObject(data);
        }
    }

    public error(message: any, error?: unknown) {
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
        let message = JSON.stringify(data, null, 2);

        this.outputChannel.appendLine(message);
    }

    private logMessage(message: any, logLevel: LogLevel): void {
        let title = new Date().toLocaleTimeString();
        this.outputChannel.appendLine(`["${logLevel}" - ${title}] ${message}`);
        console.log(message);
    }
}

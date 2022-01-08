import { ExtensionContext, workspace } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { FormattingService } from "./FormattingService";
import { LoggingService } from "./LoggingService";

export async function activate(context: ExtensionContext) {
    if (!workspace.isTrusted) {
        workspace.onDidGrantWorkspaceTrust(() => initPlugin(context));
        return;
    }

    await initPlugin(context);
}

let initPlugin = async (context: ExtensionContext) => {
    let enableDebugLogs =
        workspace.getConfiguration("csharpier").get<boolean>("enableDebugLogs") ?? false;

    let loggingService = new LoggingService(enableDebugLogs);

    let isDevelopment = (process.env as any).MODE === "development";

    loggingService.logInfo("Initializing " + (process.env as any).EXTENSION_NAME);

    let csharpierProcessProvider = new CSharpierProcessProvider(loggingService);
    new FormattingService(loggingService, csharpierProcessProvider);

    context.subscriptions.push(csharpierProcessProvider);
};

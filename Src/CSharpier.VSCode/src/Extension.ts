import { ExtensionContext, workspace } from "vscode";
import { CSharpierService } from "./CSharpierService";
import { FormattingService } from "./FormattingService";
import { LoggingService } from "./LoggingService";

export async function activate(context: ExtensionContext) {
    if (!workspace.isTrusted) {
        workspace.onDidGrantWorkspaceTrust(() => initPlugin(context));
        return;
    }

    await initPlugin(context);
}

const initPlugin = async (context: ExtensionContext) => {
    const enableDebugLogs =
        workspace.getConfiguration("csharpier").get<boolean>("enableDebugLogs") ?? false;

    const loggingService = new LoggingService(enableDebugLogs);

    const isDevelopment = (process.env as any).MODE === "development";

    loggingService.logInfo("Initializing " + (process.env as any).EXTENSION_NAME);

    const csharpierService = new CSharpierService(loggingService);
    const formattingService = new FormattingService(loggingService, csharpierService);

    context.subscriptions.push(csharpierService);
};

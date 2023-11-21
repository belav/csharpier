import { ExtensionContext, workspace } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { FormattingService } from "./FormattingService";
import { Logger } from "./Logger";
import { NullCSharpierProcess } from "./CSharpierProcess";

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

    const logger = new Logger(enableDebugLogs);
    NullCSharpierProcess.create(logger);

    const isDevelopment = (process.env as any).MODE === "development";

    logger.info("Initializing " + (process.env as any).EXTENSION_NAME);

    const csharpierProcessProvider = new CSharpierProcessProvider(logger, context.extension);
    new FormattingService(logger, csharpierProcessProvider);

    context.subscriptions.push(csharpierProcessProvider);
};

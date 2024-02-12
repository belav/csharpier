import { ExtensionContext, workspace } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { FormattingService } from "./FormattingService";
import { Logger } from "./Logger";
import { NullCSharpierProcess } from "./CSharpierProcess";
import { findDotNet } from "./DotNetProvider";

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

    logger.info("Initializing " + (process.env as any).EXTENSION_NAME);

    if (!(await findDotNet(logger))) {
        logger.error(
            "CSharpier was unable to find a way to run 'dotnet' commands. Check your PATH or set dotnet.dotnetPath or omnisharp.dotNetCliPaths",
        );
        return;
    }

    NullCSharpierProcess.create(logger);

    const csharpierProcessProvider = new CSharpierProcessProvider(logger, context.extension);
    new FormattingService(logger, csharpierProcessProvider);

    context.subscriptions.push(csharpierProcessProvider);
};

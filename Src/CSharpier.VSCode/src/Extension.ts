import { DocumentFilter, ExtensionContext, window, workspace } from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { FormattingService } from "./FormattingService";
import { Logger } from "./Logger";
import { findDotNet } from "./DotNetProvider";
import { options } from "./Options";
import { NullCSharpierProcess } from "./NullCSharpierProcess";
import { FixAllCodeActionsCommand } from "./FixAllCodeActionCommand";
import { DiagnosticsService } from "./DiagnosticsService";
import { FixAllCodeActionProvider } from "./FixAllCodeActionProvider";
import { FormatDocumentProvider } from "./FormatDocumentProvider";

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
        // add to path via sudo ln -s ~/.dotnet/dotnet /usr/bin/dotnet
        logger.debug("PATH: " + process.env.PATH);
        logger.debug("dotnet.dotnetPath: " + options.dotnetPath);
        logger.debug("omnisharp.dotNetCliPaths: " + options.dotNetCliPaths);
        window.showErrorMessage(
            "CSharpier was unable to find a way to run 'dotnet' commands. If a .NET SDK is installed make sure it can be found on PATH. Alternatively set dotnet.dotnetPath or omnisharp.dotNetCliPaths. You will need to restart VSCode after making any changes.",
        );
        return;
    }

    NullCSharpierProcess.create(logger);

    const csharpierProcessProvider = new CSharpierProcessProvider(logger, context.extension);
    const formatDocumentProvider = new FormatDocumentProvider(logger, csharpierProcessProvider);
    const diagnosticsDocumentSelector: DocumentFilter[] = [
        {
            language: "csharp",
            scheme: "file",
        },
    ];
    const diagnosticsService = new DiagnosticsService(
        formatDocumentProvider,
        diagnosticsDocumentSelector,
        logger,
    );
    const fixAllCodeActionProvider = new FixAllCodeActionProvider(diagnosticsDocumentSelector);

    new FormattingService(formatDocumentProvider);
    new FixAllCodeActionsCommand(context, formatDocumentProvider, logger);

    context.subscriptions.push(
        csharpierProcessProvider,
        fixAllCodeActionProvider,
        diagnosticsService,
    );
};

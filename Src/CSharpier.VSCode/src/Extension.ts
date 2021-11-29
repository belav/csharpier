import { env, ExtensionContext, Uri, window, workspace } from "vscode";
import { FormattingService } from "./FormattingService";
import { isInstalled } from "./IsInstalled";
import { LoggingService } from "./LoggingService";

export async function activate(context: ExtensionContext) {
    if (!workspace.isTrusted) {
        workspace.onDidGrantWorkspaceTrust(() => initPlugin(context));
        return;
    }

    initPlugin(context);
}

const initPlugin = async (context: ExtensionContext) => {
    const enableDebugLogs =
        workspace.getConfiguration("csharpier-vscode").get<boolean>("enableDebugLogs") ?? false;

    const loggingService = new LoggingService(enableDebugLogs);

    const installed = await isInstalled("dotnet-csharpier");
    if (!installed) {
        await displayInstallNeededMessage(loggingService);
        // TODO return;
    }

    const formattingService = new FormattingService(loggingService);

    context.subscriptions.push(formattingService);
};

// TODO we can probably do something to install for the user?
const displayInstallNeededMessage = async (loggingService: LoggingService) => {
    loggingService.logError("CSharpier not found");

    const selection = await window.showErrorMessage(
        "CSharpier must be installed globally.",
        "Go to CSharpiers Github",
    );

    if (selection === "Go to CSharpiers Github") {
        env.openExternal(Uri.parse("https://github.com/belav/csharpier"));
    }
};

import * as vscode from "vscode";
import { Logger } from "./Logger";
import { FormatDocumentProvider } from "./FormatDocumentProvider";

export class FixAllCodeActionsCommand {
    public static readonly Id = "csharpier-vscode.fixAllCodeActions";
    public readonly id = FixAllCodeActionsCommand.Id;

    constructor(
        private readonly context: vscode.ExtensionContext,
        private readonly formatDocumentProvider: FormatDocumentProvider,
        private readonly logger: Logger,
    ) {
        this.context.subscriptions.push(vscode.commands.registerCommand(this.id, this.execute));
    }

    public execute = async (document: vscode.TextDocument): Promise<void> => {
        try {
            const changes = await this.getChanges(document);
            if (!changes) {
                return;
            }
            const range = new vscode.Range(
                document.positionAt(0),
                document.positionAt(document.getText().length),
            );
            const workspaceEdit = new vscode.WorkspaceEdit();
            workspaceEdit.replace(document.uri, range, changes);
            await vscode.workspace.applyEdit(workspaceEdit);
        } catch (e) {
            this.logger.error(`Unable to apply workspace edits: ${(e as Error).message}`);
        }
    };

    private async getChanges(document: vscode.TextDocument): Promise<string | null> {
        const formattedSource = (await this.formatDocumentProvider.formatDocument(document)) ?? "";
        return formattedSource;
    }
}

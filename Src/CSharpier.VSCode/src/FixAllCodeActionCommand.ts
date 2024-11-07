import * as vscode from "vscode";
import { CSharpierProcessProvider } from "./CSharpierProcessProvider";
import { Logger } from "./Logger";

export class FixAllCodeActionsCommand {
    public static readonly Id = "csharpier-vscode.fixAllCodeActions";
    public readonly id = FixAllCodeActionsCommand.Id;

    constructor(
        private readonly context: vscode.ExtensionContext,
        private readonly csharpierProcessProvider: CSharpierProcessProvider,
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
        let formattedSource = "";
        const source = document.getText();
        const csharpierProcess = this.csharpierProcessProvider.getProcessFor(document.fileName);
        if ("formatFile2" in csharpierProcess) {
            const parameter = {
                fileContents: source,
                fileName: document.fileName,
            };
            const result = await csharpierProcess.formatFile2(parameter);
            if (result) {
                formattedSource = result.formattedFile;
            }
        } else {
            formattedSource = await csharpierProcess.formatFile(source, document.fileName);
        }
        return formattedSource;
    }
}

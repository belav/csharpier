import * as vscode from "vscode";
import { FixAllCodeActionsCommand } from "./FixAllCodeActionCommand";

const noChanges: vscode.CodeAction[] = [];

export class FixAllCodeActionProvider implements vscode.CodeActionProvider, vscode.Disposable {
    public static readonly fixAllCodeActionKind =
        vscode.CodeActionKind.SourceFixAll.append("csharpier");

    public static metadata: vscode.CodeActionProviderMetadata = {
        providedCodeActionKinds: [FixAllCodeActionProvider.fixAllCodeActionKind],
    };

    private readonly codeActionsProvider: vscode.Disposable;

    constructor(private readonly documentSelector: vscode.DocumentSelector) {
        this.codeActionsProvider = vscode.languages.registerCodeActionsProvider(
            this.documentSelector,
            this,
            FixAllCodeActionProvider.metadata,
        );
    }

    public dispose() {
        this.codeActionsProvider.dispose();
    }

    public provideCodeActions(
        document: vscode.TextDocument,
        _range: vscode.Range | vscode.Selection,
        context: vscode.CodeActionContext,
        cancellationToken: vscode.CancellationToken,
    ): vscode.CodeAction[] {
        if (!context.only) {
            return noChanges;
        }
        if (
            !context.only.contains(FixAllCodeActionProvider.fixAllCodeActionKind) &&
            !FixAllCodeActionProvider.fixAllCodeActionKind.contains(context.only)
        ) {
            return noChanges;
        }
        const title = "Format code using CSharpier";
        const action = new vscode.CodeAction(title, FixAllCodeActionProvider.fixAllCodeActionKind);
        action.command = {
            title,
            command: FixAllCodeActionsCommand.Id,
            arguments: [document, cancellationToken],
        };
        return [action];
    }
}

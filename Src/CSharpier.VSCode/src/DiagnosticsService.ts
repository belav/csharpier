import * as vscode from "vscode";
import { Difference, generateDifferences, showInvisibles } from "prettier-linter-helpers";
import { FixAllCodeActionsCommand } from "./FixAllCodeActionCommand";
import { Logger } from "./Logger";
import { FormatDocumentProvider } from "./FormatDocumentProvider";
import { workspace } from "vscode";

const DIAGNOSTICS_ID = "csharpier";
const DIAGNOSTICS_SOURCE_ID = "diagnostic";

export interface CsharpierDiff {
    source: string;
    formattedSource: string;
    differences: Difference[];
}

export class DiagnosticsService implements vscode.CodeActionProvider, vscode.Disposable {
    public static readonly quickFixCodeActionKind =
        vscode.CodeActionKind.QuickFix.append(DIAGNOSTICS_ID);
    public static metadata: vscode.CodeActionProviderMetadata = {
        providedCodeActionKinds: [DiagnosticsService.quickFixCodeActionKind],
    };

    private readonly diagnosticCollection: vscode.DiagnosticCollection;
    private readonly diagnosticDifferenceMap: Map<vscode.Diagnostic, Difference> = new Map();
    private readonly codeActionsProvider: vscode.Disposable;
    private readonly disposables: vscode.Disposable[] = [];

    constructor(
        private readonly formatDocumentProvider: FormatDocumentProvider,
        private readonly documentSelector: Array<vscode.DocumentFilter>,
        private readonly logger: Logger,
    ) {
        this.diagnosticCollection = vscode.languages.createDiagnosticCollection(DIAGNOSTICS_ID);
        this.codeActionsProvider = vscode.languages.registerCodeActionsProvider(
            this.documentSelector,
            this,
            DiagnosticsService.metadata,
        );
        this.registerEditorEvents();
    }

    public dispose(): void {
        for (const disposable of this.disposables) {
            disposable.dispose();
        }
        this.diagnosticCollection.dispose();
        this.codeActionsProvider.dispose();
    }

    private handleChangeTextDocument(document: vscode.TextDocument): void {
        void this.runDiagnostics(document);
    }

    public async runDiagnostics(document: vscode.TextDocument): Promise<void> {
        const shouldRunDiagnostics =
            this.documentSelector.some(selector => selector.language === document.languageId) &&
            !!vscode.workspace.getWorkspaceFolder(document.uri) &&
            (workspace.getConfiguration("csharpier").get<boolean>("enableDiagnostics") ?? true);
        if (shouldRunDiagnostics) {
            try {
                const diff = await this.getDiff(document);
                this.updateDiagnostics(document, diff);
            } catch (e) {
                this.logger.error(`Unable to provide diagnostics: ${(e as Error).message}`);
            }
        }
    }

    public updateDiagnostics(document: vscode.TextDocument, diff: CsharpierDiff): void {
        const diagnostics = this.getDiagnostics(document, diff);
        this.diagnosticCollection.set(document.uri, diagnostics);
    }

    private registerEditorEvents(): void {
        const activeDocument = vscode.window.activeTextEditor?.document;
        if (activeDocument) {
            void this.runDiagnostics(activeDocument);
        }

        const onDidChangeTextDocument = vscode.workspace.onDidChangeTextDocument(
            (e: vscode.TextDocumentChangeEvent) => {
                if (
                    e.contentChanges.length &&
                    vscode.window.activeTextEditor?.document === e.document
                ) {
                    this.handleChangeTextDocument(e.document);
                }
            },
        );

        const onDidChangeActiveTextEditor = vscode.window.onDidChangeActiveTextEditor(
            (editor?: vscode.TextEditor) => {
                if (editor) {
                    void this.runDiagnostics(editor.document);
                }
            },
        );

        this.disposables.push(
            onDidChangeTextDocument,
            onDidChangeActiveTextEditor,
            this.diagnosticCollection,
        );
    }

    private getDiagnostics(
        document: vscode.TextDocument,
        diff: CsharpierDiff,
    ): vscode.Diagnostic[] {
        const diagnostics: vscode.Diagnostic[] = [];
        for (const difference of diff.differences) {
            const diagnostic = this.getDiagnostic(document, difference);
            this.diagnosticDifferenceMap.set(diagnostic, difference);
            diagnostics.push(diagnostic);
        }
        return diagnostics;
    }

    private getDiagnostic(
        document: vscode.TextDocument,
        difference: Difference,
    ): vscode.Diagnostic {
        const range = this.getRange(document, difference);
        const message = this.getMessage(difference);
        const diagnostic = new vscode.Diagnostic(range, message);
        diagnostic.source = DIAGNOSTICS_ID;
        diagnostic.code = DIAGNOSTICS_SOURCE_ID;
        return diagnostic;
    }

    private getMessage(difference: Difference): string {
        switch (difference.operation) {
            case generateDifferences.INSERT:
                return `Insert ${showInvisibles(difference.insertText!)}`;
            case generateDifferences.REPLACE:
                return `Replace ${showInvisibles(difference.deleteText!)} with ${showInvisibles(
                    difference.insertText!,
                )}`;
            case generateDifferences.DELETE:
                return `Delete ${showInvisibles(difference.deleteText!)}`;
            default:
                return "";
        }
    }

    private getRange(document: vscode.TextDocument, difference: Difference): vscode.Range {
        if (difference.operation === generateDifferences.INSERT) {
            const start = document.positionAt(difference.offset);
            return new vscode.Range(start.line, start.character, start.line, start.character);
        }
        const start = document.positionAt(difference.offset);
        const end = document.positionAt(difference.offset + difference.deleteText!.length);
        return new vscode.Range(start.line, start.character, end.line, end.character);
    }

    private async getDiff(document: vscode.TextDocument): Promise<CsharpierDiff> {
        const source = document.getText();
        const formattedSource =
            (await this.formatDocumentProvider.formatDocument(document)) ?? source;
        const differences = generateDifferences(source, formattedSource);
        return {
            source,
            formattedSource,
            differences,
        };
    }

    public provideCodeActions(
        document: vscode.TextDocument,
        range: vscode.Range | vscode.Selection,
    ): vscode.CodeAction[] {
        let totalDiagnostics = 0;
        const codeActions: vscode.CodeAction[] = [];
        this.diagnosticCollection.forEach(
            (uri: vscode.Uri, diagnostics: readonly vscode.Diagnostic[]) => {
                if (document.uri.fsPath !== uri.fsPath) {
                    return;
                }
                diagnostics.forEach((diagnostic: vscode.Diagnostic) => {
                    totalDiagnostics += 1;
                    if (!range.isEqual(diagnostic.range)) {
                        return;
                    }
                    const difference = this.diagnosticDifferenceMap.get(diagnostic);
                    codeActions.push(
                        this.getQuickFixCodeAction(document.uri, diagnostic, difference!),
                    );
                });
            },
        );
        if (totalDiagnostics > 1) {
            codeActions.push(this.getQuickFixAllProblemsCodeAction(document, totalDiagnostics));
        }
        return codeActions;
    }

    private getQuickFixCodeAction(
        uri: vscode.Uri,
        diagnostic: vscode.Diagnostic,
        difference: Difference,
    ): vscode.CodeAction {
        const action = new vscode.CodeAction(
            `Fix this ${DIAGNOSTICS_ID} problem`,
            DiagnosticsService.quickFixCodeActionKind,
        );
        action.edit = new vscode.WorkspaceEdit();
        if (difference.operation === generateDifferences.INSERT) {
            action.edit.insert(uri, diagnostic.range.start, difference.insertText!);
        } else if (difference.operation === generateDifferences.REPLACE) {
            action.edit.replace(uri, diagnostic.range, difference.insertText!);
        } else if (difference.operation === generateDifferences.DELETE) {
            action.edit.delete(uri, diagnostic.range);
        }
        return action;
    }

    private getQuickFixAllProblemsCodeAction(
        document: vscode.TextDocument,
        totalDiagnostics: number,
    ): vscode.CodeAction {
        const title = `Fix all ${DIAGNOSTICS_ID} problems (${totalDiagnostics})`;
        const action = new vscode.CodeAction(title, DiagnosticsService.quickFixCodeActionKind);
        action.command = {
            title,
            command: FixAllCodeActionsCommand.Id,
            arguments: [document],
        };
        return action;
    }
}

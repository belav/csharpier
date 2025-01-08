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
    private static readonly quickFixCodeActionKind =
        vscode.CodeActionKind.QuickFix.append(DIAGNOSTICS_ID);

    private static metadata: vscode.CodeActionProviderMetadata = {
        providedCodeActionKinds: [DiagnosticsService.quickFixCodeActionKind],
    };

    private readonly diagnosticCollection: vscode.DiagnosticCollection;
    private readonly diagnosticDifferenceMap: Map<vscode.Diagnostic, Difference> = new Map();
    private readonly codeActionsProvider: vscode.Disposable;
    private readonly disposables: vscode.Disposable[] = [];

    constructor(
        private readonly formatDocumentProvider: FormatDocumentProvider,
        private readonly supportedLanguageIds: string[],
        private readonly logger: Logger,
    ) {
        this.diagnosticCollection = vscode.languages.createDiagnosticCollection(DIAGNOSTICS_ID);
        this.codeActionsProvider = vscode.languages.registerCodeActionsProvider(
            this.supportedLanguageIds,
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

    private onChangeTimeout: NodeJS.Timeout | undefined;

    private registerEditorEvents(): void {
        const activeDocument = vscode.window.activeTextEditor?.document;
        if (activeDocument) {
            void this.runDiagnostics(activeDocument);
        }

        const onDidChangeTextDocument = vscode.workspace.onDidChangeTextDocument(e => {
            if (
                e.contentChanges.length &&
                vscode.window.activeTextEditor?.document === e.document &&
                this.supportedLanguageIds.includes(e.document.languageId)
            ) {
                clearTimeout(this.onChangeTimeout);
                this.onChangeTimeout = setTimeout(() => {
                    // when editing don't pop up any new diagnostics, but if someone cleans up one then allow that update
                    void this.runDiagnostics(e.document, true);
                }, 100);
            }
        });

        const onDidSaveTextDocument = vscode.workspace.onDidSaveTextDocument(document => {
            if (vscode.window.activeTextEditor?.document === document) {
                void this.runDiagnostics(document);
            }
        });

        const onDidChangeActiveTextEditor = vscode.window.onDidChangeActiveTextEditor(editor => {
            if (editor) {
                void this.runDiagnostics(editor.document);
            }
        });

        this.disposables.push(
            onDidChangeTextDocument,
            onDidSaveTextDocument,
            onDidChangeActiveTextEditor,
            this.diagnosticCollection,
        );
    }

    public async runDiagnostics(
        document: vscode.TextDocument,
        onlyAllowLessDiagnostics = false,
    ): Promise<void> {
        const shouldRunDiagnostics =
            !!vscode.workspace.getWorkspaceFolder(document.uri) &&
            (workspace.getConfiguration("csharpier").get<boolean>("enableDiagnostics") ?? true);

        let currentDiagnostics = this.diagnosticCollection.get(document.uri);

        if (
            !shouldRunDiagnostics ||
            (currentDiagnostics?.length === 0 && onlyAllowLessDiagnostics)
        ) {
            this.diagnosticCollection.set(document.uri, []);
            return;
        }

        try {
            const source = document.getText();
            const formattedSource =
                (await this.formatDocumentProvider.formatDocument(document, false)) ?? source;
            const differences = generateDifferences(source, formattedSource);
            const diff = {
                source,
                formattedSource,
                differences,
            };
            const diagnostics = this.getDiagnostics(document, diff);
            if (onlyAllowLessDiagnostics) {
                let currentCount = !currentDiagnostics ? 0 : currentDiagnostics.length;
                if (diagnostics.length >= currentCount) {
                    return;
                }
            }
            this.diagnosticCollection.set(document.uri, diagnostics);
        } catch (error) {
            this.logger.error(`Exception while trying to provide diagnostics`, error);
        }
    }

    private getDiagnostics(
        document: vscode.TextDocument,
        diff: CsharpierDiff,
    ): vscode.Diagnostic[] {
        const diagnostics: vscode.Diagnostic[] = [];
        for (const difference of diff.differences) {
            let range = this.getRange(document, difference);
            let message = this.getMessage(difference);
            let diagnostic = new vscode.Diagnostic(range, message);
            diagnostic.source = DIAGNOSTICS_ID;
            diagnostic.code = DIAGNOSTICS_SOURCE_ID;
            diagnostic.severity = parseInt(
                workspace.getConfiguration("csharpier").get<string>("diagnosticsLevel") ?? "1",
                10,
            );
            this.diagnosticDifferenceMap.set(diagnostic, difference);
            diagnostics.push(diagnostic);
        }
        return diagnostics;
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

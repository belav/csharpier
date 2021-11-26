import { strictEqual } from "assert";
import { readFileSync, writeFileSync } from "fs";
import { commands, TextDocument, window, workspace } from "vscode";
import * as path from "path";

const unformattedCode = "public class ClassName {\n\n}";
const formattedCode = "public class ClassName { }\n";

suite("Formatting", () => {
    test("Formats physical C# file", async () => {
        const testFilePath = path.resolve(__dirname, "./TestFile.cs");
        writeFileSync(testFilePath, unformattedCode);

        await formatFile(testFilePath);
        const actual = readFileSync(testFilePath, "utf8");

        strictEqual(actual, formattedCode);
    });

    test("Formats virtual C# file", async () => {
        const document = await workspace.openTextDocument({
            content: unformattedCode,
            language: "csharp",
        });

        await showDocumentAndFormat(document);

        strictEqual(document.getText(), formattedCode);
    });

    test("Retains invalid virtual C# file", async () => {
        const document = await workspace.openTextDocument({
            content: "public class ClassName {",
            language: "csharp",
        });

        await showDocumentAndFormat(document);

        strictEqual(document.getText(), "public class ClassName {");
    });
});

const formatFile = async (fileName: string): Promise<void> => {
    const document = await workspace.openTextDocument(fileName);
    const originalCode = document.getText();

    await showDocumentAndFormat(document);
    await workspace.saveAll();
};

const showDocumentAndFormat = async (document: TextDocument): Promise<void> => {
    await window.showTextDocument(document);

    await commands.executeCommand("editor.action.formatDocument");
};

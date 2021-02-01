const fs = require("fs");
const path = require("path");
const prettier = require("prettier");

function runTest(directory, name) {
    const codePath = path.resolve(directory, name + ".cs");
    let code = fs.readFileSync(codePath, "utf8");

    const actualCode = prettier.format(code, {
        parser: "cs",
        plugins: ["."],
        endOfLine: "auto",
        writeParserJson: codePath.replace(".cs", ".json"),
        tabWidth: 4,
        printTodo: true,
    });

    const expectedFilePath = codePath.replace(".cs", ".expected.cs");
    if (fs.existsSync(expectedFilePath)) {
        code = fs.readFileSync(expectedFilePath, "utf8");
    }

    const actualFilePath = codePath.replace(".cs", ".actual.cs");
    if (!fs.existsSync(actualFilePath) || fs.readFileSync(actualFilePath, "utf8") !== actualCode) {
        fs.writeFileSync(actualFilePath, actualCode);
    }


    expect(actualCode).toBe(code);
}

module.exports = runTest;

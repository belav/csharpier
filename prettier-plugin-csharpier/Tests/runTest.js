const fs = require("fs");
const path = require("path");
const prettier = require("prettier");

function runTest(directory, name) {
    const codePath = path.resolve(directory, name + ".cs");
    const code = fs.readFileSync(codePath, "utf8");

    const actualCode = prettier.format(code, {
        parser: "cs",
        plugins: ["."],
        endOfLine: "auto",
        writeParserJson: codePath.replace(".cs", ".json"),
    });

    fs.writeFileSync(codePath.replace(".cs", ".actual.cs"), actualCode, "utf8");

    expect(actualCode).toBe(code);
}

module.exports = runTest;

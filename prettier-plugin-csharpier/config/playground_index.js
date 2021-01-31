const fs = require("fs");
const path = require("path");
const prettier = require("prettier");

const inputFile = process.argv[2];

// TODO 0 where do we really put this? Can the build rebuild the parser dotnet thing too?
const codePath = path.resolve(__dirname, inputFile);
const code = fs.readFileSync(codePath, "utf8");

const actualCode = prettier.format(code, {
    parser: "cs",
    plugins: ["./prettier-plugin-csharpier"],
    endOfLine: "auto",
    // writeParserJson: codePath.replace(".cs", ".json"),
    tabWidth: 4,
    // printTodo: true,
});

const actualFilePath = codePath.replace(".cs", ".Formatted.cs");
if (!fs.existsSync(actualFilePath) || fs.readFileSync(actualFilePath, "utf8") !== actualCode) {
    fs.writeFileSync(actualFilePath, actualCode);
}


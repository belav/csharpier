const fs = require("fs");
const path = require("path");
const prettier = require("prettier");

const file = "AllInOne.cs";

const codePath = path.resolve(__dirname, "../Samples", file);
const code = fs.readFileSync(codePath, "utf8");

const actualCode = prettier.format(code, {
    parser: "cs",
    plugins: ["./csharpier"],
    endOfLine: "auto",
    // writeParserJson: codePath.replace(".cs", ".json"),
    tabWidth: 4,
    // printTodo: true,
});

const actualFilePath = codePath.replace(".cs", ".actual.cs");
if (!fs.existsSync(actualFilePath) || fs.readFileSync(actualFilePath, "utf8") !== actualCode) {
    fs.writeFileSync(actualFilePath, actualCode);
}


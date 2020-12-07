const fs = require("fs");
const path = require("path");

const getDirectories = source =>
    fs.readdirSync(source, { withFileTypes: true })
        .filter(o => o.isDirectory())
        .map(o => o.name);

const testsDirectory = path.resolve(__dirname, "Tests");

getDirectories(testsDirectory).forEach(directoryName => {
    const fileName = "_" + directoryName[0].toLowerCase() + directoryName.substring(1) + ".js";
    let fileContent = "const runTest = require(\"../RunTest\");\n";
    fs.readdirSync(path.resolve(testsDirectory, directoryName)).forEach(file => {
        if (file.endsWith(".cs") && !file.endsWith(".actual.cs")) {
            const testName = file.replace(".cs", "");

            fileContent += "\n" +
            "test(\"" + testName + "\", () => {\n" +
            "    runTest(__dirname, \"" + testName + "\");\n" +
            "});"
        }
    });

    fs.writeFileSync(path.resolve(testsDirectory, directoryName, fileName), fileContent, "UTF8");
});



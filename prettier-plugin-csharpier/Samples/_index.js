const prettier = require("prettier");
const fs = require("fs");
const path = require("path");

const [, , sampleName] = process.argv;

fs.readdirSync(__dirname).forEach(file => {
    if (!file.endsWith(".cs") || file.endsWith(".Formatted.cs") || (sampleName && !file.startsWith(sampleName + "."))) {
        return;
    }

    const referenceFile = path.join(__dirname, file);
    const formattedFile = referenceFile.replace(".cs", ".Formatted.cs");

    const referenceCode = fs.readFileSync(referenceFile, "utf8");

    console.time(file);

    const formattedCode = prettier.format(referenceCode, {
        parser: "cs",
        plugins: ["."],
        endOfLine: "auto",
        writeParserJson: referenceFile.replace(".cs", ".json"),
    });

    console.timeEnd(file)

    fs.writeFileSync(formattedFile, formattedCode, "utf8");
});

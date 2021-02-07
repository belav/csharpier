const prettier = require("prettier");
const fs = require("fs");
const path = require("path");

const [, , sampleName] = process.argv;

fs.readdirSync(__dirname).forEach(file => {
    if (!file.endsWith(".java") || file.endsWith(".Formatted.java") || (sampleName && !file.startsWith(sampleName + "."))) {
        return;
    }

    console.log("Formatting " + file);

    const referenceFile = path.join(__dirname, file);
    const formattedFile = referenceFile.replace(".java", ".Formatted.java");

    const referenceCode = fs.readFileSync(referenceFile, "utf8");

    console.time(file);

    const formattedCode = prettier.format(referenceCode, {
        parser: "java",
        plugins: ["."],
        endOfLine: "auto",
        tabWidth: 4,
    });

    console.timeEnd(file)

    fs.writeFileSync(formattedFile, formattedCode, "utf8");
});

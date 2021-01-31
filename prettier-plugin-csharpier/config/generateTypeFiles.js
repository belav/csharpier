const path = require("path");
const fs = require("fs");

const typesDirectory = path.resolve(__dirname, "../src/Printer/Types");

const generateTypesFile = () => {
    let typesFile = "";
    fs.readdirSync(typesDirectory).forEach(typeFile => {
        typesFile += `export { print${typeFile.replace(".ts", "")} as ${typeFile.replace(
            ".ts",
            "",
        )} } from "./Types/${typeFile.replace(".ts", "")}"\r\n`;
    });

    const typesFilePath = typesDirectory + ".ts";

    if (!fs.existsSync(typesFilePath) || fs.readFileSync(typesFilePath, "utf8") !== typesFile) {
        fs.writeFileSync(typesFilePath, typesFile);
    }
};

module.exports = {
    watch: function () {
        fs.watch(typesDirectory, () => {
            generateTypesFile();
        });
    },
    generate: function () {
        generateTypesFile();
    }


}

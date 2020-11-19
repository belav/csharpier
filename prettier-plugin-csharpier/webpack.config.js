const path = require("path");
const fs = require("fs");

const typesDirectory = path.resolve(__dirname, "src/Printer/Types");

generateTypesFile = () => {
    let typesFile = "";
    fs.readdirSync(typesDirectory).forEach(typeFile => {
        typesFile += `export { print as ${typeFile.replace(".ts", "")} } from "./Types/${typeFile.replace(".ts", "")}"\r\n`;
    })

    const typesFilePath = path.resolve(__dirname, "src/Printer/Types.ts")

    if (!fs.existsSync(typesFilePath) || fs.readFileSync(typesFilePath, "utf8") !== typesFile) {
        fs.writeFileSync(typesFilePath, typesFile);
    }
}

fs.watch(typesDirectory, () => {
    generateTypesFile();
})

generateTypesFile();

module.exports = env => {
    return {
        mode: "development",
        watch: env && env.watch,
        watchOptions: {
            ignored: /node_modules/
        },
        entry: [
            "./src/index.ts"
        ],
        output: {
            path: path.resolve(__dirname, "dist"),
            filename: "index.js",
            libraryTarget: "umd",
            library: "csharpier",
            umdNamedDefine: true,
        },
        resolve: {
            extensions: [".ts", ".tsx", ".js"],
        },
        devtool: "inline-source-map",
        plugins: [],
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    include: path.resolve(__dirname, "src"),
                    use: ["babel-loader", "awesome-typescript-loader"],
                },
            ],
        },
        target: "node",
        externals: {
            "prettier": {
                commonjs: "prettier",
                commonjs2: "prettier",
                amd: "prettier",
            },
            "edge-js": {
                commonjs: "edge-js",
                commonjs2: "edge-js",
                amd: "edge-js",
            }
        }
    }
};

const path = require("path");
const fs = require("fs");

const generateTypeFiles = require("./generateTypeFiles");

generateTypeFiles.generate();

const outputPath = path.resolve(__dirname, "build/prettier-plugin-csharpier");

const parserPath = path.resolve(__dirname, "../CSharpier/CSharpier.Parser/bin/Release/net5.0");

copyParserDll = () => {
    const folderForDll = path.resolve(outputPath, "net5.0");
    if (!fs.existsSync(folderForDll)) {
        fs.mkdirSync(folderForDll, { recursive: true });
    }
    const filesToCopy = [
        "Microsoft.CodeAnalysis.CSharp.dll",
        "Microsoft.CodeAnalysis.dll",
        "Newtonsoft.Json.dll",
        "CSharpier.Parser.deps.json",
        "CSharpier.Parser.dll",
        "CSharpier.Parser.runtimeconfig.json"
    ];
    for (const file of filesToCopy) {
        fs.copyFileSync(path.resolve(parserPath, file), path.resolve(folderForDll, file));
    }
};

fs.watch(parserPath + "/Parser.dll", () => {
    copyParserDll();
});

copyParserDll();

module.exports = {
    mode: "production",
    entry: ["./src/Index.ts"],
    output: {
        path: outputPath,
        filename: "index.js",
        libraryTarget: "umd",
        library: "csharpier",
        umdNamedDefine: true,
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"],
    },
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
        prettier: {
            commonjs: "prettier",
            commonjs2: "prettier",
            amd: "prettier",
        },
        "edge-js": {
            commonjs: "edge-js",
            commonjs2: "edge-js",
            amd: "edge-js",
        },
    },
};

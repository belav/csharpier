const path = require("path");
const webpack = require("webpack");
const fs = require("fs");
const generateTests = require("../Tests/generateTests");
const generateTypeFiles = require("./generateTypeFiles");

generateTests.watch();
generateTypeFiles.watch();

const parserPath = "../../../CSharpier/CSharpier.Parser/bin/Debug/net5.0/CSharpier.Parser.dll";
fs.copyFileSync(path.resolve(__dirname, "playground_index.js"), path.resolve(__dirname, "../dist/index.js"));

module.exports = {
    mode: "development",
    watch: true,
    watchOptions: {
        ignored: /node_modules/,
    },
    entry: [path.resolve(__dirname, "../src/Index.ts")],
    output: {
        path: path.resolve(__dirname, "../dist/prettier-plugin-csharpier"),
        filename: "index.js",
        libraryTarget: "umd",
        library: "csharpier",
        umdNamedDefine: true,
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"],
    },
    devtool: "inline-source-map",
    plugins: [
        new webpack.DefinePlugin({
            PARSER_PATH: JSON.stringify(parserPath),
        }),
    ],
    module: {
        rules: [
            {
                test: /\.ts$/,
                include: path.resolve(__dirname, "../src"),
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

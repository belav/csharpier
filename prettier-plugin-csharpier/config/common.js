const fs = require("fs");
const path = require("path");
const webpack = require("webpack");

module.exports = {
    setupCommonConfig: function (parserPath) {
        const distPath = path.resolve(__dirname, "../dist");

        if (!fs.existsSync(distPath)) {
            fs.mkdirSync(distPath);
        }
        fs.copyFileSync(path.resolve(__dirname, "playground_index.js"), path.resolve(distPath, "index.js"));

        return {
            entry: [path.resolve(__dirname, "../src/Index.ts")],
            output: {
                path: path.resolve(distPath, "prettier-plugin-csharpier"),
                filename: "index.js",
                libraryTarget: "umd",
                library: "csharpier",
                umdNamedDefine: true,
            },
            resolve: {
                extensions: [".ts", ".tsx", ".js"],
            },
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
            }
        }
    }
};

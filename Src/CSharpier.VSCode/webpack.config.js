let webpack = require("webpack");
let path = require("path");
let extensionPackage = require("./package.json");

module.exports = env => {
    return {
        mode: env.NODE_ENV,
        target: "node",
        entry: "./src/Extension.ts",
        output: {
            path: path.resolve(__dirname, "build"),
            filename: "Extension.js",
            libraryTarget: "commonjs2",
            devtoolModuleFilenameTemplate: "../[resource-path]",
        },
        plugins: [
            new webpack.EnvironmentPlugin({
                EXTENSION_NAME: extensionPackage.name,
                EXTENSION_VERSION: extensionPackage.version,
                MODE: env.NODE_ENV,
            }),
        ],
        devtool: "source-map",
        externals: {
            vscode: "commonjs vscode",
            prettier: "commonjs prettier",
        },
        resolve: {
            extensions: [".ts", ".js"],
        },
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    exclude: /node_modules/,
                    use: [
                        {
                            loader: "ts-loader",
                        },
                    ],
                },
            ],
        },
    };
};

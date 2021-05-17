const { resolve, join } = require("path");
const { CheckerPlugin } = require("awesome-typescript-loader");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");

module.exports = {
    output: {
        filename: "[name].js",
        path: resolve(__dirname, "dist"),
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js", ".jsx"],
        plugins: [new TsconfigPathsPlugin({ configFile: "tsconfig.json" })],
    },
    context: resolve(__dirname, "../../src"),
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: ["style-loader", "css-loader"],
            },
            {
                test: /\.js$/,
                use: ["babel-loader", "source-map-loader"],
                exclude: /node_modules/,
            },
            {
                test: /\.tsx?$/,
                use: ["babel-loader", "awesome-typescript-loader"],
            },
        ],
    },
    plugins: [new CheckerPlugin(),
    new HtmlWebpackPlugin({
        hash: true,
        template: "./index.html"
    })],
    performance: {
        hints: false,
    },
};

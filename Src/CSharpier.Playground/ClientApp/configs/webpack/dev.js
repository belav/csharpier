const merge = require("webpack-merge");
const webpack = require("webpack");
const commonConfig = require("./common");
const path = require("path");

module.exports = merge(commonConfig, {
    mode: "development",
    entry: [
        "react-hot-loader/patch", // activate HMR for React
        "webpack-dev-server/client?https://0.0.0.0/",// bundle the client for webpack-dev-server and connect to the provided endpoint
        "webpack/hot/only-dev-server", // bundle the client for hot reloading, only- means to only hot reload for successful updates
        "./index.tsx" // the entry point of our app
    ],
    devServer: {
        contentBase: "./dist",
        open: false,
        hot: true,
        disableHostCheck: true,
        host: "0.0.0.0",
        port: 7010,
        injectClient: false,
    },
    devtool: "cheap-module-eval-source-map",
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new webpack.DefinePlugin({
            IS_PRODUCTION: false
        }),
    ],
});

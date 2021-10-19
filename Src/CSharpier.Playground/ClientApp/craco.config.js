const SpeedMeasurePlugin = require("speed-measure-webpack-plugin");

module.exports = {
    eslint: {
        enable: false
    },
    // plugins: [
    //     {
    //         plugin: {
    //             overrideWebpackConfig: ({ webpackConfig }) => {
    //                 // webpackConfig.plugins = webpackConfig.plugins.filter(
    //                 //     (plugin) => plugin.constructor.name !== "CaseSensitivePathsPlugin"
    //                 // );
    //                 //
    //                 // webpackConfig.plugins = webpackConfig.plugins.filter(
    //                 //     (plugin) => plugin.constructor.name !== "IgnorePlugin"
    //                 // );
    //
    //                 const speedMeasurePlugin = new SpeedMeasurePlugin({
    //                     outputFormat: "humanVerbose",
    //                     loaderTopFiles: 5,
    //                 });
    //
    //                 return speedMeasurePlugin.wrap(webpackConfig);
    //             },
    //         }
    //     }
    // ]
};
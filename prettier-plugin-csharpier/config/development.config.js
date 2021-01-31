const merge = require("webpack-merge");
const generateTests = require("../Tests/generateTests");
const generateTypeFiles = require("./generateTypeFiles");
const { setupCommonConfig } = require("./common");

generateTests.watch();
generateTypeFiles.watch();

const parserPath = "../../../CSharpier/CSharpier.Parser/bin/Debug/net5.0/CSharpier.Parser.dll";

const commonConfig = setupCommonConfig(parserPath)

const developmentConfig = {
    mode: "development",
    watch: true,
    watchOptions: {
        ignored: /node_modules/,
    },
    devtool: "inline-source-map",
};

module.exports = merge(commonConfig, developmentConfig);

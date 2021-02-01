const path = require("path");
const merge = require("webpack-merge");
const { setupCommonConfig } = require("./common");
const generateTypeFiles = require("./generateTypeFiles");

generateTypeFiles.generate();

const parserPath = path.resolve(__dirname, "../CSharpier.Parser/net5.0/CSharpier.Parser.dll");
const commonConfig = setupCommonConfig(parserPath);

const productionConfig = {
    mode: "production",
};

module.exports = merge(commonConfig, productionConfig);

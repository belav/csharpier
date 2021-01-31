const path = require("path");
const merge = require("webpack-merge");
const { setupCommonConfig } = require("./common");
const generateTypeFiles = require("./generateTypeFiles");

generateTypeFiles.generate();

const parserPath = path.resolve(__dirname, "../CSharpier/CSharpier.Parser/bin/Release/net5.0");
const commonConfig = setupCommonConfig(parserPath);

// TODO 0 have to copy everything from dist into the publish/Prettier for the github action
// then run npm install in that folder

const productionConfig = {
    mode: "production",
};

module.exports = merge(commonConfig, productionConfig);

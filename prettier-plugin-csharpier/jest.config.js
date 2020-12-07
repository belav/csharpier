// For a detailed explanation regarding each configuration property, visit:
// https://jestjs.io/docs/en/configuration.html

module.exports = {
    testPathIgnorePatterns: ["/node_modules/", "/runTest.js", "/generateTests.js"],
    testMatch: ["**/Tests/**/*.js"]
};

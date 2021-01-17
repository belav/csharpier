const runTest = require("../RunTest");

test("BasicUsingDirective", () => {
    runTest(__dirname, "BasicUsingDirective");
});
test("UsingWithAlias", () => {
    runTest(__dirname, "UsingWithAlias");
});
test("UsingWithStatic", () => {
    runTest(__dirname, "UsingWithStatic");
});
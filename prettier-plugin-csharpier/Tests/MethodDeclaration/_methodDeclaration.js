const runTest = require("../RunTest");

test("BasicMethod", () => {
    runTest(__dirname, "BasicMethod");
});
test("LongMethodWithParameters", () => {
    runTest(__dirname, "LongMethodWithParameters");
});
test("MethodWithParameters", () => {
    runTest(__dirname, "MethodWithParameters");
});
test("MethodWithStatements", () => {
    runTest(__dirname, "MethodWithStatements");
});
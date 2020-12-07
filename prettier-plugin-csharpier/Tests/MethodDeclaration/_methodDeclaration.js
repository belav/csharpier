const runTest = require("../RunTest");

test("BasicMethod", () => {
    runTest(__dirname, "BasicMethod");
});
test("MethodWithStatements", () => {
    runTest(__dirname, "MethodWithStatements");
});
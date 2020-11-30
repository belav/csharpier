const runTest = require("../RunTest");

test("Basic method", () => {
    runTest(__dirname, "BasicMethod");
});

test("Method with statements", () => {
    runTest(__dirname, "MethodWithStatements");
});

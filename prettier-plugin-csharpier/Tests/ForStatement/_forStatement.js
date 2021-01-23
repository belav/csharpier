const runTest = require("../RunTest");

test("BasicForStatement", () => {
    runTest(__dirname, "BasicForStatement");
});
test("EmptyForStatement", () => {
    runTest(__dirname, "EmptyForStatement");
});
test("ForStatementNoBraces", () => {
    runTest(__dirname, "ForStatementNoBraces");
});
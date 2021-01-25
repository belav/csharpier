const runTest = require("../RunTest");

test("BasicTryStatement", () => {
    runTest(__dirname, "BasicTryStatement");
});
test("TryStatementWithNoCatchDeclaration", () => {
    runTest(__dirname, "TryStatementWithNoCatchDeclaration");
});
test("TryStatementWithWhen", () => {
    runTest(__dirname, "TryStatementWithWhen");
});
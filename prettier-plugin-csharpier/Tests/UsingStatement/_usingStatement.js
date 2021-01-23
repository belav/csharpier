const runTest = require("../RunTest");

test("BasicUsingStatement", () => {
    runTest(__dirname, "BasicUsingStatement");
});
test("NestedUsingStatement", () => {
    runTest(__dirname, "NestedUsingStatement");
});
test("UsingStatementWithExpression", () => {
    runTest(__dirname, "UsingStatementWithExpression");
});
test("UsingStatementWithNoBody", () => {
    runTest(__dirname, "UsingStatementWithNoBody");
});
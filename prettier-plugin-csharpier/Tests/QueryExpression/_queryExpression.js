const runTest = require("../RunTest");

test("BasicQueryExpression", () => {
    runTest(__dirname, "BasicQueryExpression");
});
test("QueryWithGroup", () => {
    runTest(__dirname, "QueryWithGroup");
});
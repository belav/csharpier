const runTest = require("../RunTest");

test("BasicQueryExpression", () => {
    runTest(__dirname, "BasicQueryExpression");
});
test("ComplexQuery", () => {
    runTest(__dirname, "ComplexQuery");
});
test("QueryWithGroup", () => {
    runTest(__dirname, "QueryWithGroup");
});
test("QueryWithSelectInto", () => {
    runTest(__dirname, "QueryWithSelectInto");
});
test("QueryWithWhere", () => {
    runTest(__dirname, "QueryWithWhere");
});
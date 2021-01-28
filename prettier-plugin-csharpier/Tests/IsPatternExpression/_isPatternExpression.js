const runTest = require("../RunTest");

test("BasicIsPatternExpression", () => {
    runTest(__dirname, "BasicIsPatternExpression");
});
test("RecursivePattern", () => {
    runTest(__dirname, "RecursivePattern");
});
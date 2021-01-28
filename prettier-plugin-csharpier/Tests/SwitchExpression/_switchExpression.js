const runTest = require("../RunTest");

test("BasicSwitchExpression", () => {
    runTest(__dirname, "BasicSwitchExpression");
});
test("SwitchWithRecursivePattern", () => {
    runTest(__dirname, "SwitchWithRecursivePattern");
});
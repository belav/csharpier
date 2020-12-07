const runTest = require("../RunTest");

test("BasicProperty", () => {
    runTest(__dirname, "BasicProperty");
});
test("PropertyWithBackingValue", () => {
    runTest(__dirname, "PropertyWithBackingValue");
});
test("PropertyWithLambdaAccessors", () => {
    runTest(__dirname, "PropertyWithLambdaAccessors");
});
test("PropertyWithLambdaBody", () => {
    runTest(__dirname, "PropertyWithLambdaBody");
});
test("PropertyWithThisExpression", () => {
    runTest(__dirname, "PropertyWithThisExpression");
});
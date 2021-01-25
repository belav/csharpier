const runTest = require("../RunTest");

test("BasicProperty", () => {
    runTest(__dirname, "BasicProperty");
});
test("LineBreaks", () => {
    runTest(__dirname, "LineBreaks");
});
test("PropertyModifiers", () => {
    runTest(__dirname, "PropertyModifiers");
});
test("PropertyWithBackingValue", () => {
    runTest(__dirname, "PropertyWithBackingValue");
});
test("PropertyWithInitializer", () => {
    runTest(__dirname, "PropertyWithInitializer");
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
const runTest = require("../RunTest");

test("Basic property", () => {
    runTest(__dirname, "BasicProperty");
});

test("Property with backing value", () => {
    runTest(__dirname, "PropertyWithBackingValue");
});

test("Property with this expression", () => {
    runTest(__dirname, "PropertyWithThisExpression");
});

test("Property with lambda body", () => {
    runTest(__dirname, "PropertyWithLambdaBody");
});

test("Property with lambda accessors", () => {
    runTest(__dirname, "PropertyWithLambdaAccessors");
});

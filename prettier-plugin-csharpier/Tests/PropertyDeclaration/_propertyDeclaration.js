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

test("Property with lambda", () => {
    runTest(__dirname, "PropertyWithLambda");
});

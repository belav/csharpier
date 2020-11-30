const runTest = require("../RunTest");

test("Basic property", () => {
    runTest(__dirname, "BasicProperty");
});

test("Property with backing value", () => {
    runTest(__dirname, "PropertyWithBackingValue");
});

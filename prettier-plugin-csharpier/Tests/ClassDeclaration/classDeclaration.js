const runTest = require("../RunTest");

test("Empty class formats correctly", () => {
    runTest(__dirname, "EmptyClass");
});

test("Static Abstract class formats correctly", () => {
    runTest(__dirname, "StaticAbstractClass");
});

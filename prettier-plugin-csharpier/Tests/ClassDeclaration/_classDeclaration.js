const runTest = require("../RunTest");

test("Empty class", () => {
    runTest(__dirname, "EmptyClass");
});

test("Static abstract class", () => {
    runTest(__dirname, "StaticAbstractClass");
});

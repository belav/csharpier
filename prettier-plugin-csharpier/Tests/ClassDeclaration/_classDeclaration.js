const runTest = require("../RunTest");

test("EmptyClass", () => {
    runTest(__dirname, "EmptyClass");
});
test("StaticAbstractClass", () => {
    runTest(__dirname, "StaticAbstractClass");
});
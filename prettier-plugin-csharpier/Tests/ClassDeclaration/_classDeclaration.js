const runTest = require("../RunTest");

test("ClassImplementsInterface", () => {
    runTest(__dirname, "ClassImplementsInterface");
});
test("EmptyClass", () => {
    runTest(__dirname, "EmptyClass");
});
test("StaticAbstractClass", () => {
    runTest(__dirname, "StaticAbstractClass");
});
const runTest = require("../RunTest");

test("ClassImplementsInterface", () => {
    runTest(__dirname, "ClassImplementsInterface");
});
test("EmptyClass", () => {
    runTest(__dirname, "EmptyClass");
});
test("LongBaseListFormatsOnLines", () => {
    runTest(__dirname, "LongBaseListFormatsOnLines");
});
test("MultipleClassesWithModifiers", () => {
    runTest(__dirname, "MultipleClassesWithModifiers");
});
test("MultipleClassesWithoutModifiers", () => {
    runTest(__dirname, "MultipleClassesWithoutModifiers");
});
test("StaticAbstractClass", () => {
    runTest(__dirname, "StaticAbstractClass");
});
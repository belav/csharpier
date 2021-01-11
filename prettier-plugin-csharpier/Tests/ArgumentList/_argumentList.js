const runTest = require("../RunTest");

test("BasicArgumentList", () => {
    runTest(__dirname, "BasicArgumentList");
});
test("EmptyArgumentListStaysOnSameLine", () => {
    runTest(__dirname, "EmptyArgumentListStaysOnSameLine");
});
test("LongArgumentList", () => {
    runTest(__dirname, "LongArgumentList");
});
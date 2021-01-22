const runTest = require("../RunTest");

test("BasicMethod", () => {
    runTest(__dirname, "BasicMethod");
});
test("ExplicityInterfaceSpecifierMethod", () => {
    runTest(__dirname, "ExplicityInterfaceSpecifierMethod");
});
test("LongMethodWithParameters", () => {
    runTest(__dirname, "LongMethodWithParameters");
});
test("MethodWithParameters", () => {
    runTest(__dirname, "MethodWithParameters");
});
test("MethodWithStatements", () => {
    runTest(__dirname, "MethodWithStatements");
});
const runTest = require("../RunTest");

test("BasicConstructorDeclaration", () => {
    runTest(__dirname, "BasicConstructorDeclaration");
});
test("ConstructorWithParameters", () => {
    runTest(__dirname, "ConstructorWithParameters");
});
test("LongConstructor", () => {
    runTest(__dirname, "LongConstructor");
});
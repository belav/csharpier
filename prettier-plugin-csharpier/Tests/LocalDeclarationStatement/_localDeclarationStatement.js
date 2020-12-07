const runTest = require("../RunTest");

test("VariableWithInitializer", () => {
    runTest(__dirname, "VariableWithInitializer");
});
test("VariableWithoutInitializer", () => {
    runTest(__dirname, "VariableWithoutInitializer");
});
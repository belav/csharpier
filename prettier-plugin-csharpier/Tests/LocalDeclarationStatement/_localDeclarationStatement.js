const runTest = require("../RunTest");

test("Variable with initializer", () => {
    runTest(__dirname, "VariableWithInitializer");
});

test("Variable without initializer", () => {
    runTest(__dirname, "VariableWithoutInitializer");
});

const runTest = require("../RunTest");

test("Basic field formats correctly", () => {
    runTest(__dirname, "BasicField");
});

test("Namespaced field formats correctly", () => {
    runTest(__dirname, "NamespacedField");
});

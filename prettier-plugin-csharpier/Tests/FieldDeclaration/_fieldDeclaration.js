const runTest = require("../RunTest");

test("Basic field", () => {
    runTest(__dirname, "BasicField");
});

test("Namespaced field", () => {
    runTest(__dirname, "NamespacedField");
});

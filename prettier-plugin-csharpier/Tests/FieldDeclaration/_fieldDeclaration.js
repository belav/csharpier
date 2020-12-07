const runTest = require("../RunTest");

test("BasicField", () => {
    runTest(__dirname, "BasicField");
});
test("NamespacedField", () => {
    runTest(__dirname, "NamespacedField");
});
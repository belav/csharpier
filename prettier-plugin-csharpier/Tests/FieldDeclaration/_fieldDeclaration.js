const runTest = require("../RunTest");

test("BasicField", () => {
    runTest(__dirname, "BasicField");
});
test("FixedFieldWithSize", () => {
    runTest(__dirname, "FixedFieldWithSize");
});
test("NamespacedField", () => {
    runTest(__dirname, "NamespacedField");
});
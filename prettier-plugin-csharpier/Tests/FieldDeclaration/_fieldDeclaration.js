const runTest = require("../RunTest");

test("BasicField", () => {
    runTest(__dirname, "BasicField");
});
test("FieldDeclarationComments", () => {
    runTest(__dirname, "FieldDeclarationComments");
});
test("FixedFieldWithSize", () => {
    runTest(__dirname, "FixedFieldWithSize");
});
test("NamespacedField", () => {
    runTest(__dirname, "NamespacedField");
});
const runTest = require("../RunTest");

test("EmptyNamespace", () => {
    runTest(__dirname, "EmptyNamespace");
});
test("NamespaceWithMultipleClasses", () => {
    runTest(__dirname, "NamespaceWithMultipleClasses");
});
test("NamespaceWithUsingAndClass", () => {
    runTest(__dirname, "NamespaceWithUsingAndClass");
});
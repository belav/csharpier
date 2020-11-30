const runTest = require("../RunTest");

test("Empty namespace", () => {
    runTest(__dirname, "EmptyNamespace");
});

test("Namespace With Using And Class", () => {
    runTest(__dirname, "NamespaceWithUsingAndClass");
});

test("Namespace With Multiple Classes", () => {
    runTest(__dirname, "NamespaceWithMultipleClasses");
})

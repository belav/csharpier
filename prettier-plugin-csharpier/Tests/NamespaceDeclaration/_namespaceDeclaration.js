const runTest = require("../RunTest");

test("Empty namespace", () => {
    runTest(__dirname, "EmptyNamespace");
});

test("Namespace With Using And Class", () => {
    runTest(__dirname, "NamespaceWithUsingAndClass");
})

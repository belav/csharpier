const runTest = require("../RunTest");

test("BasicInterfaceDeclaration", () => {
    runTest(__dirname, "BasicInterfaceDeclaration");
});
test("InterfaceWithBaseList", () => {
    runTest(__dirname, "InterfaceWithBaseList");
});
test("InterfaceWithMethod", () => {
    runTest(__dirname, "InterfaceWithMethod");
});
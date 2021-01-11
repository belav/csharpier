const runTest = require("../RunTest");

test("BasicAnonymousObjectCreationExpression", () => {
    runTest(__dirname, "BasicAnonymousObjectCreationExpression");
});
test("MultipleProperties", () => {
    runTest(__dirname, "MultipleProperties");
});
test("NoNames", () => {
    runTest(__dirname, "NoNames");
});
const runTest = require("../RunTest");

test("BasicObjectCreationExpression", () => {
    runTest(__dirname, "BasicObjectCreationExpression");
});
test("ObjectCreationWithInitializer", () => {
    runTest(__dirname, "ObjectCreationWithInitializer");
});
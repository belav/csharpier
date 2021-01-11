const runTest = require("../RunTest");

test("BasicObjectCreationExpression", () => {
    runTest(__dirname, "BasicObjectCreationExpression");
});
test("ObjectCreationWithBiggerInitializer", () => {
    runTest(__dirname, "ObjectCreationWithBiggerInitializer");
});
test("ObjectCreationWithInitializer", () => {
    runTest(__dirname, "ObjectCreationWithInitializer");
});
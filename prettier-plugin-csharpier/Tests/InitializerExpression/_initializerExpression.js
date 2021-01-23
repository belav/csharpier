const runTest = require("../RunTest");

test("BasicArrayInitializerExpression", () => {
    runTest(__dirname, "BasicArrayInitializerExpression");
});
test("BasicCollectionInitializerExpression", () => {
    runTest(__dirname, "BasicCollectionInitializerExpression");
});
test("BasicComplexElementInitializerExpression", () => {
    runTest(__dirname, "BasicComplexElementInitializerExpression");
});
test("LongItemsArrayInitializer", () => {
    runTest(__dirname, "LongItemsArrayInitializer");
});
const runTest = require("../RunTest");

test("ClassComments", () => {
    runTest(__dirname, "ClassComments");
});
test("ClassComments.expected", () => {
    runTest(__dirname, "ClassComments.expected");
});
test("MethodComments", () => {
    runTest(__dirname, "MethodComments");
});
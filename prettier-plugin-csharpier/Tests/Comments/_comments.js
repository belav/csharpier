const runTest = require("../RunTest");

test("ClassComments", () => {
    runTest(__dirname, "ClassComments");
});
test("MethodComments", () => {
    runTest(__dirname, "MethodComments");
});
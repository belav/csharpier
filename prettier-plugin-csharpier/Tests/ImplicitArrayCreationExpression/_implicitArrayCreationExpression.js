const runTest = require("../RunTest");

test("BasicImplicitArrayCreationExpression", () => {
    runTest(__dirname, "BasicImplicitArrayCreationExpression");
});
test("ImplicityArrayWithCommas", () => {
    runTest(__dirname, "ImplicityArrayWithCommas");
});
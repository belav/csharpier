const runTest = require("../RunTest");

test("BasicInterpolatedStringExpression", () => {
    runTest(__dirname, "BasicInterpolatedStringExpression");
});
test("Interpolation", () => {
    runTest(__dirname, "Interpolation");
});
test("InterpolationWithAlignmentAndFormat", () => {
    runTest(__dirname, "InterpolationWithAlignmentAndFormat");
});
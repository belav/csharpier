const runTest = require("../RunTest");

test("BasicSwitchStatement", () => {
    runTest(__dirname, "BasicSwitchStatement");
});
test("DefaultWithNoBraces", () => {
    runTest(__dirname, "DefaultWithNoBraces");
});
test("EmptySwitchStatement", () => {
    runTest(__dirname, "EmptySwitchStatement");
});
test("GotoStatements", () => {
    runTest(__dirname, "GotoStatements");
});
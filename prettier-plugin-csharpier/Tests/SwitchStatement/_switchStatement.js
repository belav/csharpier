const runTest = require("../RunTest");

test("BasicSwitchStatement", () => {
    runTest(__dirname, "BasicSwitchStatement");
});
test("EmptySwitchStatement", () => {
    runTest(__dirname, "EmptySwitchStatement");
});
test("GotoStatements", () => {
    runTest(__dirname, "GotoStatements");
});
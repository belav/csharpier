const runTest = require("../RunTest");

test("AndIfStatement", () => {
    runTest(__dirname, "AndIfStatement");
});
test("BasicIfStatement", () => {
    runTest(__dirname, "BasicIfStatement");
});
test("EqualsIfStatement", () => {
    runTest(__dirname, "EqualsIfStatement");
});
test("LogicalNotIfStatement", () => {
    runTest(__dirname, "LogicalNotIfStatement");
});
test("NotEqualsIfStatement", () => {
    runTest(__dirname, "NotEqualsIfStatement");
});
test("OrIfStatement", () => {
    runTest(__dirname, "OrIfStatement");
});
test("ParenthesizedIfStatement", () => {
    runTest(__dirname, "ParenthesizedIfStatement");
});
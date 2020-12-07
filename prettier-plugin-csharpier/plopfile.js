module.exports = function (plop) {
    plop.setGenerator("node", {
        prompts: [
            {
                type: "input",
                name: "name",
                message: "name?"
            }
        ],
        actions: [
            {
                type: "add",
                path: "src/Printer/Types/{{name}}.ts",
                templateFile: "Templates/NodeType/Type.hbs"
            },
        ]
    });
    plop.setGenerator("test", {
        prompts: [
            {
                type: "input",
                name: "name",
                message: "name?"
            }
        ],
        actions: [
            {
                type: "add",
                path: "Tests/{{name}}/_{{camelCase name}}.js",
                templateFile: "Templates/NodeType/Test.hbs"
            },
        ]
    });
};

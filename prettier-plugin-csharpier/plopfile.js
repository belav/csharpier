module.exports = function (plop) {
    plop.setGenerator("nodeType", {
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
            {
                type: "add",
                path: "Tests/{{name}}/_{{camelCase name}}.js",
                templateFile: "Templates/NodeType/Test.hbs"
            }
        ]
    });
};

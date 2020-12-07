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
                path: "Tests/{{name}}/Basic{{name}}.cs",
                templateFile: "Templates/NodeType/Test.hbs"
            },
        ]
    });
};

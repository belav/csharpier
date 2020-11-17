module.exports = function (plop) {
    plop.setGenerator("nodeType", {
        prompts: [
            {
                type: "input",
                name: "name",
                message: "name?",
            },
        ],

        actions: [
            {
                type: "add",
                path: "src/Printer/Types/{{name}}.ts",
                templateFile: "Templates/NodeType.hbs",
            },
        ],
    });
};

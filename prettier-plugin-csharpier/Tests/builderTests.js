const prettier = require('prettier')
const { concat, group, join, line, softline, hardline, indent } = prettier.doc.builders

test("basic test", () => {
    const actual = print(concat(["public", " ", "class", " ", "ClassName"]));
    expect(actual).toBe("public class ClassName");
});

test("property test", () => {
    const actual = print(group(concat(["public", " ", "string", " ", "Property", " ", "{", " ", "get;", line, "set;", " ", "}"])));
    expect(actual).toBe("public string Property { get; set; }");
});

test("indent using", () => {
    const parts = [];
    parts.push("namespace Namespace");
    parts.push(hardline);
    parts.push("{");
    parts.push(indent(concat([hardline, "using One;", hardline, "using Two;"])));
    parts.push(hardline);
    parts.push("}");

    const actual = print(concat(parts));
    expect(actual).toBe(`namespace Namespace
{
    using One;
    using Two;
}`);
});

test("indent numbers", () => {
    const doc = group(
        concat([
            '[',
            indent(
                concat([
                    hardline,
                    join(concat([',', hardline]), ["1", "2", "3"])
                ])
            ),
            hardline,
            ']'
        ])
    );

    const actual = print(doc);
    expect(actual).toBe(`[
    1,
    2,
    3
]`);
});

function print(doc) {
    const result = prettier.doc.printer.printDocToString(doc, {
        tabWidth: 4,
        printWidth: 80,
    });
    return result.formatted;
}

let gutters: any[] = [];
let marks: any[] = [];
let editor: any = undefined;

export const formatCode = async (code: string) => {
    const makeRequest = async () => {
        const response = await fetch("/Format", {
            method: "POST",
            body: JSON.stringify(code),
            headers: {
                "Content-Type": "application/json",
                "cache-control": "no-cache",
            },
        });
        if (response.status === 200) {
            const data = await response.json();

            setTimeout(() => {
                setupMarks(data.errors);
            }, 100);

            return {
                syntaxTree: JSON.parse(data.json),
                formattedCode: data.code,
                doc: data.doc,
                hasErrors: !!data.errors.length,
                syntaxValidation: data.syntaxValidation
            };
        } else {
            const text = await response.text();
            return {
                formattedCode: text,
                doc: text,
                hasErrors: true,
            };
        }
    };

    for (let x = 0; x < 20; x++) {
        try {
            return makeRequest();
        } catch {
            await sleep(500);
        }
    }
    return makeRequest();
};

function sleep(milliseconds: number) {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
}

const setupMarks = (errors: any[]) => {
    if (!editor) {
        setTimeout(() => {
            setupMarks(errors);
        }, 100);
        return;
    }

    for (const gutter of gutters) {
        editor.setGutterMarker(gutter, "gutter-errors");
    }

    gutters = [];

    for (const mark of marks) {
        mark.clear();
    }
    marks = [];
    for (const error of errors) {
        const from = {
            line: error.lineSpan.startLinePosition.line,
            ch: error.lineSpan.startLinePosition.character,
        };
        const to = {
            line: error.lineSpan.endLinePosition.line,
            ch: error.lineSpan.endLinePosition.character,
        };

        const msg = document.createElement("div");
        const icon = msg.appendChild(document.createElement("span"));
        icon.innerHTML = "X";
        const title = msg.appendChild(document.createElement("div"));
        title.innerHTML = error.description;
        msg.className = "errorGutter";
        gutters.push(editor.setGutterMarker(from.line, "gutter-errors", msg));

        const options = {
            className: "compilationError",
            title: error.description,
        };
        marks.push(editor.markText(from, to, options));
    }
};

export const setFormattedCodeEditor = (value: any) => {
    editor = value;
};

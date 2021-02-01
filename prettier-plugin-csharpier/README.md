This is currently in development. There is a good chance it will produce code that doesn't compile and lose parts of files.

## Development
You'll first need to build the ../CSharpier/CSharpier.Parser .net solution

Then

```
npm install
```

Run the following to build the plugin and set up watchers to rebuild as things change
```
npm run start
```

There are two main ways to test/develop

### Samples
Files may be added to ./Samples and they can be formatted with the following command

```
npm run samples
or
npm run samples [FileNameWithoutExtension]
```

### Tests
```
npm run test
```

#### Adding a new test
- Add a new file at ./Tests/[NodeType]/[NameOfTest].cs
- if using npm run start, ./Tests/[NodeType]/_[nodeType].js will be auto updated
- otherwise run node ./Tests/generateTests.js

## Useful Info
- https://sharplab.io/ - Use to get an interactive view of the SyntaxTree for some c#
- plop node [NodeName] - Use to add a new node file
- https://dev.to/fvictorio/how-to-write-a-plugin-for-prettier-6gi - explains basics of prettier plugin

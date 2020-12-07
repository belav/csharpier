This is currently in development. There is a good chance it will produce code that doesn't compile and lose parts of files.

## Development
You'll first need to build the ../Parser .net solution

Then

```
npm run install
```

Run the following to build the plugin and set up watchers to rebuild as things change
```
npm run start
```

You will need to build the c# project for this to work

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

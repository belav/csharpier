# CSharpier

This is a prettier plugin for csharp. It uses a simple .net core console application to use Roslyn to generate the syntax tree

## Development
You'll first need to build the Parser .net solution at Parser/Parser.sln

Then from prettier-plugin-csharpier

```
npm install
```

From there you can test out samples
```
npm run samples
```
Or an individual sample
```
npm run samples [NameOfFile]
```

You can also use jest to run any tests in /Tests

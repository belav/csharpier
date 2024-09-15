This is used to scan code files using SyntaxWalkers to do things like
1. See if C# devs prefer spaces or tabs
2. See if devs prefer always braces or no braces
3. See if C# devs add empty lines into object initializers
4. Find files with specific types of SyntaxNodes
5. Print examples of how specific types of Syntax were formatted

It isn't used often enough to make it configurable or cleanup the hardcoded path to search.

csharpier-repos can be found [here](https://github.com/belav/csharpier-repos)
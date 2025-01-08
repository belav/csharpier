using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSharpier.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CSharpier.Playground.Controllers;

public class FormatResult
{
    public required string Code { get; set; }
    public required string Json { get; set; }
    public required string Doc { get; set; }
    public required List<FormatError> Errors { get; set; }
    public required string SyntaxValidation { get; set; }
}

public class FormatError
{
    public required FileLinePositionSpan LineSpan { get; set; }
    public required string Description { get; set; }
}

[ApiController]
[Route("[controller]")]
public class FormatController() : ControllerBase
{
    // ReSharper disable once SuggestBaseTypeForParameter

    public class PostModel
    {
        public string Code { get; set; } = string.Empty;
        public int PrintWidth { get; set; }
        public int IndentSize { get; set; }
        public bool UseTabs { get; set; }
        public string Parser { get; set; } = string.Empty;
    }

    [HttpPost]
    public async Task<FormatResult> Post(
        [FromBody] PostModel model,
        CancellationToken cancellationToken
    )
    {
        var sourceCodeKind = model.Parser.EqualsIgnoreCase("CSharp")
            ? SourceCodeKind.Regular
            : SourceCodeKind.Script;
        var result = await CSharpFormatter.FormatAsync(
            model.Code,
            new PrinterOptions
            {
                IncludeAST = true,
                IncludeDocTree = true,
                Width = model.PrintWidth,
                IndentSize = model.IndentSize,
                UseTabs = model.UseTabs,
            },
            sourceCodeKind,
            cancellationToken
        );

        var comparer = new SyntaxNodeComparer(
            model.Code,
            result.Code,
            result.ReorderedModifiers,
            result.ReorderedUsingsWithDisabledText,
            result.MovedTrailingTrivia,
            sourceCodeKind,
            cancellationToken
        );

        return new FormatResult
        {
            Code = result.Code,
            Json = result.AST,
            Doc = result.DocTree,
            Errors = result.CompilationErrors.Select(this.ConvertError).ToList(),
            SyntaxValidation = await comparer.CompareSourceAsync(CancellationToken.None),
        };
    }

    private FormatError ConvertError(Diagnostic diagnostic)
    {
        var lineSpan = diagnostic.Location.SourceTree!.GetLineSpan(diagnostic.Location.SourceSpan);
        return new FormatError { LineSpan = lineSpan, Description = diagnostic.ToString() };
    }
}

using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Core.DocTypes;

internal sealed class StringDoc(string value, bool isDirective = false) : Doc
{
    private static readonly ConditionalWeakTable<string, StringDoc> StringDocCache = new();
    public string Value { get; } = value;
    public bool IsDirective { get; } = isDirective;

    public static StringDoc Create(string value) =>
        value == " " ? SpaceStringDoc : new StringDoc(value);

    public static StringDoc Create(SyntaxToken token)
    {
        return token.RawSyntaxKind() switch
        {
            SyntaxKind.TildeToken => TildeTokenStringDoc,
            SyntaxKind.ExclamationToken => ExclamationTokenStringDoc,
            SyntaxKind.DollarToken => DollarTokenStringDoc,
            SyntaxKind.PercentToken => PercentTokenStringDoc,
            SyntaxKind.CaretToken => CaretTokenStringDoc,
            SyntaxKind.AmpersandToken => AmpersandTokenStringDoc,
            SyntaxKind.AsteriskToken => AsteriskTokenStringDoc,
            SyntaxKind.OpenParenToken => OpenParenTokenStringDoc,
            SyntaxKind.CloseParenToken => CloseParenTokenStringDoc,
            SyntaxKind.MinusToken => MinusTokenStringDoc,
            SyntaxKind.PlusToken => PlusTokenStringDoc,
            SyntaxKind.EqualsToken => EqualsTokenStringDoc,
            SyntaxKind.OpenBraceToken when token.Text == "{" => OpenBraceTokenStringDoc,
            SyntaxKind.CloseBraceToken when token.Text == "}" => CloseBraceTokenStringDoc,
            SyntaxKind.OpenBracketToken => OpenBracketTokenStringDoc,
            SyntaxKind.CloseBracketToken => CloseBracketTokenStringDoc,
            SyntaxKind.BarToken => BarTokenStringDoc,
            SyntaxKind.BackslashToken => BackslashTokenStringDoc,
            SyntaxKind.ColonToken => ColonTokenStringDoc,
            SyntaxKind.SemicolonToken => SemicolonTokenStringDoc,
            SyntaxKind.DoubleQuoteToken => DoubleQuoteTokenStringDoc,
            SyntaxKind.SingleQuoteToken => SingleQuoteTokenStringDoc,
            SyntaxKind.LessThanToken => LessThanTokenStringDoc,
            SyntaxKind.CommaToken => CommaTokenStringDoc,
            SyntaxKind.GreaterThanToken => GreaterThanTokenStringDoc,
            SyntaxKind.DotToken => DotTokenStringDoc,
            SyntaxKind.QuestionToken => QuestionTokenStringDoc,
            SyntaxKind.HashToken => HashTokenStringDoc,
            SyntaxKind.SlashToken => SlashTokenStringDoc,
            SyntaxKind.SlashGreaterThanToken => SlashGreaterThanTokenStringDoc,
            SyntaxKind.LessThanSlashToken => LessThanSlashTokenStringDoc,
            SyntaxKind.XmlCommentStartToken => XmlCommentStartTokenStringDoc,
            SyntaxKind.XmlCommentEndToken => XmlCommentEndTokenStringDoc,
            SyntaxKind.XmlCDataStartToken => XmlCDataStartTokenStringDoc,
            SyntaxKind.XmlCDataEndToken => XmlCDataEndTokenStringDoc,
            SyntaxKind.XmlProcessingInstructionStartToken =>
                XmlProcessingInstructionStartTokenStringDoc,
            SyntaxKind.XmlProcessingInstructionEndToken =>
                XmlProcessingInstructionEndTokenStringDoc,
            // compound
            SyntaxKind.BarBarToken => BarBarTokenStringDoc,
            SyntaxKind.AmpersandAmpersandToken => AmpersandAmpersandTokenStringDoc,
            SyntaxKind.MinusMinusToken => MinusMinusTokenStringDoc,
            SyntaxKind.PlusPlusToken => PlusPlusTokenStringDoc,
            SyntaxKind.ColonColonToken => ColonColonTokenStringDoc,
            SyntaxKind.QuestionQuestionToken => QuestionQuestionTokenStringDoc,
            SyntaxKind.MinusGreaterThanToken => MinusGreaterThanTokenStringDoc,
            SyntaxKind.ExclamationEqualsToken => ExclamationEqualsTokenStringDoc,
            SyntaxKind.EqualsEqualsToken => EqualsEqualsTokenStringDoc,
            SyntaxKind.EqualsGreaterThanToken => EqualsGreaterThanTokenStringDoc,
            SyntaxKind.LessThanEqualsToken => LessThanEqualsTokenStringDoc,
            SyntaxKind.LessThanLessThanToken => LessThanLessThanTokenStringDoc,
            SyntaxKind.LessThanLessThanEqualsToken => LessThanLessThanEqualsTokenStringDoc,
            SyntaxKind.GreaterThanEqualsToken => GreaterThanEqualsTokenStringDoc,
            SyntaxKind.GreaterThanGreaterThanToken => GreaterThanGreaterThanTokenStringDoc,
            SyntaxKind.GreaterThanGreaterThanEqualsToken =>
                GreaterThanGreaterThanEqualsTokenStringDoc,
            SyntaxKind.GreaterThanGreaterThanGreaterThanToken =>
                GreaterThanGreaterThanGreaterThanTokenStringDoc,
            SyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken =>
                GreaterThanGreaterThanGreaterThanEqualsTokenStringDoc,
            SyntaxKind.SlashEqualsToken => SlashEqualsTokenStringDoc,
            SyntaxKind.AsteriskEqualsToken => AsteriskEqualsTokenStringDoc,
            SyntaxKind.BarEqualsToken => BarEqualsTokenStringDoc,
            SyntaxKind.AmpersandEqualsToken => AmpersandEqualsTokenStringDoc,
            SyntaxKind.PlusEqualsToken => PlusEqualsTokenStringDoc,
            SyntaxKind.MinusEqualsToken => MinusEqualsTokenStringDoc,
            SyntaxKind.CaretEqualsToken => CaretEqualsTokenStringDoc,
            SyntaxKind.PercentEqualsToken => PercentEqualsTokenStringDoc,
            SyntaxKind.QuestionQuestionEqualsToken => QuestionQuestionEqualsTokenStringDoc,
            SyntaxKind.DotDotToken => DotDotTokenStringDoc,
            // Keywords
            SyntaxKind.BoolKeyword => BoolKeywordStringDoc,
            SyntaxKind.ByteKeyword => ByteKeywordStringDoc,
            SyntaxKind.SByteKeyword => SByteKeywordStringDoc,
            SyntaxKind.ShortKeyword => ShortKeywordStringDoc,
            SyntaxKind.UShortKeyword => UShortKeywordStringDoc,
            SyntaxKind.IntKeyword => IntKeywordStringDoc,
            SyntaxKind.UIntKeyword => UIntKeywordStringDoc,
            SyntaxKind.LongKeyword => LongKeywordStringDoc,
            SyntaxKind.ULongKeyword => ULongKeywordStringDoc,
            SyntaxKind.DoubleKeyword => DoubleKeywordStringDoc,
            SyntaxKind.FloatKeyword => FloatKeywordStringDoc,
            SyntaxKind.DecimalKeyword => DecimalKeywordStringDoc,
            SyntaxKind.StringKeyword => StringKeywordStringDoc,
            SyntaxKind.CharKeyword => CharKeywordStringDoc,
            SyntaxKind.VoidKeyword => VoidKeywordStringDoc,
            SyntaxKind.ObjectKeyword => ObjectKeywordStringDoc,
            SyntaxKind.TypeOfKeyword => TypeOfKeywordStringDoc,
            SyntaxKind.SizeOfKeyword => SizeOfKeywordStringDoc,
            SyntaxKind.NullKeyword => NullKeywordStringDoc,
            SyntaxKind.TrueKeyword => TrueKeywordStringDoc,
            SyntaxKind.FalseKeyword => FalseKeywordStringDoc,
            SyntaxKind.IfKeyword => IfKeywordStringDoc,
            SyntaxKind.ElseKeyword => ElseKeywordStringDoc,
            SyntaxKind.WhileKeyword => WhileKeywordStringDoc,
            SyntaxKind.ForKeyword => ForKeywordStringDoc,
            SyntaxKind.ForEachKeyword => ForEachKeywordStringDoc,
            SyntaxKind.DoKeyword => DoKeywordStringDoc,
            SyntaxKind.SwitchKeyword => SwitchKeywordStringDoc,
            SyntaxKind.CaseKeyword => CaseKeywordStringDoc,
            SyntaxKind.DefaultKeyword => DefaultKeywordStringDoc,
            SyntaxKind.TryKeyword => TryKeywordStringDoc,
            SyntaxKind.CatchKeyword => CatchKeywordStringDoc,
            SyntaxKind.FinallyKeyword => FinallyKeywordStringDoc,
            SyntaxKind.LockKeyword => LockKeywordStringDoc,
            SyntaxKind.GotoKeyword => GotoKeywordStringDoc,
            SyntaxKind.BreakKeyword => BreakKeywordStringDoc,
            SyntaxKind.ContinueKeyword => ContinueKeywordStringDoc,
            SyntaxKind.ReturnKeyword => ReturnKeywordStringDoc,
            SyntaxKind.ThrowKeyword => ThrowKeywordStringDoc,
            SyntaxKind.PublicKeyword => PublicKeywordStringDoc,
            SyntaxKind.PrivateKeyword => PrivateKeywordStringDoc,
            SyntaxKind.InternalKeyword => InternalKeywordStringDoc,
            SyntaxKind.ProtectedKeyword => ProtectedKeywordStringDoc,
            SyntaxKind.StaticKeyword => StaticKeywordStringDoc,
            SyntaxKind.ReadOnlyKeyword => ReadOnlyKeywordStringDoc,
            SyntaxKind.SealedKeyword => SealedKeywordStringDoc,
            SyntaxKind.ConstKeyword => ConstKeywordStringDoc,
            SyntaxKind.FixedKeyword => FixedKeywordStringDoc,
            SyntaxKind.StackAllocKeyword => StackAllocKeywordStringDoc,
            SyntaxKind.VolatileKeyword => VolatileKeywordStringDoc,
            SyntaxKind.NewKeyword => NewKeywordStringDoc,
            SyntaxKind.OverrideKeyword => OverrideKeywordStringDoc,
            SyntaxKind.AbstractKeyword => AbstractKeywordStringDoc,
            SyntaxKind.VirtualKeyword => VirtualKeywordStringDoc,
            SyntaxKind.EventKeyword => EventKeywordStringDoc,
            SyntaxKind.ExternKeyword => ExternKeywordStringDoc,
            SyntaxKind.RefKeyword => RefKeywordStringDoc,
            SyntaxKind.OutKeyword => OutKeywordStringDoc,
            SyntaxKind.InKeyword => InKeywordStringDoc,
            SyntaxKind.IsKeyword => IsKeywordStringDoc,
            SyntaxKind.AsKeyword => AsKeywordStringDoc,
            SyntaxKind.ParamsKeyword => ParamsKeywordStringDoc,
            SyntaxKind.ArgListKeyword => ArgListKeywordStringDoc,
            SyntaxKind.MakeRefKeyword => MakeRefKeywordStringDoc,
            SyntaxKind.RefTypeKeyword => RefTypeKeywordStringDoc,
            SyntaxKind.RefValueKeyword => RefValueKeywordStringDoc,
            SyntaxKind.ThisKeyword => ThisKeywordStringDoc,
            SyntaxKind.BaseKeyword => BaseKeywordStringDoc,
            SyntaxKind.NamespaceKeyword => NamespaceKeywordStringDoc,
            SyntaxKind.UsingKeyword => UsingKeywordStringDoc,
            SyntaxKind.ClassKeyword => ClassKeywordStringDoc,
            SyntaxKind.StructKeyword => StructKeywordStringDoc,
            SyntaxKind.InterfaceKeyword => InterfaceKeywordStringDoc,
            SyntaxKind.EnumKeyword => EnumKeywordStringDoc,
            SyntaxKind.DelegateKeyword => DelegateKeywordStringDoc,
            SyntaxKind.CheckedKeyword => CheckedKeywordStringDoc,
            SyntaxKind.UncheckedKeyword => UncheckedKeywordStringDoc,
            SyntaxKind.UnsafeKeyword => UnsafeKeywordStringDoc,
            SyntaxKind.OperatorKeyword => OperatorKeywordStringDoc,
            SyntaxKind.ImplicitKeyword => ImplicitKeywordStringDoc,
            SyntaxKind.ExplicitKeyword => ExplicitKeywordStringDoc,
            SyntaxKind.ElifKeyword => ElifKeywordStringDoc,
            SyntaxKind.EndIfKeyword => EndIfKeywordStringDoc,
            SyntaxKind.RegionKeyword => RegionKeywordStringDoc,
            SyntaxKind.EndRegionKeyword => EndRegionKeywordStringDoc,
            SyntaxKind.DefineKeyword => DefineKeywordStringDoc,
            SyntaxKind.UndefKeyword => UndefKeywordStringDoc,
            SyntaxKind.WarningKeyword => WarningKeywordStringDoc,
            SyntaxKind.ErrorKeyword => ErrorKeywordStringDoc,
            SyntaxKind.LineKeyword => LineKeywordStringDoc,
            SyntaxKind.PragmaKeyword => PragmaKeywordStringDoc,
            SyntaxKind.HiddenKeyword => HiddenKeywordStringDoc,
            SyntaxKind.ChecksumKeyword => ChecksumKeywordStringDoc,
            SyntaxKind.DisableKeyword => DisableKeywordStringDoc,
            SyntaxKind.RestoreKeyword => RestoreKeywordStringDoc,
            SyntaxKind.ReferenceKeyword => ReferenceKeywordStringDoc,
            SyntaxKind.LoadKeyword => LoadKeywordStringDoc,
            SyntaxKind.NullableKeyword => NullableKeywordStringDoc,
            SyntaxKind.EnableKeyword => EnableKeywordStringDoc,
            SyntaxKind.WarningsKeyword => WarningsKeywordStringDoc,
            SyntaxKind.AnnotationsKeyword => AnnotationsKeywordStringDoc,
            // contextual keywords
            SyntaxKind.YieldKeyword => YieldKeywordStringDoc,
            SyntaxKind.PartialKeyword => PartialKeywordStringDoc,
            SyntaxKind.FromKeyword => FromKeywordStringDoc,
            SyntaxKind.GroupKeyword => GroupKeywordStringDoc,
            SyntaxKind.JoinKeyword => JoinKeywordStringDoc,
            SyntaxKind.IntoKeyword => IntoKeywordStringDoc,
            SyntaxKind.LetKeyword => LetKeywordStringDoc,
            SyntaxKind.ByKeyword => ByKeywordStringDoc,
            SyntaxKind.WhereKeyword => WhereKeywordStringDoc,
            SyntaxKind.SelectKeyword => SelectKeywordStringDoc,
            SyntaxKind.GetKeyword => GetKeywordStringDoc,
            SyntaxKind.SetKeyword => SetKeywordStringDoc,
            SyntaxKind.AddKeyword => AddKeywordStringDoc,
            SyntaxKind.RemoveKeyword => RemoveKeywordStringDoc,
            SyntaxKind.OrderByKeyword => OrderByKeywordStringDoc,
            SyntaxKind.AliasKeyword => AliasKeywordStringDoc,
            SyntaxKind.OnKeyword => OnKeywordStringDoc,
            SyntaxKind.EqualsKeyword => EqualsKeywordStringDoc,
            SyntaxKind.AscendingKeyword => AscendingKeywordStringDoc,
            SyntaxKind.DescendingKeyword => DescendingKeywordStringDoc,
            SyntaxKind.AssemblyKeyword => AssemblyKeywordStringDoc,
            SyntaxKind.ModuleKeyword => ModuleKeywordStringDoc,
            SyntaxKind.TypeKeyword => TypeKeywordStringDoc,
            SyntaxKind.FieldKeyword => FieldKeywordStringDoc,
            SyntaxKind.MethodKeyword => MethodKeywordStringDoc,
            SyntaxKind.ParamKeyword => ParamKeywordStringDoc,
            SyntaxKind.PropertyKeyword => PropertyKeywordStringDoc,
            SyntaxKind.TypeVarKeyword => TypeVarKeywordStringDoc,
            SyntaxKind.GlobalKeyword => GlobalKeywordStringDoc,
            SyntaxKind.NameOfKeyword => NameOfKeywordStringDoc,
            SyntaxKind.AsyncKeyword => AsyncKeywordStringDoc,
            SyntaxKind.AwaitKeyword => AwaitKeywordStringDoc,
            SyntaxKind.WhenKeyword => WhenKeywordStringDoc,
            SyntaxKind.InterpolatedStringStartToken => InterpolatedStringStartTokenStringDoc,
            SyntaxKind.InterpolatedStringEndToken => InterpolatedStringEndTokenStringDoc,
            SyntaxKind.InterpolatedVerbatimStringStartToken when token.Text[0] is '$' =>
                InterpolatedVerbatimStringStartTokenStringDoc0,
            SyntaxKind.InterpolatedVerbatimStringStartToken when token.Text[0] is '@' =>
                InterpolatedVerbatimStringStartTokenStringDoc1,
            SyntaxKind.UnderscoreToken => UnderscoreTokenStringDoc,
            SyntaxKind.VarKeyword => VarKeywordStringDoc,
            SyntaxKind.AndKeyword => AndKeywordStringDoc,
            SyntaxKind.OrKeyword => OrKeywordStringDoc,
            SyntaxKind.NotKeyword => NotKeywordStringDoc,
            SyntaxKind.WithKeyword => WithKeywordStringDoc,
            SyntaxKind.InitKeyword => InitKeywordStringDoc,
            SyntaxKind.RecordKeyword => RecordKeywordStringDoc,
            SyntaxKind.ManagedKeyword => ManagedKeywordStringDoc,
            SyntaxKind.UnmanagedKeyword => UnmanagedKeywordStringDoc,
            SyntaxKind.RequiredKeyword => RequiredKeywordStringDoc,
            SyntaxKind.ScopedKeyword => ScopedKeywordStringDoc,
            SyntaxKind.FileKeyword => FileKeywordStringDoc,
            SyntaxKind.AllowsKeyword => AllowsKeywordStringDoc,
            _ => StringDocCache.GetValue(token.Text, static text => new StringDoc(text)),
        };
    }

    private static readonly StringDoc SpaceStringDoc = new(" ");
    private static readonly StringDoc TildeTokenStringDoc = new("~");
    private static readonly StringDoc ExclamationTokenStringDoc = new("!");
    private static readonly StringDoc DollarTokenStringDoc = new("$");
    private static readonly StringDoc PercentTokenStringDoc = new("%");
    private static readonly StringDoc CaretTokenStringDoc = new("^");
    private static readonly StringDoc AmpersandTokenStringDoc = new("&");
    private static readonly StringDoc AsteriskTokenStringDoc = new("*");
    private static readonly StringDoc OpenParenTokenStringDoc = new("(");
    private static readonly StringDoc CloseParenTokenStringDoc = new(")");
    private static readonly StringDoc MinusTokenStringDoc = new("-");
    private static readonly StringDoc PlusTokenStringDoc = new("+");
    private static readonly StringDoc EqualsTokenStringDoc = new("=");
    private static readonly StringDoc OpenBraceTokenStringDoc = new("{");
    private static readonly StringDoc CloseBraceTokenStringDoc = new("}");
    private static readonly StringDoc OpenBracketTokenStringDoc = new("[");
    private static readonly StringDoc CloseBracketTokenStringDoc = new("]");
    private static readonly StringDoc BarTokenStringDoc = new("|");
    private static readonly StringDoc BackslashTokenStringDoc = new("\\");
    private static readonly StringDoc ColonTokenStringDoc = new(":");
    private static readonly StringDoc SemicolonTokenStringDoc = new(";");
    private static readonly StringDoc DoubleQuoteTokenStringDoc = new("\"");
    private static readonly StringDoc SingleQuoteTokenStringDoc = new("'");
    private static readonly StringDoc LessThanTokenStringDoc = new("<");
    private static readonly StringDoc CommaTokenStringDoc = new(",");
    private static readonly StringDoc GreaterThanTokenStringDoc = new(">");
    private static readonly StringDoc DotTokenStringDoc = new(".");
    private static readonly StringDoc QuestionTokenStringDoc = new("?");
    private static readonly StringDoc HashTokenStringDoc = new("#");
    private static readonly StringDoc SlashTokenStringDoc = new("/");
    private static readonly StringDoc SlashGreaterThanTokenStringDoc = new("/>");
    private static readonly StringDoc LessThanSlashTokenStringDoc = new("</");
    private static readonly StringDoc XmlCommentStartTokenStringDoc = new("<!--");
    private static readonly StringDoc XmlCommentEndTokenStringDoc = new("-->");
    private static readonly StringDoc XmlCDataStartTokenStringDoc = new("<![CDATA[");
    private static readonly StringDoc XmlCDataEndTokenStringDoc = new("]]>");
    private static readonly StringDoc XmlProcessingInstructionStartTokenStringDoc = new("<?");
    private static readonly StringDoc XmlProcessingInstructionEndTokenStringDoc = new("?>");

    private static readonly StringDoc BarBarTokenStringDoc = new("||");
    private static readonly StringDoc AmpersandAmpersandTokenStringDoc = new("&&");
    private static readonly StringDoc MinusMinusTokenStringDoc = new("--");
    private static readonly StringDoc PlusPlusTokenStringDoc = new("++");
    private static readonly StringDoc ColonColonTokenStringDoc = new("::");
    private static readonly StringDoc QuestionQuestionTokenStringDoc = new("??");
    private static readonly StringDoc MinusGreaterThanTokenStringDoc = new("->");
    private static readonly StringDoc ExclamationEqualsTokenStringDoc = new("!=");
    private static readonly StringDoc EqualsEqualsTokenStringDoc = new("==");
    private static readonly StringDoc EqualsGreaterThanTokenStringDoc = new("=>");
    private static readonly StringDoc LessThanEqualsTokenStringDoc = new("<=");
    private static readonly StringDoc LessThanLessThanTokenStringDoc = new("<<");
    private static readonly StringDoc LessThanLessThanEqualsTokenStringDoc = new("<<=");
    private static readonly StringDoc GreaterThanEqualsTokenStringDoc = new(">=");
    private static readonly StringDoc GreaterThanGreaterThanTokenStringDoc = new(">>");
    private static readonly StringDoc GreaterThanGreaterThanEqualsTokenStringDoc = new(">>=");
    private static readonly StringDoc GreaterThanGreaterThanGreaterThanTokenStringDoc = new(">>>");
    private static readonly StringDoc GreaterThanGreaterThanGreaterThanEqualsTokenStringDoc = new(
        ">>>="
    );
    private static readonly StringDoc SlashEqualsTokenStringDoc = new("/=");
    private static readonly StringDoc AsteriskEqualsTokenStringDoc = new("*=");
    private static readonly StringDoc BarEqualsTokenStringDoc = new("|=");
    private static readonly StringDoc AmpersandEqualsTokenStringDoc = new("&=");
    private static readonly StringDoc PlusEqualsTokenStringDoc = new("+=");
    private static readonly StringDoc MinusEqualsTokenStringDoc = new("-=");
    private static readonly StringDoc CaretEqualsTokenStringDoc = new("^=");
    private static readonly StringDoc PercentEqualsTokenStringDoc = new("%=");
    private static readonly StringDoc QuestionQuestionEqualsTokenStringDoc = new("??=");
    private static readonly StringDoc DotDotTokenStringDoc = new("..");

    private static readonly StringDoc BoolKeywordStringDoc = new("bool");
    private static readonly StringDoc ByteKeywordStringDoc = new("byte");
    private static readonly StringDoc SByteKeywordStringDoc = new("sbyte");
    private static readonly StringDoc ShortKeywordStringDoc = new("short");
    private static readonly StringDoc UShortKeywordStringDoc = new("ushort");
    private static readonly StringDoc IntKeywordStringDoc = new("int");
    private static readonly StringDoc UIntKeywordStringDoc = new("uint");
    private static readonly StringDoc LongKeywordStringDoc = new("long");
    private static readonly StringDoc ULongKeywordStringDoc = new("ulong");
    private static readonly StringDoc DoubleKeywordStringDoc = new("double");
    private static readonly StringDoc FloatKeywordStringDoc = new("float");
    private static readonly StringDoc DecimalKeywordStringDoc = new("decimal");
    private static readonly StringDoc StringKeywordStringDoc = new("string");
    private static readonly StringDoc CharKeywordStringDoc = new("char");
    private static readonly StringDoc VoidKeywordStringDoc = new("void");
    private static readonly StringDoc ObjectKeywordStringDoc = new("object");
    private static readonly StringDoc TypeOfKeywordStringDoc = new("typeof");
    private static readonly StringDoc SizeOfKeywordStringDoc = new("sizeof");
    private static readonly StringDoc NullKeywordStringDoc = new("null");
    private static readonly StringDoc TrueKeywordStringDoc = new("true");
    private static readonly StringDoc FalseKeywordStringDoc = new("false");
    private static readonly StringDoc IfKeywordStringDoc = new("if");
    private static readonly StringDoc ElseKeywordStringDoc = new("else");
    private static readonly StringDoc WhileKeywordStringDoc = new("while");
    private static readonly StringDoc ForKeywordStringDoc = new("for");
    private static readonly StringDoc ForEachKeywordStringDoc = new("foreach");
    private static readonly StringDoc DoKeywordStringDoc = new("do");
    private static readonly StringDoc SwitchKeywordStringDoc = new("switch");
    private static readonly StringDoc CaseKeywordStringDoc = new("case");
    private static readonly StringDoc DefaultKeywordStringDoc = new("default");
    private static readonly StringDoc TryKeywordStringDoc = new("try");
    private static readonly StringDoc CatchKeywordStringDoc = new("catch");
    private static readonly StringDoc FinallyKeywordStringDoc = new("finally");
    private static readonly StringDoc LockKeywordStringDoc = new("lock");
    private static readonly StringDoc GotoKeywordStringDoc = new("goto");
    private static readonly StringDoc BreakKeywordStringDoc = new("break");
    private static readonly StringDoc ContinueKeywordStringDoc = new("continue");
    private static readonly StringDoc ReturnKeywordStringDoc = new("return");
    private static readonly StringDoc ThrowKeywordStringDoc = new("throw");
    private static readonly StringDoc PublicKeywordStringDoc = new("public");
    private static readonly StringDoc PrivateKeywordStringDoc = new("private");
    private static readonly StringDoc InternalKeywordStringDoc = new("internal");
    private static readonly StringDoc ProtectedKeywordStringDoc = new("protected");
    private static readonly StringDoc StaticKeywordStringDoc = new("static");
    private static readonly StringDoc ReadOnlyKeywordStringDoc = new("readonly");
    private static readonly StringDoc SealedKeywordStringDoc = new("sealed");
    private static readonly StringDoc ConstKeywordStringDoc = new("const");
    private static readonly StringDoc FixedKeywordStringDoc = new("fixed");
    private static readonly StringDoc StackAllocKeywordStringDoc = new("stackalloc");
    private static readonly StringDoc VolatileKeywordStringDoc = new("volatile");
    private static readonly StringDoc NewKeywordStringDoc = new("new");
    private static readonly StringDoc OverrideKeywordStringDoc = new("override");
    private static readonly StringDoc AbstractKeywordStringDoc = new("abstract");
    private static readonly StringDoc VirtualKeywordStringDoc = new("virtual");
    private static readonly StringDoc EventKeywordStringDoc = new("event");
    private static readonly StringDoc ExternKeywordStringDoc = new("extern");
    private static readonly StringDoc RefKeywordStringDoc = new("ref");
    private static readonly StringDoc OutKeywordStringDoc = new("out");
    private static readonly StringDoc InKeywordStringDoc = new("in");
    private static readonly StringDoc IsKeywordStringDoc = new("is");
    private static readonly StringDoc AsKeywordStringDoc = new("as");
    private static readonly StringDoc ParamsKeywordStringDoc = new("params");
    private static readonly StringDoc ArgListKeywordStringDoc = new("__arglist");
    private static readonly StringDoc MakeRefKeywordStringDoc = new("__makeref");
    private static readonly StringDoc RefTypeKeywordStringDoc = new("__reftype");
    private static readonly StringDoc RefValueKeywordStringDoc = new("__refvalue");
    private static readonly StringDoc ThisKeywordStringDoc = new("this");
    private static readonly StringDoc BaseKeywordStringDoc = new("base");
    private static readonly StringDoc NamespaceKeywordStringDoc = new("namespace");
    private static readonly StringDoc UsingKeywordStringDoc = new("using");
    private static readonly StringDoc ClassKeywordStringDoc = new("class");
    private static readonly StringDoc StructKeywordStringDoc = new("struct");
    private static readonly StringDoc InterfaceKeywordStringDoc = new("interface");
    private static readonly StringDoc EnumKeywordStringDoc = new("enum");
    private static readonly StringDoc DelegateKeywordStringDoc = new("delegate");
    private static readonly StringDoc CheckedKeywordStringDoc = new("checked");
    private static readonly StringDoc UncheckedKeywordStringDoc = new("unchecked");
    private static readonly StringDoc UnsafeKeywordStringDoc = new("unsafe");
    private static readonly StringDoc OperatorKeywordStringDoc = new("operator");
    private static readonly StringDoc ImplicitKeywordStringDoc = new("implicit");
    private static readonly StringDoc ExplicitKeywordStringDoc = new("explicit");
    private static readonly StringDoc ElifKeywordStringDoc = new("elif");
    private static readonly StringDoc EndIfKeywordStringDoc = new("endif");
    private static readonly StringDoc RegionKeywordStringDoc = new("region");
    private static readonly StringDoc EndRegionKeywordStringDoc = new("endregion");
    private static readonly StringDoc DefineKeywordStringDoc = new("define");
    private static readonly StringDoc UndefKeywordStringDoc = new("undef");
    private static readonly StringDoc WarningKeywordStringDoc = new("warning");
    private static readonly StringDoc ErrorKeywordStringDoc = new("error");
    private static readonly StringDoc LineKeywordStringDoc = new("line");
    private static readonly StringDoc PragmaKeywordStringDoc = new("pragma");
    private static readonly StringDoc HiddenKeywordStringDoc = new("hidden");
    private static readonly StringDoc ChecksumKeywordStringDoc = new("checksum");
    private static readonly StringDoc DisableKeywordStringDoc = new("disable");
    private static readonly StringDoc RestoreKeywordStringDoc = new("restore");
    private static readonly StringDoc ReferenceKeywordStringDoc = new("r");
    private static readonly StringDoc LoadKeywordStringDoc = new("load");
    private static readonly StringDoc NullableKeywordStringDoc = new("nullable");
    private static readonly StringDoc EnableKeywordStringDoc = new("enable");
    private static readonly StringDoc WarningsKeywordStringDoc = new("warnings");
    private static readonly StringDoc AnnotationsKeywordStringDoc = new("annotations");

    private static readonly StringDoc YieldKeywordStringDoc = new("yield");
    private static readonly StringDoc PartialKeywordStringDoc = new("partial");
    private static readonly StringDoc FromKeywordStringDoc = new("from");
    private static readonly StringDoc GroupKeywordStringDoc = new("group");
    private static readonly StringDoc JoinKeywordStringDoc = new("join");
    private static readonly StringDoc IntoKeywordStringDoc = new("into");
    private static readonly StringDoc LetKeywordStringDoc = new("let");
    private static readonly StringDoc ByKeywordStringDoc = new("by");
    private static readonly StringDoc WhereKeywordStringDoc = new("where");
    private static readonly StringDoc SelectKeywordStringDoc = new("select");
    private static readonly StringDoc GetKeywordStringDoc = new("get");
    private static readonly StringDoc SetKeywordStringDoc = new("set");
    private static readonly StringDoc AddKeywordStringDoc = new("add");
    private static readonly StringDoc RemoveKeywordStringDoc = new("remove");
    private static readonly StringDoc OrderByKeywordStringDoc = new("orderby");
    private static readonly StringDoc AliasKeywordStringDoc = new("alias");
    private static readonly StringDoc OnKeywordStringDoc = new("on");
    private static readonly StringDoc EqualsKeywordStringDoc = new("equals");
    private static readonly StringDoc AscendingKeywordStringDoc = new("ascending");
    private static readonly StringDoc DescendingKeywordStringDoc = new("descending");
    private static readonly StringDoc AssemblyKeywordStringDoc = new("assembly");
    private static readonly StringDoc ModuleKeywordStringDoc = new("module");
    private static readonly StringDoc TypeKeywordStringDoc = new("type");
    private static readonly StringDoc FieldKeywordStringDoc = new("field");
    private static readonly StringDoc MethodKeywordStringDoc = new("method");
    private static readonly StringDoc ParamKeywordStringDoc = new("param");
    private static readonly StringDoc PropertyKeywordStringDoc = new("property");
    private static readonly StringDoc TypeVarKeywordStringDoc = new("typevar");
    private static readonly StringDoc GlobalKeywordStringDoc = new("global");
    private static readonly StringDoc NameOfKeywordStringDoc = new("nameof");
    private static readonly StringDoc AsyncKeywordStringDoc = new("async");
    private static readonly StringDoc AwaitKeywordStringDoc = new("await");
    private static readonly StringDoc WhenKeywordStringDoc = new("when");
    private static readonly StringDoc InterpolatedStringStartTokenStringDoc = new("$\"");
    private static readonly StringDoc InterpolatedStringEndTokenStringDoc = new("\"");
    private static readonly StringDoc InterpolatedVerbatimStringStartTokenStringDoc0 = new("$@\"");
    private static readonly StringDoc InterpolatedVerbatimStringStartTokenStringDoc1 = new("@$\"");
    private static readonly StringDoc UnderscoreTokenStringDoc = new("_");
    private static readonly StringDoc VarKeywordStringDoc = new("var");
    private static readonly StringDoc AndKeywordStringDoc = new("and");
    private static readonly StringDoc OrKeywordStringDoc = new("or");
    private static readonly StringDoc NotKeywordStringDoc = new("not");
    private static readonly StringDoc WithKeywordStringDoc = new("with");
    private static readonly StringDoc InitKeywordStringDoc = new("init");
    private static readonly StringDoc RecordKeywordStringDoc = new("record");
    private static readonly StringDoc ManagedKeywordStringDoc = new("managed");
    private static readonly StringDoc UnmanagedKeywordStringDoc = new("unmanaged");
    private static readonly StringDoc RequiredKeywordStringDoc = new("required");
    private static readonly StringDoc ScopedKeywordStringDoc = new("scoped");
    private static readonly StringDoc FileKeywordStringDoc = new("file");
    private static readonly StringDoc AllowsKeywordStringDoc = new("allows");
}

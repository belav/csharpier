namespace CSharpier.Cli.Tests;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliWrap;
using CliWrap.Buffered;
using FluentAssertions;
using NUnit.Framework;

// these tests are kind of nice as c# because they run in the same place.
// except the one test that has issues with console input redirection
// they used to be powershell, but doing the multiple file thing didn't work
// that worked in by writing js, but that felt worse than powershell
// the CSharpierProcess abstraction is also a little fragile, but makes for clean tests when they
// are written properly
public class CliTests
{
    private static readonly string testFileDirectory = Path.Combine(
        Directory.GetCurrentDirectory(),
        "TestFiles"
    );

    [SetUp]
    public void BeforeEachTest()
    {
        if (File.Exists(FormattingCacheFactory.CacheFilePath))
        {
            File.Delete(FormattingCacheFactory.CacheFilePath);
        }

        void DeleteDirectory()
        {
            if (Directory.Exists(testFileDirectory))
            {
                Directory.Delete(testFileDirectory, true);
            }
        }

        try
        {
            DeleteDirectory();
        }
        catch (Exception)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            DeleteDirectory();
        }

        Directory.CreateDirectory(testFileDirectory);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Basic_File(string lineEnding)
    {
        var formattedContent = "public class ClassName { }" + lineEnding;
        var unformattedContent = $"public class ClassName {{{lineEnding}{lineEnding}}}";

        await this.WriteFileAsync("BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess().WithArguments("BasicFile.cs").ExecuteAsync();

        result.ErrorOutput.Should().BeNullOrEmpty();
        result.Output.Should().StartWith("Formatted 1 files in ");
        result.ExitCode.Should().Be(0);
        (await this.ReadAllTextAsync("BasicFile.cs")).Should().Be(formattedContent);
    }

    [TestCase(".")]
    [TestCase("./")]
    public async Task Should_Work_With_This(string fileOrDirectory)
    {
        await this.WriteFileAsync("Subfolder/BasicFile.cs", "public class ClassName { }");
        await this.WriteFileAsync("BasicFile.cs", "public class ClassName { }");
        await this.WriteFileAsync(
            ".editorconfig",
            """
root = true

# All files
[*]
indent_style = space

# Xml files
[*.xml]
indent_size = 2

# C# files
[*.cs]

#### Core EditorConfig Options ####

# Indentation and spacing
indent_size = 4
tab_width = 4

# New line preferences
end_of_line = crlf
insert_final_newline = false

#### .NET Coding Conventions ####
[*.{cs,vb}]

# Organize usings
dotnet_separate_import_directive_groups = true
dotnet_sort_system_directives_first = true
file_header_template = unset

# this. and Me. preferences
dotnet_style_qualification_for_event = false:silent
dotnet_style_qualification_for_field = false:silent
dotnet_style_qualification_for_method = false:silent
dotnet_style_qualification_for_property = false:silent

# Language keywords vs BCL types preferences
dotnet_style_predefined_type_for_locals_parameters_members = true:silent
dotnet_style_predefined_type_for_member_access = true:silent

# Parentheses preferences
dotnet_style_parentheses_in_arithmetic_binary_operators = always_for_clarity:silent
dotnet_style_parentheses_in_other_binary_operators = always_for_clarity:silent
dotnet_style_parentheses_in_other_operators = never_if_unnecessary:silent
dotnet_style_parentheses_in_relational_binary_operators = always_for_clarity:silent

# Modifier preferences
dotnet_style_require_accessibility_modifiers = for_non_interface_members:silent

# Expression-level preferences
dotnet_style_coalesce_expression = true:suggestion
dotnet_style_collection_initializer = true:suggestion
dotnet_style_explicit_tuple_names = true:suggestion
dotnet_style_null_propagation = true:suggestion
dotnet_style_object_initializer = true:suggestion
dotnet_style_operator_placement_when_wrapping = beginning_of_line
dotnet_style_prefer_auto_properties = true:suggestion
dotnet_style_prefer_compound_assignment = true:suggestion
dotnet_style_prefer_conditional_expression_over_assignment = true:suggestion
dotnet_style_prefer_conditional_expression_over_return = true:suggestion
dotnet_style_prefer_inferred_anonymous_type_member_names = true:suggestion
dotnet_style_prefer_inferred_tuple_names = true:suggestion
dotnet_style_prefer_is_null_check_over_reference_equality_method = true:suggestion
dotnet_style_prefer_simplified_boolean_expressions = true:suggestion
dotnet_style_prefer_simplified_interpolation = true:suggestion

# Field preferences
dotnet_style_readonly_field = true:warning

# Parameter preferences
dotnet_code_quality_unused_parameters = all:suggestion

# Suppression preferences
dotnet_remove_unnecessary_suppression_exclusions = none

#### C# Coding Conventions ####
[*.cs]

# var preferences
csharp_style_var_elsewhere = false:silent
csharp_style_var_for_built_in_types = false:silent
csharp_style_var_when_type_is_apparent = false:silent

# Expression-bodied members
csharp_style_expression_bodied_accessors = true:silent
csharp_style_expression_bodied_constructors = false:silent
csharp_style_expression_bodied_indexers = true:silent
csharp_style_expression_bodied_lambdas = true:suggestion
csharp_style_expression_bodied_local_functions = false:silent
csharp_style_expression_bodied_methods = false:silent
csharp_style_expression_bodied_operators = false:silent
csharp_style_expression_bodied_properties = true:silent

# Pattern matching preferences
csharp_style_pattern_matching_over_as_with_null_check = true:suggestion
csharp_style_pattern_matching_over_is_with_cast_check = true:suggestion
csharp_style_prefer_not_pattern = true:suggestion
csharp_style_prefer_pattern_matching = true:silent
csharp_style_prefer_switch_expression = true:suggestion

# Null-checking preferences
csharp_style_conditional_delegate_call = true:suggestion

# Modifier preferences
csharp_prefer_static_local_function = true:warning
csharp_preferred_modifier_order = public,private,protected,internal,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,volatile,async:silent

# Code-block preferences
csharp_prefer_braces = true:silent
csharp_prefer_simple_using_statement = true:suggestion

# Expression-level preferences
csharp_prefer_simple_default_expression = true:suggestion
csharp_style_deconstructed_variable_declaration = true:suggestion
csharp_style_inlined_variable_declaration = true:suggestion
csharp_style_pattern_local_over_anonymous_function = true:suggestion
csharp_style_prefer_index_operator = true:suggestion
csharp_style_prefer_range_operator = true:suggestion
csharp_style_throw_expression = true:suggestion
csharp_style_unused_value_assignment_preference = discard_variable:suggestion
csharp_style_unused_value_expression_statement_preference = discard_variable:silent

# 'using' directive preferences
csharp_using_directive_placement = outside_namespace:silent

#### C# Formatting Rules ####

# New line preferences
csharp_new_line_before_catch = true
csharp_new_line_before_else = true
csharp_new_line_before_finally = true
csharp_new_line_before_members_in_anonymous_types = true
csharp_new_line_before_members_in_object_initializers = true
csharp_new_line_before_open_brace = all
csharp_new_line_between_query_expression_clauses = true

# Indentation preferences
csharp_indent_block_contents = true
csharp_indent_braces = false
csharp_indent_case_contents = true
csharp_indent_case_contents_when_block = true
csharp_indent_labels = one_less_than_current
csharp_indent_switch_labels = true

# Space preferences
csharp_space_after_cast = false
csharp_space_after_colon_in_inheritance_clause = true
csharp_space_after_comma = true
csharp_space_after_dot = false
csharp_space_after_keywords_in_control_flow_statements = true
csharp_space_after_semicolon_in_for_statement = true
csharp_space_around_binary_operators = before_and_after
csharp_space_around_declaration_statements = false
csharp_space_before_colon_in_inheritance_clause = true
csharp_space_before_comma = false
csharp_space_before_dot = false
csharp_space_before_open_square_brackets = false
csharp_space_before_semicolon_in_for_statement = false
csharp_space_between_empty_square_brackets = false
csharp_space_between_method_call_empty_parameter_list_parentheses = false
csharp_space_between_method_call_name_and_opening_parenthesis = false
csharp_space_between_method_call_parameter_list_parentheses = false
csharp_space_between_method_declaration_empty_parameter_list_parentheses = false
csharp_space_between_method_declaration_name_and_open_parenthesis = false
csharp_space_between_method_declaration_parameter_list_parentheses = false
csharp_space_between_parentheses = false
csharp_space_between_square_brackets = false

# Wrapping preferences
csharp_preserve_single_line_blocks = true
csharp_preserve_single_line_statements = true

#### Naming styles ####
[*.{cs,vb}]

# Naming rules

dotnet_naming_rule.types_and_namespaces_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.types_and_namespaces_should_be_pascalcase.symbols = types_and_namespaces
dotnet_naming_rule.types_and_namespaces_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.interfaces_should_be_ipascalcase.severity = suggestion
dotnet_naming_rule.interfaces_should_be_ipascalcase.symbols = interfaces
dotnet_naming_rule.interfaces_should_be_ipascalcase.style = ipascalcase

dotnet_naming_rule.type_parameters_should_be_tpascalcase.severity = suggestion
dotnet_naming_rule.type_parameters_should_be_tpascalcase.symbols = type_parameters
dotnet_naming_rule.type_parameters_should_be_tpascalcase.style = tpascalcase

dotnet_naming_rule.methods_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.methods_should_be_pascalcase.symbols = methods
dotnet_naming_rule.methods_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.properties_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.properties_should_be_pascalcase.symbols = properties
dotnet_naming_rule.properties_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.events_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.events_should_be_pascalcase.symbols = events
dotnet_naming_rule.events_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.local_variables_should_be_camelcase.severity = suggestion
dotnet_naming_rule.local_variables_should_be_camelcase.symbols = local_variables
dotnet_naming_rule.local_variables_should_be_camelcase.style = camelcase

dotnet_naming_rule.local_constants_should_be_camelcase.severity = suggestion
dotnet_naming_rule.local_constants_should_be_camelcase.symbols = local_constants
dotnet_naming_rule.local_constants_should_be_camelcase.style = camelcase

dotnet_naming_rule.parameters_should_be_camelcase.severity = suggestion
dotnet_naming_rule.parameters_should_be_camelcase.symbols = parameters
dotnet_naming_rule.parameters_should_be_camelcase.style = camelcase

dotnet_naming_rule.public_fields_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.public_fields_should_be_pascalcase.symbols = public_fields
dotnet_naming_rule.public_fields_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.private_fields_should_be__camelcase.severity = suggestion
dotnet_naming_rule.private_fields_should_be__camelcase.symbols = private_fields
dotnet_naming_rule.private_fields_should_be__camelcase.style = _camelcase

dotnet_naming_rule.private_static_fields_should_be_s_camelcase.severity = suggestion
dotnet_naming_rule.private_static_fields_should_be_s_camelcase.symbols = private_static_fields
dotnet_naming_rule.private_static_fields_should_be_s_camelcase.style = s_camelcase

dotnet_naming_rule.public_constant_fields_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.public_constant_fields_should_be_pascalcase.symbols = public_constant_fields
dotnet_naming_rule.public_constant_fields_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.private_constant_fields_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.private_constant_fields_should_be_pascalcase.symbols = private_constant_fields
dotnet_naming_rule.private_constant_fields_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.public_static_readonly_fields_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.public_static_readonly_fields_should_be_pascalcase.symbols = public_static_readonly_fields
dotnet_naming_rule.public_static_readonly_fields_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.private_static_readonly_fields_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.private_static_readonly_fields_should_be_pascalcase.symbols = private_static_readonly_fields
dotnet_naming_rule.private_static_readonly_fields_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.enums_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.enums_should_be_pascalcase.symbols = enums
dotnet_naming_rule.enums_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.local_functions_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.local_functions_should_be_pascalcase.symbols = local_functions
dotnet_naming_rule.local_functions_should_be_pascalcase.style = pascalcase

dotnet_naming_rule.non_field_members_should_be_pascalcase.severity = suggestion
dotnet_naming_rule.non_field_members_should_be_pascalcase.symbols = non_field_members
dotnet_naming_rule.non_field_members_should_be_pascalcase.style = pascalcase

# Symbol specifications

dotnet_naming_symbols.interfaces.applicable_kinds = interface
dotnet_naming_symbols.interfaces.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.interfaces.required_modifiers = 

dotnet_naming_symbols.enums.applicable_kinds = enum
dotnet_naming_symbols.enums.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.enums.required_modifiers = 

dotnet_naming_symbols.events.applicable_kinds = event
dotnet_naming_symbols.events.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.events.required_modifiers = 

dotnet_naming_symbols.methods.applicable_kinds = method
dotnet_naming_symbols.methods.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.methods.required_modifiers = 

dotnet_naming_symbols.properties.applicable_kinds = property
dotnet_naming_symbols.properties.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.properties.required_modifiers = 

dotnet_naming_symbols.public_fields.applicable_kinds = field
dotnet_naming_symbols.public_fields.applicable_accessibilities = public, internal
dotnet_naming_symbols.public_fields.required_modifiers = 

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private, protected, protected_internal, private_protected
dotnet_naming_symbols.private_fields.required_modifiers = 

dotnet_naming_symbols.private_static_fields.applicable_kinds = field
dotnet_naming_symbols.private_static_fields.applicable_accessibilities = private, protected, protected_internal, private_protected
dotnet_naming_symbols.private_static_fields.required_modifiers = static

dotnet_naming_symbols.types_and_namespaces.applicable_kinds = namespace, class, struct, interface, enum
dotnet_naming_symbols.types_and_namespaces.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.types_and_namespaces.required_modifiers = 

dotnet_naming_symbols.non_field_members.applicable_kinds = property, event, method
dotnet_naming_symbols.non_field_members.applicable_accessibilities = public, internal, private, protected, protected_internal, private_protected
dotnet_naming_symbols.non_field_members.required_modifiers = 

dotnet_naming_symbols.type_parameters.applicable_kinds = namespace
dotnet_naming_symbols.type_parameters.applicable_accessibilities = *
dotnet_naming_symbols.type_parameters.required_modifiers = 

dotnet_naming_symbols.private_constant_fields.applicable_kinds = field
dotnet_naming_symbols.private_constant_fields.applicable_accessibilities = private, protected, protected_internal, private_protected
dotnet_naming_symbols.private_constant_fields.required_modifiers = const

dotnet_naming_symbols.local_variables.applicable_kinds = local
dotnet_naming_symbols.local_variables.applicable_accessibilities = local
dotnet_naming_symbols.local_variables.required_modifiers = 

dotnet_naming_symbols.local_constants.applicable_kinds = local
dotnet_naming_symbols.local_constants.applicable_accessibilities = local
dotnet_naming_symbols.local_constants.required_modifiers = const

dotnet_naming_symbols.parameters.applicable_kinds = parameter
dotnet_naming_symbols.parameters.applicable_accessibilities = *
dotnet_naming_symbols.parameters.required_modifiers = 

dotnet_naming_symbols.public_constant_fields.applicable_kinds = field
dotnet_naming_symbols.public_constant_fields.applicable_accessibilities = public, internal
dotnet_naming_symbols.public_constant_fields.required_modifiers = const

dotnet_naming_symbols.public_static_readonly_fields.applicable_kinds = field
dotnet_naming_symbols.public_static_readonly_fields.applicable_accessibilities = public, internal
dotnet_naming_symbols.public_static_readonly_fields.required_modifiers = readonly, static

dotnet_naming_symbols.private_static_readonly_fields.applicable_kinds = field
dotnet_naming_symbols.private_static_readonly_fields.applicable_accessibilities = private, protected, protected_internal, private_protected
dotnet_naming_symbols.private_static_readonly_fields.required_modifiers = readonly, static

dotnet_naming_symbols.local_functions.applicable_kinds = local_function
dotnet_naming_symbols.local_functions.applicable_accessibilities = *
dotnet_naming_symbols.local_functions.required_modifiers = 

# Naming styles

dotnet_naming_style.pascalcase.required_prefix = 
dotnet_naming_style.pascalcase.required_suffix = 
dotnet_naming_style.pascalcase.word_separator = 
dotnet_naming_style.pascalcase.capitalization = pascal_case

dotnet_naming_style.ipascalcase.required_prefix = I
dotnet_naming_style.ipascalcase.required_suffix = 
dotnet_naming_style.ipascalcase.word_separator = 
dotnet_naming_style.ipascalcase.capitalization = pascal_case

dotnet_naming_style.tpascalcase.required_prefix = T
dotnet_naming_style.tpascalcase.required_suffix = 
dotnet_naming_style.tpascalcase.word_separator = 
dotnet_naming_style.tpascalcase.capitalization = pascal_case

dotnet_naming_style._camelcase.required_prefix = _
dotnet_naming_style._camelcase.required_suffix = 
dotnet_naming_style._camelcase.word_separator = 
dotnet_naming_style._camelcase.capitalization = camel_case

dotnet_naming_style.camelcase.required_prefix = 
dotnet_naming_style.camelcase.required_suffix = 
dotnet_naming_style.camelcase.word_separator = 
dotnet_naming_style.camelcase.capitalization = camel_case

dotnet_naming_style.s_camelcase.required_prefix = s_
dotnet_naming_style.s_camelcase.required_suffix = 
dotnet_naming_style.s_camelcase.word_separator = 
dotnet_naming_style.s_camelcase.capitalization = camel_case


"""
        );

        var result = await new CsharpierProcess().WithArguments(fileOrDirectory).ExecuteAsync();

        result.ErrorOutput.Should().BeNullOrEmpty();
        result.Output.Should().StartWith("Formatted 1 files in ");
        result.ExitCode.Should().Be(0);
    }

    [TestCase("Subdirectory")]
    [TestCase("./Subdirectory")]
    public async Task Should_Format_Subdirectory(string subdirectory)
    {
        var formattedContent = "public class ClassName { }\n";
        var unformattedContent = "public class ClassName {\n\n}";

        await this.WriteFileAsync("Subdirectory/BasicFile.cs", unformattedContent);

        var result = await new CsharpierProcess().WithArguments(subdirectory).ExecuteAsync();

        result.Output.Should().StartWith("Formatted 1 files in ");
        result.ExitCode.Should().Be(0);
        (await this.ReadAllTextAsync("Subdirectory/BasicFile.cs")).Should().Be(formattedContent);
    }

    [Test]
    public async Task Should_Respect_Ignore_File_With_Subdirectory_When_DirectorOrFile_Is_Dot()
    {
        var unformattedContent = "public class Unformatted {     }";
        var filePath = "Subdirectory/IgnoredFile.cs";
        await this.WriteFileAsync(filePath, unformattedContent);
        await this.WriteFileAsync(".csharpierignore", filePath);

        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var result = await this.ReadAllTextAsync(filePath);

        result.Should().Be(unformattedContent, $"The file at {filePath} should have been ignored");
    }

    [Test]
    public async Task Should_Support_Config_Path()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = "TooWide.cs";
        await this.WriteFileAsync(fileName, fileContent);
        await this.WriteFileAsync("config/.csharpierrc", "printWidth: 10");

        await new CsharpierProcess()
            .WithArguments("--config-path config/.csharpierrc . ")
            .ExecuteAsync();

        var result = await this.ReadAllTextAsync(fileName);

        result.Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Should_Return_Error_When_No_DirectoryOrFile_And_Not_Piping_StdIn()
    {
        if (CannotRunTestWithRedirectedInput())
        {
            return;
        }

        var result = await new CsharpierProcess().ExecuteAsync();

        result.ExitCode.Should().Be(1);
        result
            .ErrorOutput.Should()
            .Contain("directoryOrFile is required when not piping stdin to CSharpier");
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Piped_File(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";

        var result = await new CsharpierProcess()
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Format_Piped_File_With_Config()
    {
        await this.WriteFileAsync(".csharpierrc", "printWidth: 10");

        var formattedContent1 = "var x =\n    _________________longName;\n";
        var unformattedContent1 = "var x = _________________longName;\n";

        var result = await new CsharpierProcess()
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Format_Piped_File_With_EditorConfig()
    {
        await this.WriteFileAsync(
            ".editorconfig",
            @"[*]
max_line_length = 10"
        );

        var formattedContent1 = "var x =\n    _________________longName;\n";
        var unformattedContent1 = "var x = _________________longName;\n";

        var result = await new CsharpierProcess()
            .WithPipedInput(unformattedContent1)
            .ExecuteAsync();

        result.Output.Should().Be(formattedContent1);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Format_Unicode()
    {
        // use the \u so that we don't accidentally reformat this to be '?'
        var unicodeContent = $"var test = '{'\u3002'}';\n";

        var result = await new CsharpierProcess().WithPipedInput(unicodeContent).ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.Should().Be(unicodeContent);
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Print_NotFound()
    {
        var result = await new CsharpierProcess().WithArguments("/BasicFile.cs").ExecuteAsync();

        result.Output.Should().BeEmpty();
        result
            .ErrorOutput.Should()
            .StartWith("There was no file or directory found at /BasicFile.cs");
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Should_Write_To_StdError_For_Piped_Invalid_File()
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess().WithPipedInput(invalidFile).ExecuteAsync();

        result.Output.Should().BeEmpty();
        result.ExitCode.Should().Be(1);
        result.ErrorOutput.Should().Contain("Failed to compile so was not formatted");
    }

    [Test]
    public async Task With_Check_Should_Write_Unformatted_File()
    {
        var unformattedContent = "public class ClassName1 {\n\n}";

        await this.WriteFileAsync("CheckUnformatted.cs", unformattedContent);

        var result = await new CsharpierProcess()
            .WithArguments("CheckUnformatted.cs --check")
            .ExecuteAsync();

        result
            .ErrorOutput.Replace("\\", "/")
            .Should()
            .StartWith("Error ./CheckUnformatted.cs - Was not formatted.");
        result.ExitCode.Should().Be(1);
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public async Task Should_Format_Multiple_Piped_Files(string lineEnding)
    {
        var formattedContent1 = "public class ClassName1 { }" + lineEnding;
        var formattedContent2 = "public class ClassName2 { }" + lineEnding;
        var unformattedContent1 = $"public class ClassName1 {{{lineEnding}{lineEnding}}}";
        var unformattedContent2 = $"public class ClassName2 {{{lineEnding}{lineEnding}}}";

        var input =
            $"Test1.cs{'\u0003'}{unformattedContent1}{'\u0003'}"
            + $"Test2.cs{'\u0003'}{unformattedContent2}{'\u0003'}";

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput(input)
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        var results = result.Output.Split('\u0003');
        results[0].Should().Be(formattedContent1);
        results[1].Should().Be(formattedContent2);
    }

    [TestCase("InvalidFile.cs", "./InvalidFile.cs")]
    [TestCase("./InvalidFile.cs", "./InvalidFile.cs")]
    public async Task Should_Write_Error_With_Multiple_Piped_Files(string input, string output)
    {
        const string invalidFile = "public class ClassName { ";

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"{input}{'\u0003'}{invalidFile}{'\u0003'}")
            .ExecuteAsync();

        result
            .ErrorOutput.Should()
            .StartWith(
                $"Error {output} - Failed to compile so was not formatted.{Environment.NewLine}  (1,26): error CS1513: }}"
            );
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Should_Ignore_Piped_File_With_Multiple_Piped_Files()
    {
        const string ignoredFile = "public class ClassName {     }";
        var fileName = Path.Combine(testFileDirectory, "Ignored.cs");
        await this.WriteFileAsync(".csharpierignore", "Ignored.cs");

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"{fileName}{'\u0003'}{ignoredFile}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().BeEmpty();
    }

    [Test]
    public async Task Should_Support_Config_With_Multiple_Piped_Files()
    {
        const string fileContent = "var myVariable = someLongValue;";
        var fileName = Path.Combine(testFileDirectory, "TooWide.cs");
        await this.WriteFileAsync(".csharpierrc", "printWidth: 10");

        var result = await new CsharpierProcess()
            .WithArguments("--pipe-multiple-files")
            .WithPipedInput($"{fileName}{'\u0003'}{fileContent}{'\u0003'}")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.Output.TrimEnd('\u0003').Should().Be("var myVariable =\n    someLongValue;\n");
    }

    [Test]
    public async Task Should_Not_Fail_On_Empty_File()
    {
        await this.WriteFileAsync("BasicFile.cs", "");

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();

        result.Output.Should().StartWith("Formatted 0 files in ");
        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
    }

    [Test]
    public async Task Should_Not_Fail_On_Bad_Csproj()
    {
        await this.WriteFileAsync("Empty.csproj", "");

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        result.Output.Should().StartWith("Warning The csproj at ");
    }

    [Test]
    public async Task Should_Not_Fail_On_Mismatched_MSBuild_With_No_Check()
    {
        await this.WriteFileAsync(
            "Test.csproj",
            @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""99"" />
    </ItemGroup>
</Project>"
        );

        var result = await new CsharpierProcess()
            .WithArguments("--no-msbuild-check .")
            .ExecuteAsync();

        result.ErrorOutput.Should().BeEmpty();
        result.ExitCode.Should().Be(0);
        result.Output.Should().StartWith("Formatted 0 files in ");
    }

    [Test]
    public async Task Should_Fail_On_Mismatched_MSBuild()
    {
        await this.WriteFileAsync(
            "Test.csproj",
            @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <PackageReference Include=""CSharpier.MsBuild"" Version=""99"" />
    </ItemGroup>
</Project>"
        );

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();

        result
            .ErrorOutput.Should()
            .Contain("uses version 99 of CSharpier.MsBuild which is a mismatch with version");
        result.ExitCode.Should().Be(1);
    }

    [Test]
    public async Task Should_Cache_And_Validate_Too_Many_Things()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var formattedContent = "public class ClassName { }\n";
        var filePath = "Unformatted.cs";
        await this.WriteFileAsync(filePath, unformattedContent);

        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var firstModifiedDate = GetLastWriteTime(filePath);
        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var secondModifiedDate = GetLastWriteTime(filePath);
        await this.WriteFileAsync(filePath, unformattedContent);
        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        var thirdModifiedDate = GetLastWriteTime(filePath);

        // I don't know that this exactly validates caching, because I don't think we write out a file unless it changes.
        firstModifiedDate.Should().Be(secondModifiedDate);
        secondModifiedDate.Should().BeBefore(thirdModifiedDate);

        (await this.ReadAllTextAsync(filePath)).Should().Be(formattedContent);
    }

    [Test]
    public async Task Should_Reformat_When_Options_Change_With_Cache()
    {
        var unformattedContent = "public class ClassName { \n// break\n }\n";

        await this.WriteFileAsync("Unformatted.cs", unformattedContent);
        await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        await this.WriteFileAsync(".csharpierrc", "useTabs: true");
        await new CsharpierProcess().WithArguments(".").ExecuteAsync();

        var result = await this.ReadAllTextAsync("Unformatted.cs");
        result.Should().Contain("\n\t// break\n");
    }

    [Test]
    public void Should_Handle_Concurrent_Processes()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var totalFolders = 10;
        var filesPerFolder = 100;
        var folders = new List<string>();
        for (var x = 0; x < totalFolders; x++)
        {
            folders.Add("Folder" + x);
        }

        async Task WriteFiles(string folder)
        {
            for (var y = 0; y < filesPerFolder; y++)
            {
                await this.WriteFileAsync($"{folder}/File{y}.cs", unformattedContent);
            }
        }

        var tasks = folders.Select(WriteFiles).ToArray();
        Task.WaitAll(tasks);

        async Task FormatFolder(string folder)
        {
            var result = await new CsharpierProcess().WithArguments(folder).ExecuteAsync();
            result.ErrorOutput.Should().BeEmpty();
        }

        var formatTasks = folders.Select(FormatFolder).ToArray();
        Task.WaitAll(formatTasks);
    }

    [Test]
    [Ignore(
        "This is somewhat useful for testing locally, but doesn't reliably reproduce a problem and takes a while to run. Commenting out the delete cache file line helps to reproduce problems"
    )]
    public async Task Should_Handle_Concurrent_Processes_2()
    {
        var unformattedContent = "public class ClassName {     }\n";
        var filesPerFolder = 1000;

        for (var x = 0; x < filesPerFolder; x++)
        {
            await this.WriteFileAsync($"{Guid.NewGuid()}.cs", unformattedContent);
        }

        var result = await new CsharpierProcess().WithArguments(".").ExecuteAsync();
        result.ErrorOutput.Should().BeEmpty();

        var newFiles = new List<string>();

        for (var x = 0; x < 100; x++)
        {
            var fileName = Guid.NewGuid() + ".cs";
            await this.WriteFileAsync(fileName, unformattedContent);
            newFiles.Add(fileName);
        }

        async Task FormatFile(string file)
        {
            var result = await new CsharpierProcess().WithArguments(file).ExecuteAsync();
            result.ErrorOutput.Should().BeEmpty();
        }

        var formatTasks = newFiles.Select(FormatFile).ToArray();
        Task.WaitAll(formatTasks);
    }

    private static bool CannotRunTestWithRedirectedInput()
    {
        // This test cannot run if Console.IsInputRedirected is true.
        // Running it from the command line is required.
        // See https://github.com/dotnet/runtime/issues/1147"
        return Console.IsInputRedirected;
    }

    private DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(Path.Combine(testFileDirectory, path));
    }

    private async Task WriteFileAsync(string path, string content)
    {
        var fileInfo = new FileInfo(Path.Combine(testFileDirectory, path));
        this.EnsureExists(fileInfo.Directory!);

        await File.WriteAllTextAsync(fileInfo.FullName, content);
    }

    private async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(Path.Combine(testFileDirectory, path));
    }

    private void EnsureExists(DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Parent != null)
        {
            this.EnsureExists(directoryInfo.Parent);
        }

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }

    private class CsharpierProcess
    {
        private readonly StringBuilder output = new();
        private readonly StringBuilder errorOutput = new();
        private Command command;

        private readonly Encoding encoding = Encoding.UTF8;

        public CsharpierProcess()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "dotnet-csharpier.dll");

            this.command = CliWrap
                .Cli.Wrap("dotnet")
                .WithArguments(path)
                .WithWorkingDirectory(testFileDirectory)
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(this.output, this.encoding))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(this.errorOutput, this.encoding));
        }

        public CsharpierProcess WithArguments(string arguments)
        {
            this.command = this.command.WithArguments(this.command.Arguments + " " + arguments);
            return this;
        }

        public CsharpierProcess WithPipedInput(string input)
        {
            this.command = this.command.WithStandardInputPipe(
                PipeSource.FromString(input, this.encoding)
            );

            return this;
        }

        public async Task<ProcessResult> ExecuteAsync()
        {
            var result = await this.command.ExecuteBufferedAsync(this.encoding);
            return new ProcessResult(
                this.output.ToString(),
                this.errorOutput.ToString(),
                result.ExitCode
            );
        }

        public record ProcessResult(string Output, string ErrorOutput, int ExitCode);
    }
}

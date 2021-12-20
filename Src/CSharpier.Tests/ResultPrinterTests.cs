using CSharpier.Cli;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ResultPrinterTests
{
    [Test]
    public void PadToSize_Should_Not_Error_When_Value_Larger_Than_Size()
    {
        ResultPrinter.PadToSize("1234567890", 9);
    }

    [Test]
    public void ReversePAs_Should_Not_Error_When_Value_Larger_Than_Size()
    {
        ResultPrinter.ReversePad("12345678901");
    }
}

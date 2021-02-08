#maybe run this before all tests somehow??
$testsRootDirectory = $PSScriptRoot
$testFilesDirectory = Join-Path $testsRootDirectory "TestFiles"
Write-Output $testFilesDirectory
foreach($directory in Get-ChildItem $testFilesDirectory -Directory) 
{
    $output = ""
    $output += "using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{
    public class $($directory.Name)Tests : BaseTest
    {
"

    foreach($file in $directory.GetFiles())
    {
        if (-not $file.Name.Contains(".cst") -or $file.Name.Contains(".actual") -or $file.Name.Contains(".expected"))
        {
            continue;
        }

        $testName = $file.Name.Replace(".cst", "");
        $output += "        [Test]
        public void $($testName)()
        {
            this.RunTest(`"$($directory.Name)`", `"$($testName)`");
        }
"
    }

    $output += "    }
}"
    
    $fileName = Join-Path $directory.FullName "_$($directory.Name)Tests.cs"
    Set-Content $fileName $output
    
}


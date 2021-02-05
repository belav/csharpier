using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Worker
{
    public class GenerateUnitTests
    {
        // TODO make this powershell that runs on build?
        [Test]
        public void DoWork()
        {
            var rootDirectory = new DirectoryInfo(@"C:\Projects\csharpier");
            var csharpDirectory = Path.Combine(rootDirectory.FullName, @"CSharpier\CSharpier.Tests\TestFiles");
            foreach (var directory in new DirectoryInfo(csharpDirectory).GetDirectories())
            {
                var output = new StringBuilder();

                output.AppendLine(@$"using CSharpier.Tests.TestFileTests;
using NUnit.Framework;

namespace CSharpier.Tests.TestFiles
{{
    public class {directory.Name}Tests : BaseTest
    {{");


                foreach (var file in directory.GetFiles())
                {
                    if (!file.Name.Contains(".cst") || file.Name.Contains(".actual") || file.Name.Contains(".expected"))
                    {
                        continue;
                    }

                    var testName = file.Name.Replace(".cst", "");
                    output.AppendLine(@$"        [Test]
        public void {testName}()
        {{
            this.RunTest(""{directory.Name}"", ""{testName}"");
        }}");
                }

                output.AppendLine("    }");
                output.AppendLine("}");
                
                File.WriteAllText(Path.Combine(directory.FullName, "_" + directory.Name + "Tests.cs"), output.ToString());
            }
        }
    }
}
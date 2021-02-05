using System.IO;
using System.Text;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class Samples
    {
        [Test]
        public void AllInOne()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "CSharpier.Tests")
            {
                directory = directory.Parent;
            }

            var file = Path.Combine(directory.FullName, "Samples/AllInOne.cst");
            var code = File.ReadAllText(file);
            var formattedCode = new Formatter().Format(code, new Options());
            
            // TODO what about BOM? keep it if incoming file?
            File.WriteAllText(file.Replace(".cst", ".Formatted.cst"), formattedCode, new UTF8Encoding(false));
        }
    }
}
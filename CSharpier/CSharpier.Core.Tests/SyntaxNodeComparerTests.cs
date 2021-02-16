using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace CSharpier.Core.Tests
{
    public class SyntaxNodeComparerTests
    {
        [Test]
        public void Class_Not_Equal_Namespace()
        {
            var left = "class ClassName { }";
            var right = @"namespace Namespace { }";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().Be("Root-Members[0]");
        }
        
        [Test]
        public void Class_Not_Equal_Class_Different_Whitespace()
        {
            var left = "class ClassName { }";
            var right = @"class ClassName {
}";

            var result = this.AreEqual(left, right);
            
            result.MismatchedPath.Should().Be(null);
        }

        [Test]
        public void Missing_Attribute_Should_Mean_False()
        {
            var left = @"class Resources
{
    [Obsolete]
    public Resources()
    {
    }
}
";
            var right = @"class Resources
{
    public Resources() { }
}
";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().Be("Root-Members[0]-Members[0]-AttributeLists-Count");
        }

        [Test]
        public void SeperatedSyntaxLists()
        {
            var left = @"namespace Insite.Automated.Core
{
    using System;

    public class DropdownAttribute : Attribute
    {
        public bool IgnoreIfNotPresent { get; set; }

        public DropdownAttribute()
        {
        }

        public DropdownAttribute(bool ignoreIfNotPresent)
        {
            this.IgnoreIfNotPresent = ignoreIfNotPresent;
        }
    }
}";

            var right = @"namespace Insite.Automated.Core
{
    using System;

    public class DropdownAttribute : Attribute
    {
        public bool IgnoreIfNotPresent { get; set; }

        public DropdownAttribute() { }

        public DropdownAttribute(bool ignoreIfNotPresent)
        {
            this.IgnoreIfNotPresent = ignoreIfNotPresent;
        }
    }
}
";

            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().BeNull();
        }

        [Test]
        public void Blah()
        {
            var left = @"namespace Insite.Automated.Core
{
    using System;
    using System.IO;
    using System.Text;
    using Insite.Data.Entities;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class LogHelper
    {
        private static object stringLock = new object();
        private static StringBuilder stringBuilder = new StringBuilder();
        private static DateTime lastLogDateTime = DateTime.Now;

        public static void LogAction(object parameters = null, [CallerMemberName] string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($""{nameof(methodName)} must be provided; it will be provided automatically by the compiler if not explicitly set."", nameof(methodName));
            }

            var message = methodName;
            if (parameters != null)
            {
                message += "" - "";
                if (parameters is string)
                {
                    message += parameters;
                }
                else
                {
                    foreach (var property in parameters.GetType().GetProperties())
                    {
                        message += property.Name + "": "" + GetLogValue(property.GetValue(parameters)) + "" "";
                    }   
                }
            }

            Log(message);
        }

        private static string GetLogValue(object property)
        {
            if (property == null)
            {
                return ""[null]"";
            }

            if (property is EntityBase)
            {
                return ((EntityBase) property).Id.ToString();
            }

            if (property is ILoggable)
            {
                return ((ILoggable) property).GetLogValue();
            }

            return property.ToString();
        }

        public static void Log(string message)
        {
            Log(""Info"", message);
        }

        public static void LogDebug(string message)
        {
            Log(""Debug"", message);
        }

        public static void LogSubaction(string message)
        {
            Log(""Info"", ""   "" + message);
        }

        public static void Log(string type, string message)
        {
            while (type.Length < 10)
            {
                type += "" "";
            }

            var dateTime = DateTime.Now;
            var elapsedMilliseconds = Math.Truncate((dateTime - lastLogDateTime).TotalMilliseconds) + ""ms"";
            while (elapsedMilliseconds.Length < 10)
            {
                elapsedMilliseconds = "" "" + elapsedMilliseconds;
            }

            lock (stringLock)
            {
                stringBuilder.AppendLine(dateTime.ToString(""HH:mm:ss.fff"") + "" "" + elapsedMilliseconds + ""   "" + type + message);    
            }
            
            lastLogDateTime = dateTime;
        }

        public static void Reset()
        {
            lock (stringLock)
            {
                stringBuilder = new StringBuilder();
            }
        }

        private static readonly object LogLock = new object();

        public static void WriteLog(bool writeIndividualLog = true)
        {
            lock (stringLock)
            {
                lock (LogLock)
                {
                    File.AppendAllText(FileHelper.GetRootArtifactFilePath($""log-{FileHelper.ProjectName()}.txt""), stringBuilder + Environment.NewLine);
                }

                if (writeIndividualLog)
                {
                    var secondRunFile = FileHelper.ValidateFilePath($""TeamCitySecondRun.txt"", FileHelper.BuildArtifactsPath); // TeamCity build (Run Tests step) will create this file if it's doing the 2nd run
                    var logFilePath = FileHelper.GetArtifactFilePathForCurrentTest(""log.txt"");
                    var logFilePath2ndRun = FileHelper.GetArtifactFilePathForCurrentTest(""log2.txt"");

                    if (!File.Exists(secondRunFile))
                    {
                        File.WriteAllText(logFilePath, stringBuilder.ToString());
                        File.Delete(logFilePath2ndRun);
                        Console.WriteLine($""##teamcity[testMetadata name='Log ({stringBuilder.Length})' type='artifact' value='{FileHelper.GetArtifactFilePathForCurrentTest(""log.txt"", true)}']"");
                    }
                    else
                    {
                        File.WriteAllText(logFilePath2ndRun, stringBuilder.ToString());
                        Console.WriteLine($""##teamcity[testMetadata name='Log2 ({stringBuilder.Length})' type='artifact' value='{FileHelper.GetArtifactFilePathForCurrentTest(""log2.txt"", true)}']"");
                    }
                }
            }

            Reset();
        }

        public static void InsertBlank()
        {
            lock (stringLock)
            {
                stringBuilder.AppendLine("""");
            }
        }
    }

    public interface ILoggable
    {
        string GetLogValue();
    }
}";

            var right = @"namespace Insite.Automated.Core
{
    using System;
    using System.IO;
    using System.Text;
    using Insite.Data.Entities;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class LogHelper
    {
        private static object stringLock = new object();
        private static StringBuilder stringBuilder = new StringBuilder();
        private static DateTime lastLogDateTime = DateTime.Now;

        public static void LogAction(
            object parameters = null,
            [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException(
                    $""{nameof(methodName)} must be provided; it will be provided automatically by the compiler if not explicitly set."",
                    nameof(methodName));
            }

            var message = methodName;
            if (parameters != null)
            {
                message += "" - "";
                if (parameters is string)
                {
                    message += parameters;
                }
                else
                {
                    foreach (var property in parameters.GetType().GetProperties())
                    {
                        message += property.Name + "": "" + GetLogValue(
                            property.GetValue(parameters)) + "" "";
                    }
                }
            }

            Log(message);
        }

        private static string GetLogValue(object property)
        {
            if (property == null)
            {
                return ""[null]"";
            }

            if (property is EntityBase)
            {
                return ((EntityBase)property).Id.ToString();
            }

            if (property is ILoggable)
            {
                return ((ILoggable)property).GetLogValue();
            }

            return property.ToString();
        }

        public static void Log(string message)
        {
            Log(""Info"", message);
        }

        public static void LogDebug(string message)
        {
            Log(""Debug"", message);
        }

        public static void LogSubaction(string message)
        {
            Log(""Info"", ""   "" + message);
        }

        public static void Log(string type, string message)
        {
            while (type.Length < 10)
            {
                type += "" "";
            }

            var dateTime = DateTime.Now;
            var elapsedMilliseconds = Math.Truncate(
                (dateTime - lastLogDateTime).TotalMilliseconds) + ""ms"";
            while (elapsedMilliseconds.Length < 10)
            {
                elapsedMilliseconds = "" "" + elapsedMilliseconds;
            }
            lock (stringLock)
            {
                stringBuilder.AppendLine(
                    dateTime.ToString(""HH:mm:ss.fff"") + "" "" + elapsedMilliseconds + ""   "" + type + message);
            }

            lastLogDateTime = dateTime;
        }

        public static void Reset()
        {
            lock (stringLock)
            {
                stringBuilder = new StringBuilder();
            }
        }

        private static readonly object LogLock = new object();

        public static void WriteLog(bool writeIndividualLog = true)
        {
            lock (stringLock)
            {
                lock (LogLock)
                {
                    File.AppendAllText(
                        FileHelper.GetRootArtifactFilePath(
                            $""log-{FileHelper.ProjectName()}.txt""),
                        stringBuilder + Environment.NewLine);
                }

                if (writeIndividualLog)
                {
                    var secondRunFile = FileHelper.ValidateFilePath(
                        $""TeamCitySecondRun.txt"",
                        FileHelper.BuildArtifactsPath); // TeamCity build (Run Tests step) will create this file if it's doing the 2nd run
                    var logFilePath = FileHelper.GetArtifactFilePathForCurrentTest(
                        ""log.txt"");
                    var logFilePath2ndRun = FileHelper.GetArtifactFilePathForCurrentTest(
                        ""log2.txt"");

                    if (!File.Exists(secondRunFile))
                    {
                        File.WriteAllText(
                            logFilePath,
                            stringBuilder.ToString());
                        File.Delete(logFilePath2ndRun);
                        Console.WriteLine(
                            $""##teamcity[testMetadata name='Log ({stringBuilder.Length})' type='artifact' value='{FileHelper.GetArtifactFilePathForCurrentTest(
                                ""log.txt"",
                                true)}']"");
                    }
                    else
                    {
                        File.WriteAllText(
                            logFilePath2ndRun,
                            stringBuilder.ToString());
                        Console.WriteLine(
                            $""##teamcity[testMetadata name='Log2 ({stringBuilder.Length})' type='artifact' value='{FileHelper.GetArtifactFilePathForCurrentTest(
                                ""log2.txt"",
                                true)}']"");
                    }
                }
            }

            Reset();
        }

        public static void InsertBlank()
        {
            lock (stringLock)
            {
                stringBuilder.AppendLine("""");
            }
        }
    }

    public interface ILoggable
    {
        string GetLogValue();
    }
}
";
            var result = this.AreEqual(left, right);

            result.MismatchedPath.Should().BeNull();
        }

        private AreEqualResult AreEqual(string left, string right)
        {
            var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp9);
            var leftNode = CSharpSyntaxTree.ParseText(left, cSharpParseOptions);
            var rightNode = CSharpSyntaxTree.ParseText(right, cSharpParseOptions);


            return new SyntaxNodeComparer().AreEqualIgnoringWhitespace(leftNode.GetRoot(), rightNode.GetRoot(), "Root");
        }
    }
}
namespace CSharpier.VisualStudio
{
    public interface ICSharpierProcess
    {
        string Version { get; }
        bool ProcessFailedToStart { get; }
        string FormatFile(string content, string fileName);
        void Dispose();
    }

    public interface ICSharpierProcess2 : ICSharpierProcess
    {
        FormatFileResult? formatFile(FormatFileParameter parameter);
    }

    public class FormatFileParameter
    {
        public string fileContents;
        public string fileName;
    }

    public class FormatFileResult
    {
        public string formattedFile;
        public Status status;
        public string errorMessage;
    }

    public enum Status
    {
        Formatted,
        Ignored,
        Failed,
    }
}

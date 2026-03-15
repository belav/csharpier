using System.IO.Abstractions;
using LibGit2Sharp;

namespace CSharpier.Tests.Cli;

public class GitRepository
{
    private readonly IFileSystem fileSystem;
    public string RepoPath { get; }

    public GitRepository(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem;
        var tempPath = this.fileSystem.Path.GetTempPath();
        this.RepoPath = this.fileSystem.Path.Combine(tempPath, $"fakeRepo-{Guid.NewGuid()}");
        if (this.fileSystem.Directory.Exists(this.RepoPath))
        {
            this.fileSystem.Directory.Delete(this.RepoPath, true);
        }

        this.fileSystem.Directory.CreateDirectory(this.RepoPath);

        Repository.Init(this.RepoPath);
    }

    public void AddUntrackedFileToRepo(string relativePath, int numLines = 1)
    {
        this.AddUntrackedFileToRepo(
            relativePath,
            string.Concat(Enumerable.Repeat($"Fake content line.{Environment.NewLine}", numLines))
        );
    }

    public void AddTrackedFileToRepo(string relativePath, string content)
    {
        this.AddUntrackedFileToRepo(relativePath, content);
        this.CommitPathToRepo(relativePath);
    }

    public void AddUntrackedDirToRepo(string relativePath)
    {
        var filePath = this.fileSystem.Path.Combine(this.RepoPath, relativePath);
        var directoryInfo = Directory.GetParent(filePath);
        Directory.CreateDirectory(directoryInfo!.FullName);
    }

    public IEnumerable<StatusEntry> GetUntrackedFiles()
    {
        using var repo = new Repository(this.RepoPath);
        var status = repo.RetrieveStatus();
        return status.Untracked;
    }

    public void AddUntrackedFileToRepo(string relativePath, string content)
    {
        var filePath = this.fileSystem.Path.Combine(this.RepoPath, relativePath);
        var directoryInfo = Directory.GetParent(filePath);
        Directory.CreateDirectory(directoryInfo!.FullName);
        this.fileSystem.File.WriteAllText(
            this.fileSystem.Path.Combine(this.RepoPath, relativePath),
            content
        );
    }

    public void DeleteRepoDirectory()
    {
        var dirInfo = this.fileSystem.DirectoryInfo.New(this.RepoPath);
        if (dirInfo.Exists)
        {
            SetNormalAttribute(dirInfo);
        }

        dirInfo.Delete(true);
    }

    private static void SetNormalAttribute(IDirectoryInfo dirInfo)
    {
        foreach (var subDir in dirInfo.GetDirectories())
        {
            SetNormalAttribute(subDir);
        }

        foreach (var file in dirInfo.GetFiles())
        {
            file.Attributes = FileAttributes.Normal;
        }
    }

    private void CommitPathToRepo(string relativePath)
    {
        var repo = new Repository(this.RepoPath);
        Commands.Stage(repo, relativePath);
        var author = new Signature("FakeUser", "fakeemail@test.com", DateTimeOffset.Now);
        repo.Commit("Adding files", author, author);
    }
}

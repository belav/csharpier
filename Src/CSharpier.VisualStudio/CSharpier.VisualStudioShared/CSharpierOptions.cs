namespace CSharpier.VisualStudio
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Threading;
    using Newtonsoft.Json;
    using AsyncServiceProvider = Microsoft.VisualStudio.Shell.AsyncServiceProvider;
    using ThreadHelper = Microsoft.VisualStudio.Shell.ThreadHelper;

    public class CSharpierOptions
    {
        [Category("CSharpier - Solution")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description(
            "Reformat with CSharpier on Save - this option is saved locally to the solution"
        )]
        public bool? SolutionRunOnSave { get; set; }

        [Category("CSharpier - Global")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description(
            "Reformat with CSharpier on Save - this option is saved globally to this computer"
        )]
        public bool? GlobalRunOnSave { get; set; }

        [Category("CSharpier - Global")]
        [DisplayName("Log Debug Messages")]
        [Description("Log Debug Messages - this option is saved globally to this computer")]
        public bool GlobalLogDebugMessages { get; set; }

        [Category("CSharpier - Developer")]
        [DisplayName("Custom Path")]
        [Description(
            "Custom Path - Path to directory containing dotnet-csharpier - used for testing the extension with new versions of csharpier."
        )]
        public string? CustomPath { get; set; }

        [Category("CSharpier - Developer")]
        [DisplayName("Disable CSharpier Server")]
        [Description(
            "Disable CSharpier Server - Use the legacy version of piping stdin to csharpier for formatting files."
        )]
        public bool DisableCSharpierServer { get; set; }

        protected void LoadFrom(CSharpierOptions newInstance)
        {
            this.SolutionRunOnSave = newInstance.SolutionRunOnSave;
            this.GlobalRunOnSave = newInstance.GlobalRunOnSave;
            this.GlobalLogDebugMessages = newInstance.GlobalLogDebugMessages;
            this.CustomPath = newInstance.CustomPath;
            this.DisableCSharpierServer = newInstance.DisableCSharpierServer;
        }

        private static readonly AsyncLazy<CSharpierOptions> liveModel = new(
            CreateAsync,
            ThreadHelper.JoinableTaskFactory
        );

        private static FileSystemWatcher _hotReloadWatcher;

        public static CSharpierOptions Instance
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return ThreadHelper.JoinableTaskFactory.Run(liveModel.GetValueAsync);
            }
        }

        public static Task<CSharpierOptions> GetLiveInstanceAsync() => liveModel.GetValueAsync();

        private static async Task<CSharpierOptions> CreateAsync()
        {
            var instance = new CSharpierOptions();
            await instance.LoadAsync();
            await InitializeHotReloadWatcherAsync();
            return instance;
        }

        public void Load()
        {
            ThreadHelper.JoinableTaskFactory.Run(LoadAsync);
        }

        private async Task LoadAsync()
        {
            var newInstance = new CSharpierOptions();

            async Task LoadOptionsFromFile(
                Func<Task<string?>> getFilePath,
                Action<OptionsDto> doStuff
            )
            {
                try
                {
                    var filePath = await getFilePath();
                    if (filePath == null || !File.Exists(filePath))
                    {
                        return;
                    }

                    using var fileStream = File.Open(filePath, FileMode.Open);
                    var result = new byte[fileStream.Length];
                    await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
                    var json = Encoding.UTF8.GetString(result);
                    var optionsDto = JsonConvert.DeserializeObject<OptionsDto>(json)!;
                    doStuff(optionsDto);
                }
                catch (Exception e)
                {
                    Logger.Instance.Error(e);
                }
            }

            await LoadOptionsFromFile(
                GetSolutionOptionsFileNameAsync,
                o =>
                {
                    newInstance.SolutionRunOnSave = o.RunOnSave;
                }
            );

            await LoadOptionsFromFile(
                this.GetGlobalOptionsFileNameAsync,
                o =>
                {
                    newInstance.GlobalRunOnSave = o.RunOnSave;
                    newInstance.GlobalLogDebugMessages = o.LogDebugMessages;
                    newInstance.CustomPath = o.CustomPath;
                    newInstance.DisableCSharpierServer = o.DisableCSharpierServer;
                }
            );

            this.LoadFrom(newInstance);
        }

        public void Save()
        {
            ThreadHelper.JoinableTaskFactory.Run(SaveAsync);
        }

        public async Task SaveAsync()
        {
            async Task SaveOptions(Func<Task<string?>> getFilePath, OptionsDto optionsDto)
            {
                try
                {
                    var filePath = await getFilePath();
                    if (filePath == null)
                    {
                        return;
                    }
                    var json = JsonConvert.SerializeObject(optionsDto);
                    var encodedText = Encoding.UTF8.GetBytes(json);

                    using var fileStream = new FileStream(
                        filePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        useAsync: true
                    );

                    await fileStream.WriteAsync(encodedText, 0, encodedText.Length);
                }
                catch (Exception e)
                {
                    Logger.Instance.Error(e);
                }
            }

            await SaveOptions(
                GetSolutionOptionsFileNameAsync,
                new OptionsDto { RunOnSave = this.SolutionRunOnSave }
            );

            await SaveOptions(
                this.GetGlobalOptionsFileNameAsync,
                new OptionsDto
                {
                    RunOnSave = this.GlobalRunOnSave,
                    LogDebugMessages = this.GlobalLogDebugMessages,
                    CustomPath = this.CustomPath,
                    DisableCSharpierServer = this.DisableCSharpierServer,
                }
            );
        }

        private Task<string?> GetGlobalOptionsFileNameAsync()
        {
            return Task.FromResult<string?>(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CSharpier",
                    "vs.json"
                )
            );
        }

        private static async Task<string?> GetSolutionOptionsFileNameAsync()
        {
#pragma warning disable VSSDK006
            var solution =
                await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(SVsSolution))
                as IVsSolution;
#pragma warning restore
            solution!.GetSolutionInfo(out _, out _, out var userOptsFile);

            return userOptsFile != null
                ? Path.Combine(Path.GetDirectoryName(userOptsFile), "csharpier.json")
                : null;
        }

        private static async Task InitializeHotReloadWatcherAsync()
        {
            string filePath = await GetSolutionOptionsFileNameAsync();

            _hotReloadWatcher = new FileSystemWatcher(
                Path.GetDirectoryName(filePath),
                Path.GetFileName(filePath)
            );

            static void OnFileChanged(object sender, FileSystemEventArgs e)
            {
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    await Instance.LoadAsync();
                });
#pragma warning restore
            }

            _hotReloadWatcher.Changed += OnFileChanged;
            _hotReloadWatcher.Created += OnFileChanged;
            _hotReloadWatcher.Renamed += OnFileChanged;

            _hotReloadWatcher.EnableRaisingEvents = true;
        }

        private class OptionsDto
        {
            public bool? RunOnSave { get; set; }
            public bool LogDebugMessages { get; set; }
            public string? CustomPath { get; set; }
            public bool DisableCSharpierServer { get; set; }
        }
    }
}

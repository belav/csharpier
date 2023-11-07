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

        protected void LoadFrom(CSharpierOptions newInstance)
        {
            this.SolutionRunOnSave = newInstance.SolutionRunOnSave;
            this.GlobalRunOnSave = newInstance.GlobalRunOnSave;
            this.GlobalLogDebugMessages = newInstance.GlobalLogDebugMessages;
        }

        private static readonly AsyncLazy<CSharpierOptions> liveModel =
            new(CreateAsync, ThreadHelper.JoinableTaskFactory);

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
                this.GetSolutionOptionsFileNameAsync,
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
                this.GetSolutionOptionsFileNameAsync,
                new OptionsDto { RunOnSave = this.SolutionRunOnSave, }
            );

            await SaveOptions(
                this.GetGlobalOptionsFileNameAsync,
                new OptionsDto
                {
                    RunOnSave = this.GlobalRunOnSave,
                    LogDebugMessages = this.GlobalLogDebugMessages
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

        private async Task<string?> GetSolutionOptionsFileNameAsync()
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

        private class OptionsDto
        {
            public bool? RunOnSave { get; set; }
            public bool LogDebugMessages { get; set; }
        }
    }
}

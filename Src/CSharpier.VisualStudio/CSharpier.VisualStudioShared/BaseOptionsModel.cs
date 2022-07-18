namespace CSharpier.VisualStudio
{
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Threading;

    public abstract class BaseOptionModel<T> where T : BaseOptionModel<T>, new()
    {
        private static readonly AsyncLazy<T> liveModel = new AsyncLazy<T>(
            CreateAsync,
            ThreadHelper.JoinableTaskFactory
        );

        public static T Instance
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return ThreadHelper.JoinableTaskFactory.Run(liveModel.GetValueAsync);
            }
        }

        public static Task<T> GetLiveInstanceAsync() => liveModel.GetValueAsync();

        private static async Task<T> CreateAsync()
        {
            var instance = new T();
            await instance.LoadAsync();
            return instance;
        }

        public void Load()
        {
            ThreadHelper.JoinableTaskFactory.Run(this.LoadAsync);
        }

        protected abstract Task LoadAsync();

        public void Save()
        {
            ThreadHelper.JoinableTaskFactory.Run(this.SaveAsync);
        }

        protected abstract Task SaveAsync();

        protected async Task<string?> GetLocalOptionsFileNameAsync()
        {
            var solution =
                await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(SVsSolution))
                as IVsSolution;
            solution.GetSolutionInfo(out _, out _, out var userOptsFile);

            return userOptsFile != null
                ? Path.Combine(Path.GetDirectoryName(userOptsFile), "csharpier.json")
                : null;
        }

        protected async Task<string> GetGlobalOptionsFileNameAsync()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CSharpier",
                "vs.json"
            );
        }
    }
}

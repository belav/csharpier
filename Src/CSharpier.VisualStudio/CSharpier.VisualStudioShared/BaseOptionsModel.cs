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
    using Task = System.Threading.Tasks.Task;

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
            ThreadHelper.JoinableTaskFactory.Run(LoadAsync);
        }

        private async Task LoadAsync()
        {
            var newInstance = new T();
            try
            {
                var fileName = await this.GetOptionsFileNameAsync();
                if (!File.Exists(fileName))
                {
                    this.LoadFrom(newInstance);
                    return;
                }

                using var fileStream = File.Open(fileName, FileMode.Open);
                var result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
                var json = Encoding.UTF8.GetString(result);
                newInstance = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e);
            }

            this.LoadFrom(newInstance);
        }

        protected abstract void LoadFrom(T newInstance);

        public void Save()
        {
            ThreadHelper.JoinableTaskFactory.Run(SaveAsync);
        }

        public async Task SaveAsync()
        {
            try
            {
                var fileName = await this.GetOptionsFileNameAsync();
                if (fileName == null)
                {
                    return;
                }
                var json = JsonConvert.SerializeObject(this);
                var encodedText = Encoding.UTF8.GetBytes(json);

                using var fileStream = new FileStream(
                    fileName,
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

        private async Task<string> GetOptionsFileNameAsync()
        {
            var solution =
                await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(SVsSolution))
                as IVsSolution;
            solution.GetSolutionInfo(out _, out _, out var userOptsFile);

            if (userOptsFile != null)
            {
                return Path.Combine(Path.GetDirectoryName(userOptsFile), "csharpier.json");
            }

            return null;
        }
    }
}

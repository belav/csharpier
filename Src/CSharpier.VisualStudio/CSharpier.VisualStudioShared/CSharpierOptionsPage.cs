using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpier.VisualStudio
{
    public class CSharpierOptionsPage : BaseOptionPage<CSharpierOptions> { }

    public class CSharpierOptions : BaseOptionModel<CSharpierOptions>
    {
        private PersistedSettings? localSettings;
        private PersistedSettings globalSettings;

        private PersistedSettings CurrentSettings =>
            this.OptionStorageType == StorageType.Global ? this.globalSettings : this.localSettings;

        private PersistedSettings GetCurrentSettings()
        {
            if (this.OptionStorageType == StorageType.Local)
            {
                this.localSettings = new PersistedSettings();
            }

            return this.OptionStorageType == StorageType.Global
                ? this.globalSettings
                : this.localSettings;
        }

        [Category("CSharpier")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description("Reformat with CSharpier on Save")]
        public bool RunOnSave
        {
            get => this.CurrentSettings == null ? false : this.CurrentSettings.RunOnSave;
            set => this.GetCurrentSettings().RunOnSave = value;
        }

        [Category("CSharpier")]
        [DisplayName("Log Debug Messages")]
        [Description("Log Debug Messages")]
        public bool LogDebugMessages
        {
            get => this.CurrentSettings == null ? false : this.CurrentSettings.LogDebugMessages;
            set => this.GetCurrentSettings().LogDebugMessages = value;
        }

        [Category("CSharpier")]
        [DisplayName("Storage type")]
        [Description(
            "Global options (same for all solutions) or local options (specific for each solution)."
        )]
        public StorageType OptionStorageType { get; set; }

        protected override async Task LoadAsync()
        {
            this.localSettings = await this.LoadSettings(this.GetLocalOptionsFileNameAsync);
            this.globalSettings =
                await this.LoadSettings(this.GetGlobalOptionsFileNameAsync)
                ?? new PersistedSettings();
        }

        private async Task<PersistedSettings?> LoadSettings(Func<Task<string?>> getFileNameAsync)
        {
            try
            {
                var fileName = await getFileNameAsync();
                if (!File.Exists(fileName))
                {
                    return null;
                }

                using var fileStream = File.Open(fileName, FileMode.Open);
                var result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
                var json = Encoding.UTF8.GetString(result);
                return JsonConvert.DeserializeObject<PersistedSettings>(json);
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e);
            }

            return new PersistedSettings();
        }

        protected override async Task SaveAsync()
        {
            await this.SaveSettingsAsync(this.GetLocalOptionsFileNameAsync, this.localSettings);
            await this.SaveSettingsAsync(this.GetGlobalOptionsFileNameAsync, this.globalSettings);
        }

        protected async Task SaveSettingsAsync(
            Func<Task<string?>> getFileNameAsync,
            PersistedSettings settings
        )
        {
            if (settings == null)
            {
                return;
            }

            try
            {
                var fileName = await getFileNameAsync();
                if (fileName == null)
                {
                    return;
                }
                var json = JsonConvert.SerializeObject(settings);
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
    }

    public class PersistedSettings
    {
        public bool RunOnSave { get; set; }
        public bool LogDebugMessages { get; set; }
    }

    public enum StorageType
    {
        Global,
        Local
    }
}

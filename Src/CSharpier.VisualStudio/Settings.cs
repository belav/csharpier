using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;

namespace CSharpier.VisualStudio
{
    public class Settings
    {
        private readonly WritableSettingsStore userSettingsStore;

        private Settings(WritableSettingsStore userSettingsStore)
        {
            this.userSettingsStore = userSettingsStore;
        }

        public static Settings Instance { get; private set; }

        public static Task InitializeAsync(CSharpierPackage package)
        {
            var settingsManager = new ShellSettingsManager(package);
            var userSettingsStore = settingsManager.GetWritableSettingsStore(
                SettingsScope.UserSettings
            );
            if (!userSettingsStore.CollectionExists("csharpier"))
            {
                userSettingsStore.CreateCollection("csharpier");
                userSettingsStore.SetBoolean("csharpier", "RunOnSave", false);
            }

            Instance = new Settings(userSettingsStore);

            return Task.CompletedTask;
        }

        public bool RunOnSave
        {
            get => userSettingsStore.GetBoolean("csharpier", "RunOnSave");
            set => userSettingsStore.SetBoolean("csharpier", "RunOnSave", value);
        }
    }
}

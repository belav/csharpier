namespace CSharpier.VisualStudio
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    using Microsoft;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Shell.Settings;
    using Microsoft.VisualStudio.Threading;

    using Task = System.Threading.Tasks.Task;

    public abstract class BaseOptionModel<T> where T : BaseOptionModel<T>, new()
    {
        private static readonly AsyncLazy<T> liveModel = new AsyncLazy<T>(
            CreateAsync,
            ThreadHelper.JoinableTaskFactory
        );
        private static AsyncLazy<ShellSettingsManager> settingsManager =
            new AsyncLazy<ShellSettingsManager>(
                GetSettingsManagerAsync,
                ThreadHelper.JoinableTaskFactory
            );
        public static event EventHandler<EventArgs>? Saved;

        protected BaseOptionModel() { }

        public static T Instance
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return ThreadHelper.JoinableTaskFactory.Run(GetLiveInstanceAsync);
            }
        }

        public static Task<T> GetLiveInstanceAsync() => liveModel.GetValueAsync();

        public static async Task<T> CreateAsync()
        {
            var instance = new T();
            await instance.LoadAsync();
            return instance;
        }

        protected string CollectionName { get; } = typeof(T).FullName;

        public void Load()
        {
            ThreadHelper.JoinableTaskFactory.Run(LoadAsync);
        }

        public async Task LoadAsync()
        {
            var manager = await settingsManager.GetValueAsync();
            var settingsStore = manager.GetReadOnlySettingsStore(SettingsScope.UserSettings);

            if (!settingsStore.CollectionExists(CollectionName))
            {
                return;
            }

            foreach (var property in GetOptionProperties())
            {
                try
                {
                    var serializedProp = settingsStore.GetString(CollectionName, property.Name);
                    var value = DeserializeValue(serializedProp, property.PropertyType);
                    property.SetValue(this, value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                }
            }
        }

        public void Save()
        {
            ThreadHelper.JoinableTaskFactory.Run(SaveAsync);
        }

        public async Task SaveAsync()
        {
            var manager = await settingsManager.GetValueAsync();
            var settingsStore = manager.GetWritableSettingsStore(SettingsScope.UserSettings);

            if (!settingsStore.CollectionExists(CollectionName))
            {
                settingsStore.CreateCollection(CollectionName);
            }

            foreach (var property in GetOptionProperties())
            {
                var output = SerializeValue(property.GetValue(this));
                settingsStore.SetString(CollectionName, property.Name, output);
            }

            var liveModel = await GetLiveInstanceAsync();

            if (this != liveModel)
            {
                await liveModel.LoadAsync();
            }

            Saved?.Invoke(this, EventArgs.Empty);
        }

        protected string SerializeValue(object value)
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, value);
            stream.Flush();
            return Convert.ToBase64String(stream.ToArray());
        }

        protected object DeserializeValue(string value, Type type)
        {
            var b = Convert.FromBase64String(value);

            using var stream = new MemoryStream(b);
            var formatter = new BinaryFormatter { Binder = new OptionSerializationBinder() };
            return formatter.Deserialize(stream);
        }

        private static async Task<ShellSettingsManager> GetSettingsManagerAsync()
        {
            var svc =
                await AsyncServiceProvider.GlobalProvider.GetServiceAsync(
                    typeof(SVsSettingsManager)
                ) as IVsSettingsManager;

            Assumes.Present(svc);

            return new ShellSettingsManager(svc);
        }

        private IEnumerable<PropertyInfo> GetOptionProperties()
        {
            return GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsSerializable && p.PropertyType.IsPublic);
        }

        private class OptionSerializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                var property = typeof(T)
                    .GetProperties()
                    .FirstOrDefault(p => p.PropertyType.FullName == typeName);
                return property.PropertyType;
            }
        }
    }
}

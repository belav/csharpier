using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace CSharpier.VisualStudio
{
    public class CSharpierOptionsPage : DialogPage
    {
        private CSharpierOptions localOptions = new CSharpierOptions();
        private Func<bool>? persistOptions;

        [Category("CSharpier")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description("Reformat with CSharpier on Save")]
        public bool RunOnSave { get; set; }

        [Category("CSharpier")]
        [DisplayName("Log Debug Messages")]
        [Description("Log Debug Messages")]
        public bool LogDebugMessages { get; set; }

        public enum StorageType
        {
            Global,
            Local
        }

        [Category("CSharpier")]
        [DisplayName("Storage type")]
        [Description(
            "Global options (same for all solutions) or local options (specific for each solution).\nLocal options are stored in a .suo file."
        )]
        public StorageType OptionStorageType
        {
            get => _storageType;
            set
            {
                if (_storageType != value && value == StorageType.Local)
                {
                    this.AssignFromLocal();
                }
                _storageType = value;
            }
        }
        private StorageType _storageType = StorageType.Global;

        public void OnOptionsLoaded(CSharpierOptions options)
        {
            this.localOptions = options;

            if (this.OptionStorageType == StorageType.Local)
            {
                this.AssignFromLocal();
            }
        }

        private void AssignFromLocal()
        {
            this.RunOnSave = this.localOptions.RunOnSave;
            this.LogDebugMessages = this.localOptions.LogDebugMessages;
        }

        private void AssignTo(CSharpierOptions options)
        {
            options.RunOnSave = this.RunOnSave;
            options.LogDebugMessages = this.LogDebugMessages;
        }

        public CSharpierOptions LoadDefaultOptions()
        {
            var options = new CSharpierOptions();
            this.AssignTo(options);
            return options;
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (this.OptionStorageType == StorageType.Local)
            {
                this.AssignTo(this.localOptions);
                this.persistOptions?.Invoke();
            }
            else
            {
                base.OnApply(e);
            }
        }

        internal void SetOnApply(Func<bool> persistOptions)
        {
            this.persistOptions = persistOptions;
        }
    }

    [Serializable]
    public class CSharpierOptions
    {
        public bool RunOnSave { get; set; }
        public bool LogDebugMessages { get; set; }
    }
}

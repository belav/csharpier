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

        public void OnOptionsLoaded(CSharpierOptions options)
        {
            this.localOptions = options;
            this.AssignFrom(this.localOptions);
        }

        private void AssignFrom(CSharpierOptions options)
        {
            this.RunOnSave = options.RunOnSave;
            this.LogDebugMessages = options.LogDebugMessages;
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

        protected override void SaveSetting(PropertyDescriptor property)
        {
            base.SaveSetting(property);
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
            
            this.AssignTo(this.localOptions);
            this.persistOptions?.Invoke();
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

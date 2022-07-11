using System.ComponentModel;

namespace CSharpier.VisualStudio
{
    public class CSharpierOptionsPage : BaseOptionPage<CSharpierOptions> { }

    public class CSharpierOptions : BaseOptionModel<CSharpierOptions>
    {
        [Category("CSharpier")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description(
            "Reformat with CSharpier on Save - this option is saved locally to the solution"
        )]
        public bool RunOnSave { get; set; }

        [Category("CSharpier")]
        [DisplayName("Log Debug Messages")]
        [Description("Log Debug Messages - this option is saved locally to the solution")]
        public bool LogDebugMessages { get; set; }

        protected override void LoadFrom(CSharpierOptions newInstance)
        {
            this.RunOnSave = newInstance.RunOnSave;
            this.LogDebugMessages = newInstance.LogDebugMessages;
        }
    }
}

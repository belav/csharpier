using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace CSharpier.VisualStudio
{
    // TODO see https://github.com/Elders/VSE-FormatDocumentOnSave for how that works, I think Settings can go away
    public class CSharpierOptionsPage : DialogPage
    {
        [Category("CSharpier")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description("Reformat with CSharpier on Save")]
        public bool RunOnSave
        {
            get => Settings.Instance.RunOnSave;
            set => Settings.Instance.RunOnSave = value;
        }
    }
}

using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace CSharpier.VisualStudio
{
    public class CSharpierOptionsPage : DialogPage
    {
        [Category("CSharpier")]
        [DisplayName("Reformat with CSharpier on Save")]
        [Description("Reformat with CSharpier on Save")]
        public bool RunOnSave { get; set; }
    }
}

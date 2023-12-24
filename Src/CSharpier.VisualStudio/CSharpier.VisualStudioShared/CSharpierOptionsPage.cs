namespace CSharpier.VisualStudio
{
    using Microsoft.VisualStudio.Shell;

    public class CSharpierOptionsPage : DialogPage
    {
        private readonly CSharpierOptions model;

        public CSharpierOptionsPage()
        {
            this.model = ThreadHelper
                .JoinableTaskFactory
                .Run(CSharpierOptions.GetLiveInstanceAsync);
        }

        public override object AutomationObject => this.model;

        public override void LoadSettingsFromStorage()
        {
            this.model.Load();
        }

        public override void SaveSettingsToStorage()
        {
            this.model.Save();
        }
    }
}

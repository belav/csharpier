using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CSharpier.VisualStudio
{
    public class ReformatWithCSharpierOnSave : IVsRunningDocTableEvents3
    {
        private readonly DTE dte;
        private readonly RunningDocumentTable runningDocumentTable;
        private readonly FormattingService formattingService;
        private readonly CSharpierOptionsPage options;
        private readonly CSharpierProcessProvider cSharpierProcessProvider;
        private readonly CSharpierPackage package;

        private ReformatWithCSharpierOnSave(
            CSharpierPackage package,
            DTE dte,
            CSharpierOptionsPage options
        )
        {
            this.package = package;
            this.dte = dte;
            this.runningDocumentTable = new RunningDocumentTable(package);
            this.formattingService = FormattingService.GetInstance(package);
            this.cSharpierProcessProvider = CSharpierProcessProvider.GetInstance(package);
            this.options = options;

            this.runningDocumentTable.Advise(this);
        }

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            var options = package.GetDialogPage<CSharpierOptionsPage>();

            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
            new ReformatWithCSharpierOnSave(package, dte, options);
        }

        public int OnBeforeSave(uint docCookie)
        {
            var runOnSave = package.GetDialogPage<CSharpierOptionsPage>().RunOnSave;

            Logger.Instance.Info(runOnSave.ToString());
            Logger.Instance.Info(this.options.RunOnSave.ToString());

            if (!this.options.RunOnSave)
            {
                return VSConstants.S_OK;
            }

            var document = this.FindDocument(docCookie);

            if (document == null)
            {
                return VSConstants.S_OK;
            }

            this.formattingService.Format(document);

            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            var document = this.FindDocument(docCookie);

            if (document != null)
            {
                this.cSharpierProcessProvider.FindAndWarmProcess(document.FullName);
            }

            return VSConstants.S_OK;
        }

        private Document? FindDocument(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentInfo = this.runningDocumentTable.GetDocumentInfo(docCookie);
            var documentPath = documentInfo.Moniker;

            return this.dte.Documents
                .Cast<Document>()
                .FirstOrDefault(o => o.FullName == documentPath);
        }

        public int OnAfterFirstDocumentLock(
            uint docCookie,
            uint dwRdtLockType,
            uint dwReadLocksRemaining,
            uint dwEditLocksRemaining
        )
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(
            uint docCookie,
            uint dwRdtLockType,
            uint dwReadLocksRemaining,
            uint dwEditLocksRemaining
        )
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents3.OnAfterAttributeChangeEx(
            uint docCookie,
            uint grfAttribs,
            IVsHierarchy pHierOld,
            uint itemidOld,
            string pszMkDocumentOld,
            IVsHierarchy pHierNew,
            uint itemidNew,
            string pszMkDocumentNew
        )
        {
            return VSConstants.S_OK;
        }

        int IVsRunningDocTableEvents2.OnAfterAttributeChangeEx(
            uint docCookie,
            uint grfAttribs,
            IVsHierarchy pHierOld,
            uint itemidOld,
            string pszMkDocumentOld,
            IVsHierarchy pHierNew,
            uint itemidNew,
            string pszMkDocumentNew
        )
        {
            return VSConstants.S_OK;
        }
    }
}

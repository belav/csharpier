using System.Linq;
using System.Threading.Tasks;
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
        private readonly CSharpierOptionsPage csharpierOptionsPage;

        private ReformatWithCSharpierOnSave(
            DTE dte,
            RunningDocumentTable runningDocumentTable,
            FormattingService formattingService,
            CSharpierOptionsPage csharpierOptionsPage
        )
        {
            this.dte = dte;
            this.runningDocumentTable = runningDocumentTable;
            this.formattingService = formattingService;
            this.csharpierOptionsPage = csharpierOptionsPage;
        }

        public static async Task InitializeAsync(
            CSharpierPackage csharpierPackage,
            FormattingService formattingService,
            CSharpierOptionsPage cSharpierOptionsPage
        )
        {
            var dte = await csharpierPackage.GetServiceAsync(typeof(DTE)) as DTE;
            var runningDocumentTable = new RunningDocumentTable(csharpierPackage);
            var reformatWithCSharpierOnSave = new ReformatWithCSharpierOnSave(
                dte,
                runningDocumentTable,
                formattingService,
                cSharpierOptionsPage
            );
            runningDocumentTable.Advise(reformatWithCSharpierOnSave);
        }

        public int OnBeforeSave(uint docCookie)
        {
            if (!this.csharpierOptionsPage.RunOnSave)
            {
                return VSConstants.S_OK;
            }

            var document = FindDocument(docCookie);

            if (document == null)
            {
                return VSConstants.S_OK;
            }

            this.formattingService.Format(document);

            return VSConstants.S_OK;
        }

        private Document FindDocument(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentInfo = runningDocumentTable.GetDocumentInfo(docCookie);
            var documentPath = documentInfo.Moniker;

            return dte.Documents.Cast<Document>().FirstOrDefault(o => o.FullName == documentPath);
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

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
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

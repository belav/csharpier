using System;
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
        private readonly CSharpierProcessProvider cSharpierProcessProvider;

        private ReformatWithCSharpierOnSave(CSharpierPackage package, DTE dte)
        {
            this.dte = dte;
            this.runningDocumentTable = new RunningDocumentTable(package);
            this.formattingService = FormattingService.GetInstance(package);
            this.cSharpierProcessProvider = CSharpierProcessProvider.GetInstance(package);

            this.runningDocumentTable.Advise(this);
        }

        public static async Task InitializeAsync(CSharpierPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
            new ReformatWithCSharpierOnSave(package, dte!);
        }

        public int OnBeforeSave(uint docCookie)
        {
            Logger.Instance.Debug("OnBeforeSave");
            var runOnSave =
                CSharpierOptions.Instance.SolutionRunOnSave is true
                || (
                    CSharpierOptions.Instance.SolutionRunOnSave is null
                    && CSharpierOptions.Instance.GlobalRunOnSave is true
                );

            if (!runOnSave)
            {
                Logger.Instance.Debug("No RunOnSave");
                return VSConstants.S_OK;
            }

            var document = this.FindDocument(docCookie);

            if (document == null)
            {
                Logger.Instance.Debug("No Document");
                return VSConstants.S_OK;
            }

            Logger.Instance.Debug("Before format");
            this.formattingService.Format(document);
            Logger.Instance.Debug("Done Format");
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

        private Document FindDocument(uint docCookie)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var documentInfo = this.runningDocumentTable.GetDocumentInfo(docCookie);
                var documentPath = documentInfo.Moniker;

                if (this.dte.ActiveDocument.FullName == documentPath)
                {
                    return this.dte.ActiveDocument;
                }

                return this.dte.Documents.Item(documentPath);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
                return null;
            }
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

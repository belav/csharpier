package com.intellij.csharpierrider;

import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.CommonDataKeys;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.editor.Editor;
import com.intellij.openapi.editor.ex.util.EditorScrollingPositionKeeper;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.util.text.StringUtil;
import com.intellij.openapi.vfs.ReadonlyStatusHandler;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.psi.PsiDocumentManager;
import com.intellij.psi.PsiFile;
import com.intellij.util.ArrayUtil;
import com.intellij.util.LineSeparator;
import org.jetbrains.annotations.NotNull;

import java.util.Collections;

import static com.intellij.openapi.command.WriteCommandAction.runWriteCommandAction;

public class ReformatWithCSharpierAction extends AnAction {
    Logger LOG = Logger.getInstance(ReformatWithCSharpierAction.class);

    public ReformatWithCSharpierAction() {
        LOG.info("creating instance");
    }

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        LOG.info("Formatting!");
        Project project = e.getProject();
        if (project == null) {
            return;
        }
        Editor editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            processFileInEditor(project, editor);
        }
        // TODO what is this?
        else {
            VirtualFile[] virtualFiles = e.getData(CommonDataKeys.VIRTUAL_FILE_ARRAY);
            if (!ArrayUtil.isEmpty(virtualFiles)) {
                // TODO processVirtualFiles(project, Arrays.asList(virtualFiles), myErrorHandler);
            }
        }
    }

    private static void processFileInEditor(@NotNull Project project, @NotNull Editor editor) {
        PsiFile file = PsiDocumentManager.getInstance(project).getPsiFile(editor.getDocument());
        if (file == null) {
            return;
        }
        VirtualFile vFile = file.getVirtualFile();
        if (ReadonlyStatusHandler.getInstance(project).ensureFilesWritable(Collections.singletonList(vFile))
                .hasReadonlyFiles()) {
            return;
        }

        CSharpierService cSharpierService = CSharpierService.getInstance(project);
        String result = cSharpierService.format(file.getText(), file.getName());
        // TODO check for actual formatting result
        if (result == "") {
            return;
        }

        Document document = editor.getDocument();
        CharSequence textBefore = document.getImmutableCharSequence();
        String newContent = result;
        // TODO do we need this?
        LineSeparator newLineSeparator = StringUtil.detectSeparators(newContent);
        String newDocumentContent = StringUtil.convertLineSeparators(newContent);

        //Ref<Boolean> lineSeparatorUpdated = new Ref<>(Boolean.FALSE);
        EditorScrollingPositionKeeper.perform(editor, true, () -> {
            runWriteCommandAction(project, () -> {
                if (!StringUtil.equals(textBefore, newContent)) {
                    document.setText(newDocumentContent);
                }
                //lineSeparatorUpdated.set(setDetectedLineSeparator(project, vFile, newLineSeparator));
            });
        });

        // showHintLater(editor, buildNotificationMessage(document, textBefore, lineSeparatorUpdated.get()), false, null);
        //}
    }
}
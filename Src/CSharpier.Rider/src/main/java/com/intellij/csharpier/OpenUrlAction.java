package com.intellij.csharpier;

import com.intellij.ide.BrowserUtil;
import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import org.jetbrains.annotations.NotNull;

public class OpenUrlAction extends AnAction {

  private String url;

  public OpenUrlAction(String title, String url) {
    super(title);
    this.url = url;
  }

  @Override
  public void actionPerformed(@NotNull AnActionEvent e) {
    BrowserUtil.browse(this.url);
  }
}

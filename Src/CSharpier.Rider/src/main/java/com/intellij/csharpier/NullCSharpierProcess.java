package com.intellij.csharpier;

public class NullCSharpierProcess implements ICSharpierProcess {

    public static ICSharpierProcess Instance = new NullCSharpierProcess();

    private NullCSharpierProcess() {}

    @Override
    public String getVersion() {
        return "NULL";
    }

    @Override
    public boolean getProcessFailedToStart() {
        return false;
    }

    @Override
    public String formatFile(String content, String fileName) {
        return "";
    }

    @Override
    public void dispose() {}
}

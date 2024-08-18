package com.intellij.csharpier;

interface ICSharpierProcess {
  String getVersion();
  boolean getProcessFailedToStart();
  String formatFile(String content, String fileName);
  void dispose();
}

interface ICSharpierProcess2 extends ICSharpierProcess {
  FormatFileResult formatFile(FormatFileParameter parameter);
  void dispose();
}

class FormatFileParameter {

  public String fileContents;
  public String fileName;
}

class FormatFileResult {

  public String formattedFile;
  public Status status;
  public String errorMessage;
}

enum Status {
  Formatted,
  Ignored,
  Failed,
}

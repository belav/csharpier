package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;

public class CSharpierLogger {

  private static final Logger logger = Logger.getInstance(
    CSharpierLogger.class
  );

  public static Logger getInstance() {
    return logger;
  }
}

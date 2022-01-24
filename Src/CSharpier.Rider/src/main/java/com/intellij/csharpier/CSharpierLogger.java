package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import org.apache.log4j.Level;

public class CSharpierLogger {
    private final static Logger logger = createLogger();

    private static Logger createLogger() {
        var logger = Logger.getInstance(CSharpierLogger.class);
        if ("1".equals(System.getenv("DEBUG"))) {
            logger.setLevel(Level.DEBUG);
        }

        return logger;
    }

    public static Logger getInstance() {
        return logger;
    }
}


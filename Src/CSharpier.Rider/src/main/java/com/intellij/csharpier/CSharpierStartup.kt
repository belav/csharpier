package com.intellij.csharpier

import com.intellij.openapi.application.ApplicationManager
import com.intellij.openapi.diagnostic.Logger
import com.intellij.openapi.project.Project
import com.intellij.openapi.startup.StartupActivity
import com.intellij.openapi.startup.StartupActivity.DumbAware
import com.jetbrains.rd.platform.util.lifetime
import com.jetbrains.rd.util.reactive.adviseUntil
import com.jetbrains.rider.model.riderSolutionLifecycle
import com.jetbrains.rider.projectView.solution
import com.jetbrains.rider.runtime.RiderDotNetActiveRuntimeHost

// kotlin because I have no idea how to get project.solution.riderSolutionLifecycle.isProjectModelReady.adviseUntil happy in java
class CSharpierStartup : StartupActivity, DumbAware {
    var logger: Logger = CSharpierLogger.getInstance()

    override fun runActivity(project: Project) {
        project.solution.riderSolutionLifecycle.isProjectModelReady.adviseUntil(project.lifetime) { isReady ->

            val dotNetCoreRuntime =
                RiderDotNetActiveRuntimeHost.Companion.getInstance(project).dotNetCoreRuntime.value

            if (!isReady || dotNetCoreRuntime == null) {
                if (isReady) {
                    logger.warn("isProjectModelReady is true, but dotNetCoreRuntime is still null");
                }

                return@adviseUntil false
            }

            CSharpierProcessProvider.getInstance(project)
            ApplicationManager.getApplication().getService(ReformatWithCSharpierOnSave::class.java)
            DotNetProvider.getInstance(project).initialize();

            return@adviseUntil true
        }
    }

}
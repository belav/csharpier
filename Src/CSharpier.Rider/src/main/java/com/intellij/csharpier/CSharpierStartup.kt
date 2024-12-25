package com.intellij.csharpier

import com.intellij.openapi.diagnostic.Logger
import com.intellij.openapi.project.Project
import com.intellij.openapi.startup.StartupActivity
import com.intellij.openapi.startup.StartupActivity.DumbAware
import com.intellij.util.application
import com.jetbrains.rd.platform.util.lifetime
import com.jetbrains.rd.util.lifetime.Lifetime
import com.jetbrains.rd.util.lifetime.LifetimeDefinition
import com.jetbrains.rd.util.reactive.adviseUntil
import com.jetbrains.rider.model.riderSolutionLifecycle
import com.jetbrains.rider.projectView.solution
import com.jetbrains.rider.runtime.RiderDotNetActiveRuntimeHost

// kotlin because I have no idea how to get project.solution.riderSolutionLifecycle.isProjectModelReady.adviseUntil happy in java
class CSharpierStartup : StartupActivity, DumbAware {
    val logger: Logger = CSharpierLogger.getInstance()

    private val lifetimeDefinition = LifetimeDefinition()
    private val lifetime: Lifetime = lifetimeDefinition


    override fun runActivity(project: Project) {
        // this ensures we meet the conditions of isProjectModelReady "Must be executed on UI thread or background threads with special permissions"
        application.invokeLater {
            project.solution.riderSolutionLifecycle.isProjectModelReady.adviseUntil(lifetime) { isReady ->

                val dotNetCoreRuntime =
                    RiderDotNetActiveRuntimeHost.Companion.getInstance(project).dotNetCoreRuntime.value

                if (!isReady || dotNetCoreRuntime == null) {
                    if (isReady) {
                        logger.warn("isProjectModelReady is true, but dotNetCoreRuntime is still null");
                    }

                    return@adviseUntil false
                }

                CSharpierProcessProvider.getInstance(project)
                DotNetProvider.getInstance(project).initialize();

                return@adviseUntil true
            }
        }
    }
}

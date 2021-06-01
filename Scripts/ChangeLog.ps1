﻿# this uses https://github.com/microsoft/PowerShellForGitHub
# you'll need to Set-GitHubAuthentication first

$versionNumber = "0.9.5"
$previousVersionNumber = "0.9.4"

$repository = "https://github.com/belav/csharpier"

$milestones = Get-GitHubMilestone -Uri $repository -State "Open"
$milestoneNumber = -1
foreach($milestone in $milestones) {
    if ($milestone.title -eq $versionNumber) {
        $milestoneNumber = $milestone.number
    }
}

$issues = Get-GitHubIssue -Uri https://github.com/belav/csharpier -MilestoneNumber $milestoneNumber -State All

$content = [System.Text.StringBuilder]::new()
$content.AppendLine("# " + $versionNumber)
$content.AppendLine()
$content.AppendLine("[diff](https://github.com/belav/csharpier/compare/" + $previousVersionNumber + "..." + $versionNumber + ")")
$content.AppendLine()
foreach ($issue in $issues) {
    if ($issue.title.ToLower().Contains("checklist")) {
        continue
    }
    
    $content.AppendLine("- " + $issue.title + " [#" + $issue.number +"](" + $issue.html_url + ")")
}

Write-Output $content.ToString()

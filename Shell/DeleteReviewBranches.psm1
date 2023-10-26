function CSH-DeleteReviewBranches {

    # TODO probably make this configurable
    $pathToTestingRepo = "C:/Projects/csharpier-repos"
    Set-Location $pathToTestingRepo
    
    git checkout main
    git branch | Where-Object { $_ -notmatch "main" } | ForEach-Object { git branch -D $_ }
    git branch -r | Where-Object { $_ -notmatch "origin/main" } | ForEach-Object { git push origin --delete $_.Replace("origin/", "").Trim() }
}

Export-ModuleMember -Function CSH-*

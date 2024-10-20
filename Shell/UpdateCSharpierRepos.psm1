function CSH-UpdateCSharpierRepos()
{

    $repositories = @()
    $repositories += "https://github.com/dotnet/aspnetcore.git"
    $repositories += "https://github.com/aspnet/AspNetWebStack.git"
    $repositories += "https://github.com/AutoMapper/AutoMapper.git"
    $repositories += "https://github.com/castleproject/Core.git"
    $repositories += "https://github.com/dotnet/command-line-api.git"
    $repositories += "https://github.com/dotnet/format.git"
    $repositories += "https://github.com/dotnet/efcore.git"
    $repositories += "https://github.com/moq/moq4.git"
    $repositories += "https://github.com/JamesNK/Newtonsoft.Json.git"
    $repositories += "https://github.com/dotnet/roslyn.git"
    $repositories += "https://github.com/dotnet/runtime.git"
    $repositories += "https://github.com/mono/mono.git"
    $repositories += "https://github.com/increase-POS/Res-Server.git"

    $tempLocation = "c:\temp\UpdateRepos"

    if (-not(Test-Path $tempLocation))
    {
        New-Item $tempLocation -Force -ItemType Directory
    }

    Set-Location $tempLocation

    $ErrorActionPreference = "Continue"

    foreach ($repository in $repositories)
    {
        $repoFolder = $tempLocation + "/" + (Split-Path $repositories -Leaf).Replace(".git", "")
        if (Test-Path $repoFolder)
        {
            Set-Location $repoFolder
            & git pull origin
            Set-Location $tempLocation
        }
        else
        {
            & git clone $repository
        }
    }

    $destination = "C:\projects\csharpier-repos\"
    Set-Location $destination
    & git checkout main

    $extensions = (".csproj", ".props", ".targets", ".xml", ".config", ".cs")
    
    foreach ($extension in $extensions)
    {
        Get-ChildItem $tempLocation | Copy-Item -Destination $destination -Filter "*$extension" -Recurse -Force    
    }

    return
    
    $items = Get-ChildItem -Recurse C:\projects\csharpier-repos -File
    $count = 0
    foreach ($item in $items)
    {
        if ($item.Extension -eq ".cs")
        {
            # we don't really need all of these files, let's just cut out every other one
            if ($count % 2 -eq 0)
            {
                Remove-Item $item.FullName
            }
            $count++
        }
    }

    Set-Location $destination

    & git add .
    & git commit -m "Updating repos"
    & git push origin
}

Export-ModuleMember -Function CSH-*
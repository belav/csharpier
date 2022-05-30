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

$tempLocation = "c:\temp\UpdateRepos"

#TODO just pull it they exist already
if (Test-Path $tempLocation) {
    Remove-Item -Recurse -Force $tempLocation
}

New-Item $tempLocation -Force -ItemType Directory
Set-Location $tempLocation

$ErrorActionPreference = "Continue"

foreach ($repository in $repositories)
{
    & git clone $repository
}

#TODO make sure to switch to main/master
$destination = "C:\projects\csharpier-repos\"

Get-ChildItem $tempLocation | Copy-Item -Destination $destination -Filter *.cs -Recurse -Force

$items = Get-ChildItem -Recurse C:\projects\csharpier-repos -File
foreach ($item in $items) {
    if ($item.Name -eq ".git") {
        Remove-Item -Force -Recurse $item.FullName
    }
    if ($item.Extension -ne ".cs") {
        Remove-Item $item.FullName
    }
}

$items = Get-ChildItem C:\projects\csharpier-repos -Directory
foreach ($item in $items) {
    if ($item.Name -eq ".git") {
        Remove-Item -Force -Recurse $item.FullName
    }
}

# TODO commit and push
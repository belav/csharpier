Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

foreach ($file in Get-ChildItem $PSScriptRoot -Filter "*.psm1") {
    Import-Module $file.FullName -DisableNameChecking -Force
}

Write-Host -ForegroundColor DarkMagenta "Welcome to CSharpier shell"
Write-Host -ForegroundColor DarkMagenta "Get-Command -Verb CSH will list commands"

<# 
this will get the CSH shell to run within powershell.
open a command prompt
notepad $PROFILE
modify the file to include the path to this file
Import-Module c:\projects\csharpier\Shell\Init.ps1
save

repeat for windows terminal

WSL
sudo apt-get update
apt-get install powershell
mkdir ~/.config/powershell
pwsh
nano $PROFILE
Import-Module /mnt/c/projects/csharpier/Shell/Init.ps1
cd /mnt/c/projects/csharpier

#>
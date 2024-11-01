function CSH-PublishVS {
    param (
        [Parameter(Mandatory=$true)]
        [string]$accessToken
    )
    
    $repositoryRoot = Join-Path $PSScriptRoot ".."
    $vsRoot = Join-Path $repositoryRoot "/Src/CSharpier.VisualStudio"

    $vsixPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe"

    & $vsixPath publish `
        -payload $vsRoot/CSharpier.VisualStudio/bin/Release/CSharpier.VisualStudio.vsix `
        -publishManifest $vsRoot/manifest.json `
        -personalAccessToken $accessToken

    & $vsixPath publish `
        -payload $vsRoot/CSharpier.VisualStudio2019/bin/Release/CSharpier.VisualStudio2019.vsix `
        -publishManifest $vsRoot/manifest2019.json `
        -personalAccessToken $accessToken

}

Export-ModuleMember -Function CSH-*
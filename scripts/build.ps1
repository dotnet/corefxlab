Param(
    [string]$Configuration="Debug",
    [string]$Restore="true",
    [string]$Channel="preview",
    [string]$Version="2.0.0-preview1-005675",
    [string]$BuildVersion=[System.DateTime]::Now.ToString('eyyMMdd-1')
)

$file = "corefxlab.sln"

Write-Host "Commencing full build for Configuration=$Configuration."

if (!(Test-Path "dotnet\dotnet.exe")) {
    Write-Host "dotnet.exe not installed, downloading and installing."
    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Channel $Channel -Version $Version -InstallDir $PSScriptRoot\..\dotnet"
    if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
        Write-Error "Failed to install dotnet.exe, exit code [$lastexitcode], aborting build."
        exit -1
    }
}

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1

$dotnetExePath="$PSScriptRoot\..\dotnet\dotnet.exe"

if ($Restore -eq "true") {
    Write-Host "Restoring all packages"
    Invoke-Expression "$dotnetExePath restore $file /p:VersionSuffix=$BuildVersion"
    if ($lastexitcode -ne 0) {
        Write-Error "Failed to restore packages."
        exit -1
    }
}

$errorsEncountered = 0

Write-Host "Building solution $file..."
Invoke-Expression "$dotnetExePath build $file -c $Configuration /p:VersionSuffix=$BuildVersion"
if ($lastexitcode -ne 0) {
    Write-Error "Failed to build solution $file"
    $errorsEncountered++
}

Write-Host "Building and running tests for solution $file..."
Invoke-Expression "$dotnetExePath test $file -c $Configuration -- -notrait category=performance -notrait category=outerloop"

if ($lastexitcode -ne 0) {
    Write-Error "Some tests failed in solution $file"
    $errorsEncountered++
}

if ($errorsEncountered -eq 0) {
    Write-Host "** Build succeeded. **" -foreground "green"
}
else {
    Write-Host "** Build failed. **" -foreground "red"
}

exit $errorsEncountered

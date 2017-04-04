#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

param(
    [switch]$Help,
    [switch]$Update)

if($Help)
{
    Write-Host "Usage: .\update-dependencies.ps1"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  -Help                 Display this help message"
    Write-Host "  -Update               Update dependencies (but don't open a PR)"
    exit 0
}

$ProjectPath = "$PSScriptRoot\update-dependencies\update-dependencies.csproj"
$ProjectArgs = ""
$DotNetExePath = "$PSScriptRoot\..\dotnet\dotnet.exe"

if ($Update)
{
    $ProjectArgs = "--update"
}

$Version = (Get-Content "$PSScriptRoot\..\DotnetCLIVersion.txt" -Raw).Trim()

# Ensure dotnet is installed
if (!(Test-Path "dotnet\dotnet.exe")) {
    Write-Host "dotnet.exe not installed, downloading and installing."
    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Version $Version -InstallDir $PSScriptRoot\..\dotnet"
    if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
        Write-Error "Failed to install dotnet.exe, exit code [$lastexitcode], aborting build."
        exit -1
    }
}

# Restore the app
Write-Host "Restoring App $ProjectPath..."
Invoke-Expression "$DotNetExePath restore `"$ProjectPath`""
if($LASTEXITCODE -ne 0) { throw "Build failed" }

# Run the app
Write-Host "Invoking App $ProjectPath..."
Invoke-Expression "$DotNetExePath run -p `"$ProjectPath`" `"$ProjectArgs`""
if($LASTEXITCODE -ne 0) { throw "Build failed" }

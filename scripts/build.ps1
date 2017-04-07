Param(
    [string]$Configuration="Debug",
    [string]$Restore="true",
    [string]$Version="<default>",
    [string]$BuildVersion=[System.DateTime]::Now.ToString('eyyMMdd-1')
)

$file = "corefxlab.sln"

if ($Version -eq "<default>") {
    $Version = (Get-Content "$PSScriptRoot\..\DotnetCLIVersion.txt" -Raw).Trim()
}

Write-Host "Commencing full build for Configuration=$Configuration."

if (!(Test-Path "dotnet\dotnet.exe")) {
    Write-Host "dotnet.exe not installed, downloading and installing."
    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Version $Version -InstallDir $PSScriptRoot\..\dotnet"
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
Invoke-Expression "$dotnetExePath msbuild $file /p:cofiguration=$Configuration /p:VersionSuffix=$BuildVersion"
if ($lastexitcode -ne 0) {
    Write-Error "Failed to build solution $file"
    $errorsEncountered++
}

$projectsFailed = New-Object System.Collections.Generic.List[String]

foreach ($testFile in [System.IO.Directory]::EnumerateFiles("$PSScriptRoot\..\tests", "*.csproj", "AllDirectories")) {
    Write-Host "Building and running tests for project $testFile..."
    Invoke-Expression "$dotnetExePath test $testFile -c $Configuration --no-build -- -notrait category=performance -notrait category=outerloop"

    if ($lastexitcode -ne 0) {
        Write-Error "Some tests failed in project $testFile"
        $projectsFailed.Add($testFile)
        $errorsEncountered++
    }
}

if ($errorsEncountered -eq 0) {
    Write-Host "** Build succeeded. **" -foreground "green"
}
else {
    Write-Host "** Build failed. $errorsEncountered projects failed to build or test. **" -foreground "red"
    foreach ($projectFile in $projectsFailed) {
        Write-Host "    $projectFile" -foreground "red"
    }
}

exit $errorsEncountered

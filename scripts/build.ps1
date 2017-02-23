Param(
    [string]$Configuration="Debug",
    [string]$Restore="true",
    [string]$Channel="preview",
    [string]$Version="1.0.0-rc4-004771"
)

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
    Invoke-Expression "$dotnetExePath restore"
    if ($lastexitcode -ne 0) {
        Write-Error "Failed to restore packages."
        exit -1
    }
}

$errorsEncountered = 0
$projectsFailed = New-Object System.Collections.Generic.List[String]

foreach ($file in [System.IO.Directory]::EnumerateFiles("$PSScriptRoot\..\src", "System*.csproj", "AllDirectories")) {
    Write-Host "Building $file..."
    Invoke-Expression "$dotnetExePath build $file -c $Configuration"

    if ($lastexitcode -ne 0) {
        Write-Error "Failed to build project $file"
        $projectsFailed.Add($file)
        $errorsEncountered++
    }
}

foreach ($file in [System.IO.Directory]::EnumerateFiles("$PSScriptRoot\..\tests", "*.csproj", "AllDirectories")) {
    if ($file -match "Benchmarks" -or
			$file -match "System.Binary.Base64.Tests" -or
			$file -match "System.Slices.Tests") {
        Write-Warning "Skipping tests in $file. These tests use the xunit.performance package which needs to be updated for dotnet SDK rc4."
        continue;
    }
    if ($file -match "System.IO.Pipelines.Performance.Tests")
    {
        Write-Warning "Building Benchmark.NET project $file. Benchmark would not be ran as part of the build."
        Invoke-Expression "$dotnetExePath build $file -c $Configuration"
        continue;
    }
    Write-Host "Building and running tests for project $file..."
    Invoke-Expression "$dotnetExePath test $file -c $Configuration -- -notrait category=performance -notrait category=outerloop"

    if ($lastexitcode -ne 0) {
        Write-Error "Some tests failed in project $file"
        $projectsFailed.Add($file)
        $errorsEncountered++
    }
}

if ($errorsEncountered -eq 0) {
    Write-Host "** Build succeeded. **" -foreground "green"
}
else {
    Write-Host "** Build failed. $errorsEncountered projects failed to build or test. **" -foreground "red"
    foreach ($file in $projectsFailed) {
        Write-Host "    $file" -foreground "red"
    }
}

exit $errorsEncountered
Param(
    [string]$Configuration="Debug",
    [string]$Restore="true",
    [string]$Version="<default>",
    [string]$BuildVersion=[System.DateTime]::Now.ToString('preview2-yyMMdd-1'),
    [string]$SkipTests="false"
)

Write-Host "Configuration=$Configuration."
Write-Host "Restore=$Restore."
Write-Host "Version=$Version."
Write-Host "BuildVersion=$BuildVersion."
Write-Host "SkipTests=$SkipTests."

$dotnetExePath="$PSScriptRoot\..\dotnetcli\dotnet.exe"
if ($Version -eq "<default>") {
    $Version = (Get-Content "$PSScriptRoot\..\DotnetCLIVersion.txt" -Raw).Trim()
}

if (!(Test-Path "$PSScriptRoot\..\dotnetcli")) {
    Write-Host "dotnet.exe not installed, downloading and installing."
    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Channel master -Version $Version -InstallDir $PSScriptRoot\..\dotnetcli"
    if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
        Write-Error "Failed to install latest dotnet.exe, exit code [$lastexitcode], aborting build."
        exit -1
    }

    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Version 2.0.0 -InstallDir $PSScriptRoot\..\dotnetcli"
    if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
        Write-Error "Failed to install framework version 2.0.0, exit code [$lastexitcode], aborting build."
        exit -1
    }

    Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Version 2.1.401 -InstallDir $PSScriptRoot\..\dotnetcli"
    if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
        Write-Error "Failed to install framework version 2.1.401, exit code [$lastexitcode], aborting build."
        exit -1
    }
} else {
    Write-Host "dotnet.exe is installed, checking for latest."

    $cliVersion = Invoke-Expression "$dotnetExePath --version"
    if ($cliVersion -ne $Version) {
        Write-Host "Newest version of dotnet cli not installed, downloading and installing."
        Invoke-Expression -Command "$PSScriptRoot\install-dotnet.ps1 -Channel master -Version $Version -InstallDir $PSScriptRoot\..\dotnetcli"
        if ($lastexitcode -ne $null -and $lastexitcode -ne 0) {
            Write-Error "Failed to install latest dotnet.exe, exit code [$lastexitcode], aborting build."
            exit -1
        }
    }
    else
    {
        Write-Host "Newest version of dotnet cli is already installed."
    }
}

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1
$env:DOTNET_MULTILEVEL_LOOKUP = 0


$file = "corefxlab.sln"

if ($Restore -eq "true") {
    Write-Host "Restoring all packages"
    Invoke-Expression "$dotnetExePath restore $file /p:VersionSuffix=$BuildVersion"
    if ($lastexitcode -ne 0) {
        Write-Error "Failed to restore packages."
        exit -1
    }
}

Write-Host "Building solution $file..."
Invoke-Expression "$dotnetExePath build $file -c $Configuration /p:VersionSuffix=$BuildVersion --no-restore /nologo"

if ($lastexitcode -ne 0) {
    Write-Error "Failed to build solution $file"
    exit -1
}

$errorsEncountered = 0
$projectsFailed = New-Object System.Collections.Generic.List[String]

if ($SkipTests -ne "true") {
    foreach ($testFile in [System.IO.Directory]::EnumerateFiles("$PSScriptRoot\..\tests", "*Tests*.csproj", "AllDirectories")) {
        Write-Host "Building and running tests for project $testFile..."
        Invoke-Expression "$dotnetExePath test $testFile -c $Configuration --no-build -- -notrait category=performance -notrait category=outerloop"
    
        if ($lastexitcode -ne 0) {
            Write-Error "Some tests failed in project $testFile"
            $projectsFailed.Add($testFile)
            $errorsEncountered++
        }
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

Param(
    [string]$Configuration="Debug"
)

Write-Host "Commencing full build for Configuration=$Configuration."

if (!(Test-Path "dotnet\dotnet.exe")) {
    Write-Host "dotnet.exe not installed, downloading and installing."
    .\install-dotnet.ps1
    if (!$?) {
        Write-Error "Failed to install dotnet.exe, aborting build."
        exit -1
    }
}

Write-Host "Restoring all packages"
.\dotnet\dotnet.exe restore src tests
    if (!$?) {
      Write-Error "Failed to restore packages."
      exit -1
    }

$errorsEncountered = 0
$projectsFailed = New-Object System.Collections.Generic.List[String]

foreach ($file in [System.IO.Directory]::EnumerateFiles(".\src", "project.json", "AllDirectories")) {
    Write-Host "Building $file..."
    .\dotnet\dotnet.exe build $file -c $Configuration

    if (!$?) {
        Write-Error "Failed to build project $file"
        $projectsFailed.Add($file)
        $errorsEncountered++
    }
}

foreach ($file in [System.IO.Directory]::EnumerateFiles(".\tests", "project.json", "AllDirectories")) {
    Write-Host "Building and running tests for project $file..."
    .\dotnet\dotnet.exe test $file -c $Configuration -notrait category=performance -notrait category=outerloop

    if (!$?) {
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
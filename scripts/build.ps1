Param(
    [string]$Configuration="Debug",
    [string]$Restore="true",
    [string]$Channel="preview",
    [string]$Version="2.0.0-preview1-005675",
    [string]$BuildVersion=[System.DateTime]::Now.ToString('eyyMMdd-1')
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
    Invoke-Expression "$dotnetExePath restore corefxlab.sln /p:VersionSuffix=$BuildVersion"
    if ($lastexitcode -ne 0) {
        Write-Error "Failed to restore packages."
        exit -1
    }
}

$errorsEncountered = 0
$projectsFailed = New-Object System.Collections.Generic.List[String]

$source = @("src\", "samples\")
$test = @("tests\")
$root = "$PSScriptRoot\..\"

$reader = [System.IO.File]::OpenText("corefxlab.sln")
while($null -ne ($line = $reader.ReadLine())) {
    $pos = $line.IndexOf('.csproj')
    if ($pos -ne -1)
    {
        Write-Host $line
        Write-Host $pos
        $projectLinesSplitUp = $line.Split(',',[System.StringSplitOptions]::RemoveEmptyEntries)
        $projectLinesSplitUp | Select-String -Pattern $source -SimpleMatch | ForEach {
                Write-Host "FOR AHSON 2: $_..."
                $fileName = $_ -replace ' "', "" -replace '"', ""
                Write-Host "FOR AHSON 3: $fileName..."
                $file = -join ($root, $fileName)

                Write-Host "Building $file..."
                Invoke-Expression "$dotnetExePath build $file -c $Configuration /p:VersionSuffix=$BuildVersion"

                if ($lastexitcode -ne 0) {
                    Write-Error "Failed to build project $file"
                    $projectsFailed.Add($file)
                    $errorsEncountered++
                }
        }

        $projectLinesSplitUp | Select-String -Pattern $test -SimpleMatch | ForEach {
                Write-Host "FOR AHSON 4: $_..."
                $fileName = $_ -replace ' "', "" -replace '"', ""
                Write-Host "FOR AHSON 5: $fileName..."
                $file = -join ($root, $fileName)

                Write-Host "Building and running tests for project $file..."
                Invoke-Expression "$dotnetExePath test $file -c $Configuration -- -notrait category=performance -notrait category=outerloop"

                if ($lastexitcode -ne 0) {
                    Write-Error "Some tests failed in project $file"
                    $projectsFailed.Add($file)
                    $errorsEncountered++
                }
        }
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

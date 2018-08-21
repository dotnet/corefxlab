Param(
    [string]$Configuration="Debug",
    [string]$ApiKey,
    [string]$BuildVersion=[System.DateTime]::Now.ToString('preview2-yyMMdd-1')
)

$repoRoot = "$PSScriptRoot\.."
$dotnetExePath="$repoRoot\dotnetcli\dotnet.exe"
$nugetPath = "$repoRoot\nuget\nuget.exe"
$packagesPath = "$repoRoot\packages"

$env:DOTNET_MULTILEVEL_LOOKUP = 0
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1

Function Ensure-Nuget-Exists {
    if (!(Test-Path "$nugetPath")) {
        if (!(Test-Path "$repoRoot\nuget")) {
            New-Item -ItemType directory -Path "$repoRoot\nuget"
        }
        Write-Host "nuget.exe not found. Downloading to $nugetPath"
        Invoke-WebRequest "https://dist.nuget.org/win-x86-commandline/v4.3.0/nuget.exe" -OutFile $nugetPath
    }
}

Write-Host "** Building all NuGet packages. **"
foreach ($file in [System.IO.Directory]::EnumerateFiles("$repoRoot\src", "*.csproj", "AllDirectories")) {

    $projectName = [System.IO.Path]::GetFileName($file);
    if (!($projectName.startswith("System.") -or $projectName.startswith("Microsoft."))) {
        continue;
    }

    Write-Host "Creating NuGet package for $file..."
    Invoke-Expression "$dotnetExePath pack $file -c $Configuration -o $packagesPath --include-symbols --version-suffix $BuildVersion"

    if (!$?) {
        Write-Error "Failed to create NuGet package for project $file"
    }
}

if ($ApiKey)
{
    Ensure-Nuget-Exists

    foreach ($file in [System.IO.Directory]::EnumerateFiles("$packagesPath", "*.nupkg")) {
        try {
            Write-Host "Pushing package $file to MyGet..."
            if($file.EndsWith("symbols.nupkg")) {
                $arguments = "push $file $apiKey -Source https://dotnet.myget.org/F/dotnet-corefxlab/symbols/api/v2/package"
            }
            else { 
                $arguments = "push $file $apiKey -Source https://dotnet.myget.org/F/dotnet-corefxlab/api/v2/package"
            }
            $process = Start-Process -FilePath $nugetPath -ArgumentList $arguments -Wait -PassThru -NoNewWindow
            $RetVal = $process.ExitCode
            if($RetVal -eq 0) {
                Write-Host "done"
            }
            else {
                Write-Error "Failed to push nuget package $file with error code $RetVal"
            }
        } catch [System.Exception] {
            Write-Host "Failed to push nuget package $file with error $_.Exception.Message"
        }
    }
}

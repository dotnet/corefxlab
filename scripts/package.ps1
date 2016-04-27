Param(
    [string]$Configuration="Debug",
    [string]$ApiKey
)

$repoRoot = "$PSScriptRoot\.."
$dotnetExePath="$repoRoot\dotnet\dotnet.exe"
$nugetPath = "$repoRoot\nuget\nuget.exe"
$packagesPath = "$repoRoot\packages"

Function Ensure-Nuget-Exists {
    if (!(Test-Path "$nugetPath")) {
        if (!(Test-Path "$repoRoot\nuget")) {
            New-Item -ItemType directory -Path "$repoRoot\nuget"
        }
        Write-Host "nuget.exe not found. Downloading to $nugetPath"
        Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $nugetPath
    }
}

Write-Host "** Building all NuGet packages. **"
foreach ($file in [System.IO.Directory]::EnumerateFiles("$repoRoot\src", "project.json", "AllDirectories")) {
    Write-Host "Creating NuGet package for $file..."
    $dateSuffix = [System.DateTime]::Now.ToString('eyyMMdd-1')
    Invoke-Expression "$dotnetExePath pack $file -c $Configuration -o $packagesPath --version-suffix $dateSuffix"

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
            $arguments = "push $file $apiKey -Source https://dotnet.myget.org/F/dotnet-corefxlab/api/v2/package"
            Start-Process -FilePath $nugetPath -ArgumentList $arguments -Wait -PassThru
            Write-Host "done"
        } catch [System.Exception] {
            Write-Host "Failed to push nuget package $file with error $_.Exception.Message"
        }
    }
}

Push-Location $PSScriptRoot
[Environment]::CurrentDirectory = $PWD

$apiKey = $args[0]
$nugetPath = ".\packages\NuGet.exe"

foreach ($file in [System.IO.Directory]::EnumerateFiles(".\nuget", "*.nupkg")) {
    try {
        Write-Host "Pushing package $file to MyGet..."
        $arguments = "push $file $apiKey -Source https://dotnet.myget.org/F/dotnet-corefxlab/api/v2/package"
        Start-Process -FilePath $nugetPath -ArgumentList $arguments -Wait -PassThru
        Write-Host "done"
    } catch [System.Exception] {
        Write-Host "Failed to push nuget package $file with error $_.Exception.Message"
    }
}

Pop-Location
[Environment]::CurrentDirectory = $PWD
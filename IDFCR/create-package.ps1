param (
    [int]$targetPath,
    [bool]$skipVersionIncrement = $false
)

enum TargetPath {
    Local = 1;
    Remote = 2;
}

dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Build failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
} else {
    Write-Host "✅ Build succeeded!"
}

dotnet test --no-restore --verbosity normal --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Tests failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
} else {
    Write-Host "✅ Tests succeeded!"
}

$deploymentPath = ""
$versionComponent = 0

switch ([TargetPath]$targetPath) {
    "Local" {
        $deploymentPath = "C:\dev\packages\IDFCR"
        if ($skipVersionIncrement -eq $false) {
            $versionComponent = 4
        }
    }
    "Remote" {
        if ($skipVersionIncrement -eq $false) {
            $versionComponent = 3
        }
    }
    default {
        Write-Host "Invalid target path specified. Use 1 for Local or 2 for Remote."
        exit 1
    }
}

./Set-Property-Info "$PSScriptRoot/Directory.Build.props" $versionComponent

dotnet pack -c Release -o $deploymentPath


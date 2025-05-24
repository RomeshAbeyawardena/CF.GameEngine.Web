param (
    [int]$targetPath,
    [bool]$skipVersionIncrement = $false,
    [bool]$useFeatureFolder = $false
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
$includeSymbols = $false
switch ([TargetPath]$targetPath) {
    "Local" {
        if ($useFeatureFolder) {
            $deploymentPath = "C:\dev\packages\IDFCR\Feature"
        } else {
            $deploymentPath = "C:\dev\packages\IDFCR"
        })

        if ($skipVersionIncrement -eq $false) {
            $versionComponent = 4
        }
        $includeSymbols = $true
        break;
    }
    "Remote" {
        if ($skipVersionIncrement -eq $false) {
            $versionComponent = 3
        }
        break;
    }
    default {
        Write-Host "Invalid target path specified. Use 1 for Local or 2 for Remote."
        exit 1
    }
}

./Set-Property-Info -propFile "$PSScriptRoot/Directory.Build.props" -versionElement $versionComponent -includeSymbols $includeSymbols

dotnet pack -c Release -o $deploymentPath
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Pack failed with exit code $LASTEXITCODE"
}
else {
    Write-Host "✅ Pack succeeded!"
}
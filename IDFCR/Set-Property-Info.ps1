param (
    [string]$propFile,
    [int]$versionElement
)

enum VersionComponent {
    Major = 1
    Minor = 2
    Build = 3
    Revision = 4
}


# Check if the property file exists
if (-not (Test-Path $propFile)) {
    Write-Host "Property file not found: $propFile"
    exit 1
}

# Read the property file
[System.Xml.XmlDocument]$xmlDoc = New-Object System.Xml.XmlDocument
$xmlDoc.Load($propFile)
$properties = $xmlDoc.SelectSingleNode("Project/PropertyGroup")

[System.Version]$version = [System.Version]::Parse($properties.Version)

switch ([VersionComponent]$versionElement) {
    "Major" {
        $version = [System.Version]::new($version.Major + 1, 0, 0, 0)
    }
    "Minor" {
        $version = [System.Version]::new($version.Major, $version.Minor + 1, 0, 0)
    }
    "Build" {
        $version = [System.Version]::new($version.Major, $version.Minor, $version.Build + 1, 0)
    }
    "Revision" {
        $version = [System.Version]::new($version.Major, $version.Minor, $version.Build, $version.Revision + 1)
    }
}

$properties.Version = $version.ToString()
$properties.AssemblyVersion = $version.ToString()
$properties.FileVersion = $version.ToString()
$properties.InformationalVersion = $version.ToString()

$xmlDoc.Save($propFile)
Write-Host "Updated version to $($version.ToString()) in $propFile"
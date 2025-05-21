param (
    [string]$propFile,
    [int]$versionElement,
    [bool]$includeSymbols = $false
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

if ([Enum]::IsDefined([VersionComponent], $versionElement)) { 
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
    Write-Host "Updated version to $($version.ToString()) in $propFile"
}

if ($includeSymbols) {

    $el = $xmlDoc.CreateElement("IncludeSymbols")
    $el.InnerText = "true"
    $node = $properties.AppendChild($el)

    $el = $xmlDoc.CreateElement("IncludeSource")
    $el.InnerText = "true"
    $node = $properties.AppendChild($el)

    $el = $xmlDoc.CreateElement("EmbedUntrackedSources")
    $el.InnerText = "true"
    $node = $properties.AppendChild($el)

    $el = $xmlDoc.CreateElement("SymbolPackageFormat")
    $el.InnerText = "snupkg"
    $node = $properties.AppendChild($el)
} else {
    $remove = @("IncludeSymbols", "IncludeSource", "SymbolPackageFormat", "EmbedUntrackedSources")
    foreach ($name in $remove) {
        $node = $properties.SelectSingleNode($name)
        if ($node) {
            $properties.RemoveChild($node) | Out-Null
        }
    }
}

$xmlDoc.Save($propFile)
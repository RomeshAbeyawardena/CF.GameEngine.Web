param (
	[string]$migrationName
)

if (-not $migrationName) {
    Write-Host "Migration name required. Usage: .\Add-Migration.ps1 'AddSomeEntity'"
    exit 1
}

cd "CF.Identity.Infrastructure.SqlServer"
iex "dotnet ef migrations add '$migrationName' --startup-project '..\CF.Identity.MigrationTool'"
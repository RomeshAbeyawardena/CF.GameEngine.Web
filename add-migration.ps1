param (
	[string]$migrationName
)

if (-not $migrationName) {
    Write-Host "Migration name required. Usage: .\Add-Migration.ps1 'AddSomeEntity'"
    exit 1
}

cd "CF.GameEngine\CF.GameEngine.Infrastructure.SqlServer"
iex "dotnet ef migrations add '$migrationName' --startup-project '..\CF.GameEngine.MigrationTool'"
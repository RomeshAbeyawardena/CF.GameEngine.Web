param (
	[string]$migrationName
)

cd "CF.GameEngine\CF.GameEngine.Infrastructure.SqlServer"
iex "dotnet ef migrations add '$migrationName' --startup-project '..\CF.GameEngine.MigrationTool'"
# CF Authentication basic
A basic opionated version of an Authentication service that covers the basic Machine to Machine authentication 
schemes at minimum.

## Set up

Use `CF.Identity.MigrationUtility` to apply EntityFramework migration scripts to the database.

```
cd CF.Identity.MigrationUtility
dotnet run --project CF.Identity.MigrationUtility.csproj -- --migrate -seed
```

### Optional seeding
If you want to seed the database with some test data, you can use the `-seed` option. This will create a user with the username `admin` and password `admin`. You can change this in the `Program.cs` file.
cd CF.Identity.MigrationUtility
```
dotnet run --project CF.Identity.MigrationUtility.csproj -- --migrate -seed-[seed]
```


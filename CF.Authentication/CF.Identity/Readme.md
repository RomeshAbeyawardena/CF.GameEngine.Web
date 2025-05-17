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

Example secrets.json
```json
{
  "Encryption": {
    "Key": "01621481-926b-4c9b-974c-ca7c040b8d40"
  },
  "ConnectionStrings": {
    "CFIdentity": "Server=localhost,5060;Database=CFIdentity;User Id=sa;Password=<password>;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "Issuer": "https://localhost:7234",
    "Audience": "https://localhost:8224",
    "SigningKey": "dev-Only-Signing-Key_thatIsSecureEnough!2025"
  },
  "Seed": {
    "Client": {
      "SystemClientSecret": "dev-client-secret"
    },
    "User": {
      "Password": "@dmin-123!",
      "EmailAddress": "admin@identity.co",
      "PreferredUsername": "admin",
      "Firstname": "Admin",
      "Lastname": "User"
    }
  }
}
```

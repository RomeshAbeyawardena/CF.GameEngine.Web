# CF Authentication basic
A basic opionated version of an Authentication service that covers the basic Machine to Machine authentication 
schemes at minimum.

## Set up

Use `CF.Identity.MigrationUtility` to apply EntityFramework migration scripts to the database.

```
cd CF.Identity.MigrationUtility
dotnet run --project CF.Identity.MigrationUtility.csproj -- --migrate --seed
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
  "Seed:User:Username": "admin",
  "Seed:User:PrimaryTelephoneNumber": "+44-7123-012345",
  "Seed:User:PreferredUsername": "admin",
  "Seed:User:Password": "@dmin-123!",
  "Seed:User:Lastname": "User",
  "Seed:User:Firstname": "Admin",
  "Seed:User:EmailAddress": "admin@identity.co",
  "Seed:Client:Reference": "dev-system",
  "Seed:Client:Name": "Dev System",
  "Seed:Client:SystemClientSecret": "dev-client-secret",
  "Kestrel:Certificates:Development:Password": "<unique-id>",
  "JwtSettings:SigningKey": "dev-Only-Signing-Key_thatIsSecureEnough!2025",
  "JwtSettings:Issuer": "https://localhost:7234",
  "JwtSettings:Audience": "https://localhost:8224",
  "Encryption:Key": "<unique-id-min-15-length>",
  "ConnectionStrings:CFIdentity": "Server=localhost,5060;Database=CFIdentity;User Id=sa;Password=<password>;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

---
## Api Features

Each endpoint will follow the structure of
- Feature
  - [Entity]
     - Endpoints/CommandHandlers

### Structure
The API is structured around the following main components:

#### Delete
- Delete (by id) - returns non-return type unit result 
- Extension delete methods may live here too with a non-return type unit result 
#### Get
- List (paged list)
-- Accepts paged filters - hard limit 50
- Get (non-paged list) not to exposed publicly - hard limit 500
- Get (by id) - single entity/null

#### Post

- Post (with payload) - returns id

#### Put

- Post (with payload + ID in query) - returns id
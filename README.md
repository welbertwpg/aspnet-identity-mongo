## MongoDB.Identity

This is an updated version of the [MongoDB provider](https://github.com/g0t4/aspnet-identity-mongo) for the ASP.NET Core Identity framework made by [Wes Higbee](https://github.com/g0t4)

The core classes were renamed, there's no reason to conflict them with the original classes from Microsoft.AspNetCore.Identity.

## Usage

- Reference this package in your .csproj: MongoDB.Identity
- Then, in ConfigureServices--or wherever you are registering services--include the following to register both the Identity services and MongoDB stores:

```csharp
services.AddIdentityWithMongoStores("mongodb://localhost/myDB");
```

- If you want to customize what is registered, refer to the tests for further options (CoreTests/MongoIdentityBuilderExtensionsTests.cs)
- Remember with the Identity framework, the whole point is that both a `UserManager` and `RoleManager` are provided for you to use, here's how you can resolve instances manually. Of course, constructor injection is also available.

```csharp
var userManager = provider.GetService<UserManager<IdentityUser>>();
var roleManager = provider.GetService<RoleManager<IdentityRole>>();
```

## Updates

- NetStandard 2.0
- Microsoft.AspNetCore.Identity v2.2.0
- MongoDB.Driver v2.8.1
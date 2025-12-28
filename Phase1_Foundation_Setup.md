# Microsoft Identity Integration

## Purpose
Adding security to AppRazor and AppMvc by integrating ASP.NET Core Identity and adding Identity models to Application Database.

## Overview
This guide covers the initial setup of Microsoft Identity in both Razor Pages and MVC applications.

## Steps

### 1. Update User Model
In Models/Authorization, create a User class that inherit from `IdentityUser` and map `UserId` to `IdentityUser.Id`:

```csharp
public class User : IdentityUser<Guid>
{
    // Implementation details
}
```

### 2. Update DbContext - Add Namespaces
In DbContext add required namespaces:

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
```

### 3. Update DbContext - Inherit from IdentityDbContext
Have `MainDbContext` inherit from `IdentityDbContext` instead of `DbContext`:

```csharp
public class MainDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
```

## AppRazor Configuration

### 4. Configure Authentication and Authorization
In `Program.cs`, ensure the app uses Authentication and Authorization:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### 5. Add Package Reference
In `AppRazor.csproj`, add the following reference:

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="9.0.8" />
```

### 6. Configure Identity Services
In `Program.cs`, add and configure Identity services:

```csharp
// Add IdentityServices to DbContext.MainDbContext
builder.Services.AddDefaultIdentity<User>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<DbContext.MainDbContext>();
```

### 7. Database Migration and Update
With Terminal in folder `_scripts`, run:

**Linux/macOS:**
```bash
./database-rebuild-all.sh sql-music sqlserver docker dbo ../AppRazor
```

**Windows:**
```powershell
./database-rebuild-all.ps1 sql-music sqlserver docker dbo ../AppRazor
```

**Important:** 
- Ensure no errors from build, migration or database update
- Execute the `initDatabase.sql` script in a database access tool

## AppMvc Configuration

### 4. Configure Authentication and Authorization
In `Program.cs`, ensure the app uses Authentication and Authorization:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### 5. Add Package Reference
In `AppMvc.csproj`, add the following reference:

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.UI" Version="9.0.8" />
```

### 6. Configure Identity Services
In `AppMvc.Program.cs`, add and configure Identity services:

```csharp
// Add IdentityServices to DbContext.MainDbContext
builder.Services.AddDefaultIdentity<User>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<DbContext.MainDbContext>();
```

### 7. Database Migration and Update
With Terminal in folder `_scripts`, run:

**Linux/macOS:**
```bash
./database-rebuild-all.sh sql-music sqlserver docker dbo ../AppMvc
```

**Windows:**
```powershell
./database-rebuild-all.ps1 sql-music sqlserver docker dbo ../AppMvc
```

**Important:** 
- Ensure no errors from build, migration or database update
- Execute the `initDatabase.sql` script in a database access tool
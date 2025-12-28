# User Registration with Email Verification

## Purpose
Adding security to AppRazor and AppMvc by implementing ASP.NET Core Identity registration capabilities with email verification.

## Overview
This guide covers implementing user registration with email confirmation functionality in both Razor Pages and MVC applications.

## Common Steps

### 1. Extend User Model
In `Models/Authorization`, add first name and last name properties to the User class:

```csharp
public string FirstName { get; set; }
public string LastName { get; set; }
```

### 2. Add Email Service Package
In Services project, add the following NuGet package:

```xml
<PackageReference Include="MailKit" Version="4.8.0" />
```

### 3. Create Email Service
In Services, add a service to send email:

```csharp
public class EmailService : IEmailSender
{
    // Implementation details
}
```

## AppRazor Implementation

### 4. Configure Email Service
In `Program.cs`, add EmailService as a transient service using Dependency Injection:

```csharp
builder.Services.AddTransient<IEmailSender, EmailService>();
```

### 5. Create Email Storage Directory
In the application, create a directory `TempMail` for storing all emails.

### 6. Database Migration and Update
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

### 7. Create Registration Pages
In `Pages`, add folder `Account` with the following pages:
- Add a **Register** Razor page and model
- Add a **RegisterConfirmation** page and model  
- Add a **ConfirmEmail** Razor page and model

### 8. Update Navigation Menu
In `Pages/Shared/_Layout`, add a menu item to the Register page.

### 9. Testing Registration
1. Execute the application and register users to the database
2. Check the `AspNetUser` table in the database
3. Check the `TempMail` folder and open the email
4. Click on the link in the email and verify email confirmation
5. Check the `AspNetUser` table again in the database

## AppMvc Implementation

### 4. Configure Email Service
In `Program.cs`, add EmailService as a transient service using Dependency Injection:

```csharp
builder.Services.AddTransient<IEmailSender, EmailService>();
```

### 5. Create Email Storage Directory
In the application, create a directory `TempMail` for storing all emails.

### 6. Database Migration and Update
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

### 7. Create Registration Components
In `AccountController`, add the following actions:
- `ConfirmEmail(...)`
- `Register(...)`
- `RegisterSave(...)`
- `RegisterConfirmation(...)`

In `Models`, add:
- `RegisterViewModel`

In `Views/Account`, add:
- `Register.cshtml`
- `RegisterConfirmation.cshtml`

### 8. Update Navigation Menu
In `Views/Shared/_Layout`, add a menu item to the Register page.

### 9. Testing Registration
1. Execute the application and register users to the database
2. Check the `AspNetUser` table in the database
3. Check the `TempMail` folder and open the email
4. Click on the link in the email and verify email confirmation
5. Check the `AspNetUser` table again in the database
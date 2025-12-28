# User Login Implementation

## Purpose
Adding security to AppRazor and AppMvc by implementing ASP.NET Core Identity login capabilities.

## Overview
This guide covers implementing user login and logout functionality with custom authentication pages in both Razor Pages and MVC applications.

## AppRazor Implementation

### 1. Create Authentication Pages
In `Pages/Account/`, add the following pages:
- Add a **Login** Razor page and model
- Add a **Logout** Razor page and model

### 2. Configure Application Cookies
In `Program.cs`, after `builder.Services.AddDefaultIdentity<User>(...)`, add configuration for custom login pages:

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Allow both HTTP and HTTPS in development
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = "GoodMusicAuth";
});
```

### 3. Create Login Partial View
In `Pages/Shared`, add a Razor page partial named `_LoginPartial` (it can be empty initially).

### 4. Update Layout with Authentication
In `_Layout.cshtml`, add the following using statements and injection:

```csharp
@using Microsoft.AspNetCore.Identity;
@using Models;
@inject UserManager<User> userManager;
```

### 5. Configure Authentication Menu
In `_Layout.cshtml`, add `@if (!User.Identity.IsAuthenticated)` to control Login, Register, and Logout menu items visibility.

### 6. Testing Login Functionality
Execute the application and test user login and logout functionality.

## AppMvc Implementation

### 1. Create Authentication Actions
In `AccountController`, add the following actions:
- `LoginOk(...)`
- `Logout(...)`

In `Models`, add:
- `LoginViewModel`

In `Views/Account`, add:
- `Login.cshtml`

### 2. Configure Application Cookies
In `Program.cs`, after `builder.Services.AddDefaultIdentity<User>(...)`, add configuration for custom login pages:

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Allow both HTTP and HTTPS in development
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = "GoodMusicAuth";
});
```

### 3. Update Layout with Authentication
In `Views/Shared/_Layout.cshtml`, add the following using statements and injection:

```csharp
@using Microsoft.AspNetCore.Identity;
@using Models;
@inject UserManager<User> userManager;
```

### 4. Configure Authentication Menu
In `_Layout.cshtml`, add `@if (!User.Identity.IsAuthenticated)` to control Login, Register, and Logout menu items visibility.

## Key Features

### Cookie Configuration
- **Login Path**: `/Account/Login`
- **Access Denied Path**: `/Account/Login`
- **Logout Path**: `/Account/Logout`
- **Session Duration**: 60 minutes with sliding expiration
- **Cookie Name**: `GoodMusicAuth`
- **Security**: HttpOnly, SameAsRequest secure policy, Lax SameSite mode

### Menu Adaptations
The layout shows context-appropriate menu items:
- **Not authenticated**: Display Login and Register links
- **Authenticated**: Display Logout link only

### Testing Checklist
1. ✅ Test user login with valid credentials
2. ✅ Test user login with invalid credentials
3. ✅ Test user logout functionality
4. ✅ Verify menu items change based on authentication state
5. ✅ Test cookie expiration and sliding window renewal
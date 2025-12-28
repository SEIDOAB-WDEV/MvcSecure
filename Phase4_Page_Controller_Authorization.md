# Page and Controller Level Authorization

## Purpose
Adding security to AppRazor and AppMvc by implementing server-side authorization at page and controller levels to restrict access to unauthorized users.

## Overview
This guide covers implementing authorization policies that forbid access to pages (Razor Pages) or controllers/actions (MVC) if users are not authorized.

## AppRazor Implementation

### 1. Configure Page-Level Authorization
In `Program.cs`, set the Members folder to require authorization for all pages in the folder:

```csharp
builder.Services.AddRazorPages(option =>
{
    option.Conventions.AuthorizeFolder("/Members");
});
```

### 2. Testing Page Authorization
1. Execute the application
2. Try to access pages with and without being logged in
3. Verify that unauthorized users are redirected to login

### 3. Important Note
Notice that individual actions like "Delete group" may still not be properly authorized, as authorization is implemented at the page level. This will be addressed in the next step with model-level authorization.

## AppMvc Implementation

### 1. Apply Authorization Attributes
Apply `[Authorize]` and `[AllowAnonymous]` attributes strategically:

**AccountController:**
```csharp
[Authorize]
public class AccountController : Controller
{
    // All actions require authorization by default
}
```

**GroupController:**
```csharp
[Authorize]
public class GroupController : Controller
{
    [AllowAnonymous]
    public ActionResult ListOfGroups(...)
    {
        // Public action - no authentication required
    }
    
    [AllowAnonymous]
    public ActionResult SearchGroup(...)
    {
        // Public action - no authentication required
    }
    
    [AllowAnonymous]
    public ActionResult ViewGroup(...)
    {
        // Public action - no authentication required
    }
    
    // Other actions require authorization (inherited from class level [Authorize])
}
```

### 2. Testing Controller Authorization
1. Execute the application
2. Try to access different controller actions with and without being logged in
3. Verify that:
   - `ListOfGroups`, `SearchGroup`, and `ViewGroup` are accessible without login
   - Other actions require authentication
   - Delete group operations are properly authorized

### 3. Authorization Advantage
Notice that "Delete group" is properly authorized in MVC because authorization is applied at both controller and action levels, providing more granular control.

## Authorization Levels Comparison

| Application Type | Authorization Level | Scope | Granularity |
|-----------------|-------------------|-------|-------------|
| **Razor Pages** | Page Level | Entire folders/pages | Coarse-grained |
| **MVC** | Controller/Action Level | Individual actions | Fine-grained |

## Key Differences

### Razor Pages
- ✅ Simple folder-based authorization
- ✅ Easy to implement for entire sections
- ⚠️ Less granular control over individual operations
- ⚠️ May require additional model-level authorization for specific actions

### MVC
- ✅ Fine-grained control at action level
- ✅ Flexible attribute-based authorization
- ✅ Better separation between public and private actions
- ✅ More precise authorization boundaries

## Testing Checklist

### For Both Applications:
1. ✅ Test access without authentication - should redirect to login
2. ✅ Test access with valid authentication - should allow access
3. ✅ Test logout - should remove access to protected resources

### Specific to Razor Pages:
1. ✅ Test folder-level authorization works for all pages in `/Members`
2. ✅ Verify individual operations may need additional authorization

### Specific to MVC:
1. ✅ Test `[AllowAnonymous]` actions work without login
2. ✅ Test `[Authorize]` actions require login
3. ✅ Verify controller-level authorization applies to all actions unless overridden
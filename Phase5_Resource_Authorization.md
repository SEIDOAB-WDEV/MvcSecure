# Model-Level Authorization

## Purpose
Adding security to AppRazor and AppMvc by implementing CRUD user authorization at the model level, providing server-side resource-level authorization that forbids access if users are not authorized to access specific resources.

## Overview
This guide covers implementing fine-grained authorization at the model level, allowing or denying access to specific resources based on user permissions and CRUD operations.

## Common Implementation

### 1. Create Authorization Infrastructure
In folder `Models/Authorization`, add two classes:

#### CrudOperations Class
```csharp
public static class CrudOperations
{
    public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = "Create" };
    public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = "Read" };
    public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = "Update" };
    public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = "Delete" };
}
```

#### MusicGroupAuthorizationHandler Class
```csharp
public class MusicGroupAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, MusicGroup>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        MusicGroup resource)
    {
        // Authorization logic implementation
        // Determine if the current user can perform the operation on the resource
        
        if (requirement == CrudOperations.Delete)
        {
            // Implement delete authorization logic
            if (context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
        }
        
        return Task.CompletedTask;
    }
}
```

## AppRazor Implementation

### 2. Update ListOfGroups Page
In `ListOfGroups.cshtml.cs`, add required namespaces:

```csharp
using Microsoft.AspNetCore.Authorization;
using Models.Authorization;
```

### 3. Inject Authorization Service
Inject `IAuthorizationService` into the ListOfGroups constructor and set a private field `_authService` to store the injected service:

```csharp
private readonly IAuthorizationService _authorizationService;

public ListOfGroupsModel(IService service, IAuthorizationService authorizationService)
{
    _service = service;
    _authorizationService = authorizationService;
}
```

### 4. Implement Authorization Check
In `ListOfGroups.cshtml.cs`, modify `OnPostDeleteGroup` to check if the user is authorized to delete a group:

```csharp
public async Task<IActionResult> OnPostDeleteGroup(Guid groupId)
{
    var mg = (await _service.ReadMusicGroupAsync(groupId, true)).Item;
    var result = await _authorizationService.AuthorizeAsync(User, mg, CrudOperations.Delete);
    
    if (!result.Succeeded)
    {
        return Forbid();
    }
    
    // Proceed with delete operation
    await _service.DeleteMusicGroupAsync(groupId);
    return RedirectToPage();
}
```

### 5. Configure Authorization Handler
In `Program.cs`, add model authorization service:

```csharp
// Model Authorization
builder.Services.AddSingleton<IAuthorizationHandler, MusicGroupAuthorizationHandler>();
```

### 6. Testing Model Authorization
1. Run the application
2. Verify that you are redirected to Login if you try to delete a group without authentication
3. When logged in, verify that you can delete groups successfully

## AppMvc Implementation

### 2. Configure Authorization Handler
In `Program.cs`, add model authorization service:

```csharp
// Model Authorization
builder.Services.AddSingleton<IAuthorizationHandler, MusicGroupAuthorizationHandler>();
```


## Key Benefits

### Fine-Grained Control
- ✅ **Resource-specific**: Authorization per individual resource instance
- ✅ **Operation-specific**: Different rules for Create, Read, Update, Delete
- ✅ **User-specific**: Context-aware decisions based on user identity and roles

### Security Advantages
- ✅ **Defense in Depth**: Multiple authorization layers (Page/Controller + Model)
- ✅ **Consistent Logic**: Same authorization rules across different UI patterns
- ✅ **Centralized Rules**: Authorization logic in dedicated handlers

## CRUD Operations Matrix

| Operation | Description | Typical Use Case |
|-----------|-------------|-----------------|
| **Create** | User can create new resources | Add new music groups |
| **Read** | User can view resources | Display group details |
| **Update** | User can modify existing resources | Edit group information |
| **Delete** | User can remove resources | Remove music groups |

## Testing Checklist

### AppRazor:
1. ✅ Test delete operation without authentication - should redirect to login
2. ✅ Test delete operation with authentication - should succeed
3. ✅ Verify authorization service is properly injected
4. ✅ Test `Forbid()` response for unauthorized operations

### AppMvc:
1. ✅ Verify authorization handler is registered
2. ✅ Prepare foundation for UI-level authorization implementation
3. ✅ Test that basic controller authorization still works

### Authorization Handler:
1. ✅ Test handler receives correct operation requirements
2. ✅ Verify resource-specific authorization logic
3. ✅ Test different CRUD operations return appropriate results
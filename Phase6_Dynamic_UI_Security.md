# UI Authorization - Dynamic Element Visibility

## Purpose
Adding security to AppRazor and AppMvc by adjusting UI to dynamically hide and show resource UI elements based on whether the user is authorized to access specific resources.

## Overview
This guide covers implementing dynamic UI authorization that controls the visibility of buttons, links, and other interactive elements based on user permissions at the resource level.

## AppRazor Implementation

### 1. Add Required Directives
In `ListOfGroups.cshtml` and `ViewGroup.cshtml`, ensure the following using statements and injections are in place:

```csharp
@using Microsoft.AspNetCore.Authorization;
@using Microsoft.AspNetCore.Identity;
@using Models.Authorization;

@inject IAuthorizationService _authService;
```

### 2. Implement Authorization Check in ListOfGroups
In `ListOfGroups.cshtml`, make a call to `AuthorizeAsync` to check if the user is authorized to access operations on the model:

```csharp
@{
    ViewData["Title"] = "List groups";
    var authResult = await _authService.AuthorizeAsync(User, Model.MusicGroups.First(), CrudOperations.Delete);
}
```

### 3. Implement Authorization Check in ViewGroup
In `ViewGroup.cshtml`, make a call to `AuthorizeAsync` to check if the user is authorized to access operations on the specific resource:

```csharp
@{
    ViewData["Title"] = "View group";
    var authResult = await _authService.AuthorizeAsync(User, Model.MusicGroup, CrudOperations.Delete);
}
```

### 4. Conditional UI Rendering in ListOfGroups
In `ListOfGroups.cshtml`, conditionally display New, Edit, and Delete buttons based on user authorization:

```html
<div class="@(authResult.Succeeded ? "col-md-10" : "col-md-12") themed-grid-head-col">
    Group Name
</div>

@if (authResult.Succeeded)
{
    <div class="col-md-2 themed-grid-head-col">
        <a class="btn btn-primary" asp-page="./CreateGroup">New Group</a>
    </div>
}

<!-- In the group listing loop -->
@foreach (var group in Model.MusicGroups)
{
    <div class="@(authResult.Succeeded ? "col-md-10" : "col-md-12")">
        @group.Name
    </div>
    
    @if (authResult.Succeeded)
    {
        <div class="col-md-2">
            <a class="btn btn-sm btn-secondary" asp-page="./EditGroup" asp-route-id="@group.MusicGroupId">Edit</a>
            <form method="post" style="display:inline;">
                <input type="hidden" name="groupId" value="@group.MusicGroupId" />
                <button type="submit" class="btn btn-sm btn-danger" asp-page-handler="DeleteGroup">Delete</button>
            </form>
        </div>
    }
}
```

### 5. Conditional UI Rendering in ViewGroup
In `ViewGroup.cshtml`, conditionally display the Edit button:

```html
@if (authResult.Succeeded)
{
    <div class="mt-3">
        <a class="btn btn-primary" asp-page="./EditGroup" asp-route-id="@Model.MusicGroup.MusicGroupId">Edit Group</a>
        <form method="post" style="display:inline;" class="ms-2">
            <input type="hidden" name="groupId" value="@Model.MusicGroup.MusicGroupId" />
            <button type="submit" class="btn btn-danger" asp-page-handler="DeleteGroup">Delete Group</button>
        </form>
    </div>
}
```

### 6. Testing UI Authorization
Run the application and verify that buttons for user actions (Delete, Edit, New) are only visible when you are logged in.

## AppMvc Implementation

### 1. Add Required Directives
In `Views/Group/ListOfGroups.cshtml` and `ViewGroup.cshtml`, ensure the following using statements and injections are in place:

```csharp
@using Microsoft.AspNetCore.Authorization;
@using Microsoft.AspNetCore.Identity;
@using Models.Authorization;

@inject IAuthorizationService _authService;
```

### 2. Implement Authorization Check in ListOfGroups
In `ListOfGroups.cshtml`, make a call to `AuthorizeAsync` to check if the user is authorized:

```csharp
@{
    ViewData["Title"] = "List groups";
    var authResult = await _authService.AuthorizeAsync(User, Model.MusicGroups.First(), CrudOperations.Delete);
}
```

### 3. Implement Authorization Check in ViewGroup
In `ViewGroup.cshtml`, make a call to `AuthorizeAsync`:

```csharp
@{
    ViewData["Title"] = "View group";
    var authResult = await _authService.AuthorizeAsync(User, Model.MusicGroup, CrudOperations.Delete);
}
```

### 4. Conditional UI Rendering in ListOfGroups
In `ListOfGroups.cshtml`, conditionally display action buttons:

```html
<div class="@(authResult.Succeeded ? "col-md-10" : "col-md-12") themed-grid-head-col">
    Group Name
</div>

@if (authResult.Succeeded)
{
    <div class="col-md-2 themed-grid-head-col">
        @Html.ActionLink("New Group", "CreateGroup", "Group", null, new { @class = "btn btn-primary" })
    </div>
}

<!-- In the group listing -->
@foreach (var group in Model.MusicGroups)
{
    <div class="@(authResult.Succeeded ? "col-md-10" : "col-md-12")">
        @Html.ActionLink(group.Name, "ViewGroup", "Group", new { id = group.MusicGroupId })
    </div>
    
    @if (authResult.Succeeded)
    {
        <div class="col-md-2">
            @Html.ActionLink("Edit", "EditGroup", "Group", new { id = group.MusicGroupId }, new { @class = "btn btn-sm btn-secondary" })
            @using (Html.BeginForm("DeleteGroup", "Group", new { id = group.MusicGroupId }, FormMethod.Post, new { style = "display:inline;" }))
            {
                <button type="submit" class="btn btn-sm btn-danger">Delete</button>
            }
        </div>
    }
}
```

### 5. Conditional UI Rendering in ViewGroup
In `ViewGroup.cshtml`, conditionally display the Edit button:

```html
@if (authResult.Succeeded)
{
    <div class="mt-3">
        @Html.ActionLink("Edit Group", "EditGroup", "Group", new { id = Model.MusicGroup.MusicGroupId }, new { @class = "btn btn-primary" })
        @using (Html.BeginForm("DeleteGroup", "Group", new { id = Model.MusicGroup.MusicGroupId }, FormMethod.Post, new { @class = "d-inline ms-2" }))
        {
            <button type="submit" class="btn btn-danger">Delete Group</button>
        }
    </div>
}
```

### 6. Testing UI Authorization
Run the application and verify that buttons for user actions (Delete, Edit, New) are only visible when you are logged in.

## UI Authorization Patterns

### Responsive Layout Pattern
```html
<div class="@(authResult.Succeeded ? "col-md-10" : "col-md-12")">
    <!-- Content takes full width when user can't perform actions -->
</div>
```

### Conditional Action Buttons
```html
@if (authResult.Succeeded)
{
    <!-- Only show action buttons to authorized users -->
    <button type="submit" class="btn btn-danger">Delete</button>
}
```

## Security Benefits

### User Experience
- ✅ **Clean Interface**: Users only see actions they can perform
- ✅ **Reduced Confusion**: No misleading buttons that result in access denied
- ✅ **Professional Appearance**: UI adapts dynamically to user permissions

### Security Advantages
- ✅ **Defense in Depth**: UI-level filtering plus server-side authorization
- ✅ **Consistent Experience**: Authorization state reflected in UI
- ✅ **Reduced Attack Surface**: Fewer entry points for unauthorized actions

## Testing Checklist

### Visual Testing:
1. ✅ **Logged Out Users**: Verify no action buttons are visible
2. ✅ **Logged In Users**: Verify all authorized action buttons appear
3. ✅ **Layout Responsiveness**: Ensure proper column sizing based on authorization
4. ✅ **Cross-Browser Testing**: Verify consistent behavior across browsers

### Functional Testing:
1. ✅ **Authorization Service**: Verify `IAuthorizationService` injection works
2. ✅ **Resource-Specific Auth**: Test authorization checks against actual resources
3. ✅ **Performance**: Ensure authorization checks don't significantly impact page load
4. ✅ **Error Handling**: Test behavior when authorization service fails

### Security Testing:
1. ✅ **Server-Side Validation**: Verify UI hiding doesn't bypass server authorization
2. ✅ **JavaScript Manipulation**: Test that manually showing hidden elements still fails server-side
3. ✅ **Direct URL Access**: Confirm direct action URLs still require proper authorization
4. ✅ **Session Management**: Test UI updates correctly when authentication state changes

## Key Implementation Notes

- **Resource-Specific**: Authorization checks are performed against actual model instances
- **Operation-Specific**: Different UI elements can be controlled by different CRUD operations
- **Performance Conscious**: Authorization checks are cached per request
- **Maintainable**: Centralized authorization logic in handlers, not scattered in views
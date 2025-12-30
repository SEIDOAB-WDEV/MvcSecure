# Phase 7: Access Secure WebApi using JWT Tokens

## Overview
This phase transforms the application from accessing a public WebAPI to accessing a secure WebAPI that requires JWT token authentication. The implementation integrates JWT token handling with ASP.NET Core Identity, storing tokens securely in authentication cookies and automatically adding them to WebAPI requests.

## Key Changes Summary
1. **JWT Token Management**: Created infrastructure to obtain, store, and use JWT tokens
2. **HTTP Client Enhancement**: Added automatic token injection for WebAPI requests
3. **Authentication Integration**: Combined Identity authentication with WebAPI token storage
4. **Configuration Updates**: Updated endpoints to use secure WebAPI
5. **UI/Navigation Changes**: Added Overview page and reorganized navigation

## Detailed Implementation Steps

### 1. JWT Token Handler Implementation

**File**: [`Encryptions/JwtTokenHandler.cs`](Encryptions/JwtTokenHandler.cs) (NEW)

Create a `DelegatingHandler` that automatically adds JWT Bearer tokens to outgoing HTTP requests:

- **Purpose**: Intercepts all HTTP requests to WebAPI and adds Authorization header
- **Token Retrieval**: Gets token from Identity authentication cookie using `GetTokenAsync()`
- **Automatic Injection**: Adds `Authorization: Bearer <token>` header to requests
- **Logging**: Provides detailed logging for debugging token injection
- **Focused Responsibility**: Handles only HTTP message interception and token injection

**Key Features**:
- Retrieves stored JWT token from `IdentityConstants.ApplicationScheme`
- Automatically adds Bearer token to all WebAPI requests
- Comprehensive logging for troubleshooting
- Graceful handling of missing tokens

### 2. JWT Token Storage Implementation

**File**: [`Encryptions/JwtTokenStorage.cs`](Encryptions/JwtTokenStorage.cs) (NEW)

Static utility class for handling JWT token storage in authentication properties:

- **Purpose**: Centralized JWT token management and authentication property handling
- **Token Storage**: Manages JWT tokens in ASP.NET Core authentication properties
- **User Sign-in**: Handles user authentication with stored JWT tokens
- **Separation of Concerns**: Dedicated class for token storage operations

**Key Methods**:
- `StoreTokenAsync<TUser>()`: Signs in user with JWT token stored in authentication properties
- `CreateAuthenticationPropertiesWithToken()`: Creates authentication properties with JWT token
- Generic implementation supporting any Identity user type
- Clean separation from HTTP message handling

### 3. WebAPI Login Service

**File**: [`Services/LoginServiceWapi.cs`](Services/LoginServiceWapi.cs) (NEW)

Implement WebAPI login service to authenticate and obtain JWT tokens:

- **Interface**: Implements `ILoginService` for WebAPI authentication
- **Token Acquisition**: Calls WebAPI `/guest/loginuser` endpoint
- **Response Handling**: Returns `LoginUserSessionDto` containing JWT token
- **Error Management**: Ensures successful HTTP responses with proper error handling

### 4. Login Page Enhancement

**File**: [`AppRazor/Pages/Account/Login.cshtml.cs`](AppRazor/Pages/Account/Login.cshtml.cs)
**File**: [`AppMvc/Controllers/AccountController.cs`](AppMvc/Controllers/AccountController.cs)
  In `public async Task<IActionResult> LoginOk(LoginViewModel vm)`

Enhanced login process to integrate WebAPI authentication:

**Key Changes**:
- **Service Injection**: Added `ILoginService` dependency injection
- **Enhanced Login Flow**: After successful Identity login, authenticate with WebAPI
- **Token Storage**: Direct integration with `JwtTokenStorage.StoreTokenAsync()` for token management
- **Error Handling**: Graceful handling of WebAPI authentication failures
- **User Feedback**: Clear error messages for different failure scenarios

### 5. Registration Page Enhancement

**File**: [`AppRazor/Pages/Account/Register.cshtml.cs`](AppRazor/Pages/Account/Register.cshtml.cs)
**File**: [`AppMvc/Controllers/AccountController.cs`](AppMvc/Controllers/AccountController.cs)
  In `public async Task<IActionResult> RegisterSave(RegisterViewModel vm)`

Updated registration to include WebAPI authentication:

**Modifications**:
- Added WebAPI login service injection
- Modified registration success flow to include WebAPI authentication
- Enhanced error handling for WebAPI authentication failures
- Consistent user experience with login page

### 6. Program.cs Configuration Updates

**File**: [`AppRazor/Program.cs`](AppRazor/Program.cs)
**File**: [`AppMvc/Program.cs`](AppMvc/Program.cs)

**Cookie Configuration Enhancement**:
```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
  ... Exisiting code

    // Enable saving tokens in the cookie - required for httpContext.GetTokenAsync to work
    options.Events.OnSigningIn = context =>
    {
        context.Properties.IsPersistent = true;
        return Task.CompletedTask;
    };
});
```
- Enables token storage in authentication properties

**HTTP Client Configuration**:
```csharp
builder.Services.AddTransient<JwtTokenHandler>();
builder.Services.AddHttpClient(name: "MusicWebApi", configureClient: options =>
{
    ...Exisiting code
})
.AddHttpMessageHandler<JwtTokenHandler>();
```
- Registers JWT token handler for HTTP message interception
- Configures HttpClient to use token handler for automatic token injection
- JwtTokenHandler focused solely on HTTP request enhancement

**Service Registration**:
```csharp
builder.Services.AddScoped<ILoginService, LoginServiceWapi>();
```
- Registers WebAPI login service
- JwtTokenStorage used as static utility class (no DI registration needed)

- Similar configuration updates for MVC application

### 7. Configuration Updates

**Files**: 
- [`AppRazor/appsettings.json`](AppRazor/appsettings.json)
- [`AppMvc/appsettings.json`](AppMvc/appsettings.json)

**Changes**:
- **WebApiBaseUri**: Updated from public to secure endpoint and default to WebApi:
```csharp
  "DataService": 
  {
    "DataSource": "WebApi",    
    "WebApiBaseUri": "https://music.api.secure.seido.se/api/"
  }
```

### 8. Compile and Test the Applications

**Build and Run Both Applications**:

After implementing all the changes above, compile and run both applications to test the new secure WebAPI integration:

**Build the Solution**:
```csharp
dotnet build
```

**Run AppRazor**:
```csharp
dotnet run --project AppRazor
```

**Run AppMvc** (in a separate terminal):
```csharp
dotnet run --project AppMvc
```

**Important Note**: 
- **User Authorization Change**: With the switch to the secure WebAPI endpoint, **users are no longer automatically authorized** to access WebAPI resources
- **Authentication Required**: Users must now log in to obtain JWT tokens for WebAPI access
- **Dual Authentication**: The application now requires both Identity authentication (for web pages) AND WebAPI JWT token authentication (for API calls)
- **Behavior Change**: Unauthenticated users will see limited functionality compared to the previous public WebAPI implementation

**Testing Steps**:
1. **Before Login**: Navigate to pages that display data - you should see limited or no data from WebAPI
2. **After Login**: Log in with valid credentials and verify that WebAPI data is now accessible
3. **Token Verification**: Check browser developer tools to confirm JWT Bearer tokens are being sent with WebAPI requests
4. **Error Handling**: Test the graceful degradation when WebAPI authentication fails

### 9. UI and Navigation Enhancements

**New DataSourceInfo Page**:
- **File**: [`AppRazor/Pages/DataSourceInfo.cshtml`](AppRazor/Pages/DataSourceInfo.cshtml) (NEW)
- **File**: [`AppRazor/Pages/DataSourceInfo.cshtml.cs`](AppRazor/Pages/DataSourceInfo.cshtml.cs) (NEW)
- **File**: [`AppMvc/Views/Home/DataSourceInfo.cshtml`](AppMvc/Views/Home/DataSourceInfo.cshtml) (NEW)
- **File**: [`AppMvc/Models/DataSourceInfoViewModel.cs`](AppMvc/Models/DataSourceInfoViewModel.cs) (NEW)

**Controller Enhancement**:
- **File**: [`AppMvc/Controllers/HomeController.cs`](AppMvc/Controllers/HomeController.cs) (ENHANCED)
- **Enhancement**: Added `DataSourceInfo` action method to handle the new DataSourceInfo page
- **Purpose**: Provides information about current data source configuration and connection status
- **Integration**: Works with the new DataSourceInfoViewModel to display WebAPI connection details

**Navigation Updates**:
- **File**: [`AppRazor/Pages/Shared/_Layout.cshtml`](AppRazor/Pages/Shared/_Layout.cshtml)
- **File**: [`AppMvc/Views/Shared/_Layout.cshtml`](AppMvc/Views/Shared/_Layout.cshtml)

**Changes**:
- Added "DataSourceInfo" link accessible to all users
- Moved "Famous groups" link to authenticated users only section
- Updated URLs to reflect new page structure (`~/Members/ListOfGroups`)

**Page Organization**:
- Moved `ListOfGroups.cshtml` to `Members/` folder
- Updated all navigation links and redirects to new structure

**Authorization Updates**:
- Remove `[AllowAnonymous]` attributes from all GroupController endpoints in the MVC application
- Ensures proper authentication is required for accessing music group functionality
- Maintains security consistency across the application

### 10. Authentication Flow Documentation

**File**: [`AuthorizationFlow.md`](AuthorizationFlow.md) (NEW)

Created comprehensive documentation explaining the dual authentication system:

**Two Authentication Systems**:
1. **ASP.NET Core Identity**: Cookie-based authentication for Razor Pages
2. **WebAPI JWT Token**: Bearer token authentication for API access

**Complete Flow Documentation**:
- User login process
- Token acquisition and storage
- HTTP request interception
- WebAPI authentication
- Database connection selection based on JWT claims

## Security Architecture

### Token Storage Strategy
- **Secure Storage**: JWT tokens stored in ASP.NET Core authentication properties via `JwtTokenStorage`
- **Cookie Integration**: Tokens travel with Identity cookies automatically
- **Automatic Retrieval**: `JwtTokenHandler` transparently retrieves and uses tokens
- **Separation of Concerns**: Token storage (`JwtTokenStorage`) separated from HTTP handling (`JwtTokenHandler`)

### Authentication Layers
1. **Frontend Authentication**: ASP.NET Core Identity (cookie-based)
2. **API Authentication**: JWT Bearer tokens
3. **Database Access**: Role-based connection selection via JWT claims
4. **Token Management**: Centralized token operations via `JwtTokenStorage`

### Component Responsibilities
- **`JwtTokenHandler`**: HTTP message interception and Bearer token injection
- **`JwtTokenStorage`**: JWT token storage in authentication properties and user sign-in
- **`LoginServiceWapi`**: WebAPI authentication and JWT token acquisition

### Error Handling
- **Graceful Degradation**: Application continues with Identity authentication if WebAPI fails
- **User Feedback**: Clear error messages for authentication failures
- **Logging**: Comprehensive logging for debugging and monitoring

## Benefits of This Implementation

1. **Seamless User Experience**: Users authenticate once, system handles WebAPI authentication
2. **Security**: JWT tokens securely stored and automatically managed via dedicated classes
3. **Maintainability**: Centralized and separated token handling logic (`JwtTokenStorage` vs `JwtTokenHandler`)
4. **Single Responsibility**: Each class has a focused responsibility in the authentication flow
5. **Scalability**: Easy to extend to multiple WebAPI endpoints with clear component boundaries
6. **Debugging**: Extensive logging for troubleshooting with clear component separation
7. **Flexibility**: Fallback mechanisms for API authentication failures
8. **Testability**: Separated concerns make individual components easier to test

## Testing Considerations

1. **Token Expiration**: Ensure proper handling of expired tokens
2. **Network Failures**: Test WebAPI authentication failure scenarios
3. **Mixed Authentication**: Verify both Identity and WebAPI authentication work together
4. **Token Refresh**: Consider implementing token refresh mechanism for long sessions
5. **Security**: Validate tokens are properly secured and not exposed in logs or client-side

## Migration Notes

When migrating from Phase 6 (3-mvc-secure) to Phase 7:
1. Install required NuGet packages for JWT handling
2. Update configuration endpoints
3. Add new service registrations
4. Test authentication flow thoroughly
5. Monitor logs for token handling issues
6. Verify all WebAPI calls include proper authorization headers
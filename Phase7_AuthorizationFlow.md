Two Different Authentication Systems Working in Harmony

1. ASP.NET Core Identity (AppRazor)

Purpose: Authenticates the user in the Razor Pages application
Mechanism: Cookie-based authentication
Cookie: GoodMusicAuth
Contains: User identity claims (UserId, Email, Roles, etc.) + the WebAPI JWT token stored in authentication properties
Used for: Authorizing access to Razor Pages, determining who the user is in the web app

2. WebAPI JWT Token (AppWebApi)

Purpose: Authenticates requests to the WebAPI
Mechanism: Bearer token authentication
Transport: HTTP Authorization header (Bearer <token>)
Contains: User role and claims for database access (UserRole, UserId, UserName)
Used for: API authorization, selecting the correct database connection based on role

3. The Flow
User Login
    ↓
Identity validates credentials → Creates Identity cookie with user claims
    ↓
Login to WebAPI (dbo1) → Get JWT token
    ↓
Store JWT token in Identity cookie's AuthenticationProperties
    ↓
Cookie saved to browser: {
    Identity Claims: { UserId, Email, IsAuthenticated... }
    Stored Tokens: { access_token: "JWT..." }
}
    ↓
User navigates to MusicGroups page
    ↓
Identity cookie authenticates the request (User.Identity.IsAuthenticated = true)
    ↓
MusicGroupsServiceWapi needs data from WebAPI
    ↓
JwtTokenHandler intercepts HTTP request
    ↓
Retrieves JWT token from cookie: GetTokenAsync("access_token")
    ↓
Adds to request: Authorization: Bearer <JWT>
    ↓
WebAPI receives request, validates JWT token
    ↓
WebAPI DbContextExtensions extracts UserRole from JWT → Selects database connection
    ↓
WebAPI Data returned to MusicGroupsServiceWapi
    ↓
Data presented at Razor Page
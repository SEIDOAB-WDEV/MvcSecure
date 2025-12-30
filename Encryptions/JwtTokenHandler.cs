using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Encryption;

// DelegatingHandler that automatically adds JWT Bearer tokens to outgoing HTTP requests
// - **Purpose**: Intercepts all HTTP requests to WebAPI and adds Authorization header
// - **Token Retrieval**: Gets token from Identity authentication cookie using `GetTokenAsync()`
// - **Automatic Injection**: Adds `Authorization: Bearer <token>` header to requests
// - **Logging**: Provides detailed logging for debugging token injection
// - **Focused Responsibility**: Handles only HTTP message interception and token injection

public class JwtTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<JwtTokenHandler> _logger;

    public JwtTokenHandler(IHttpContextAccessor httpContextAccessor, ILogger<JwtTokenHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// HTTP message handler that automatically adds JWT token from authentication session to outgoing requests
    /// </summary>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Retrieve the token from the authentication session
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            // Use IdentityConstants.ApplicationScheme to retrieve the token stored by SignInManager
            var token = await httpContext.GetTokenAsync(IdentityConstants.ApplicationScheme, "access_token");
            if (!string.IsNullOrEmpty(token))
            {
                // Add the token as a Bearer token to the Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation($"Token added to request: {request.RequestUri}");
            }
            else
            {
                _logger.LogWarning($"No access_token found for request: {request.RequestUri}. User authenticated: {httpContext.User?.Identity?.IsAuthenticated}");
            }
        }
        else
        {
            _logger.LogWarning($"HttpContext is null for request: {request.RequestUri}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

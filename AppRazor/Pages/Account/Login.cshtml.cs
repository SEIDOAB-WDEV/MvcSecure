using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.Authorization;
using AppRazor.SeidoHelpers;
using static AppRazor.Pages.RegisterModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Models.DTO;
using Services;
using Encryption;

namespace AppRazor.Pages.Account
{
	public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ILoginService _wapiLoginService;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, ILoginService wapiLoginService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _wapiLoginService = wapiLoginService;
        }

        [BindProperty]
        public LoginIM LoginCreds { get; set; }

        //For Validation and Identity Errors
        public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);

        //public string ReturnUrl { get; set; }

        public class LoginIM
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGetAsync()
        {
            LoginCreds = new LoginIM();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var returnUrl = Url.Content("~/");

            if (!ModelState.IsValidPartially(out ModelValidationResult validationResult))
            {
                ValidationResult = validationResult;
                return Page();
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(LoginCreds.Email, LoginCreds.Password, LoginCreds.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Retrieve user
                var user = await _signInManager.UserManager.FindByEmailAsync(LoginCreds.Email);
                _logger.LogInformation("User {Email} logged in successfully", user.Email);

                // Authenticate with WebAPI to obtain JWT token
                var wapiResponse = await _wapiLoginService.LoginUserAsync(new LoginCredentialsDto
                {
                    UserNameOrEmail = "dbo1",
                    UserPassword = "dbo1"
                });
                _logger.LogInformation("WebAPI authentication succeeded for user {Email}", user.Email);

                // Store the JWT token in the authentication properties
                await JwtTokenStorage.StoreTokenAsync(
                    _signInManager,
                    user,
                    LoginCreds.RememberMe,
                    wapiResponse.Item.JwtToken);
                _logger.LogInformation("WebAPI token stored for user {Email}", user.Email);

                _logger.LogInformation("Login succeeded");
                return LocalRedirect("/");
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = LoginCreds.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            //Failed Login
            ValidationResult = new ModelValidationResult(true, new List<string>() { "Invalid login attempt." }, null) ;
            return Page();
        }
    }
}

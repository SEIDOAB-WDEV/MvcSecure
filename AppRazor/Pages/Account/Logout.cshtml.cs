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
using static AppRazor.Pages.Account.LoginModel;

namespace AppRazor.Pages.Account
{
	public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var returnUrl = Url.Content("~/");

            await _signInManager.SignOutAsync();
            return LocalRedirect(returnUrl);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

using AppMvc.SeidoHelpers;
using Models.Authorization;

namespace AppMvc.Models
{
	public class RegisterViewModel
    {        
        [BindProperty]
        public UserIM RegUser { get; set; }

        //For Validation and Identity Errors
        public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);
    
        #region Input Model
        //InputModel (IM) is locally declared classes that contains ONLY the properties of the Model
        //that are bound to the <form> tag
        //EVERY property must be bound to an <input> tag in the <form>
        //These classes are in center of ModelBinding and Validation
        public class UserIM
        {
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
            [DataType(DataType.Password)]

            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //The Basic IM methods
            public User UpdateModel(User model)
            {
                model.FirstName = this.FirstName;
                model.LastName = this.LastName;
                model.Email = this.Email;
                return model;
            }

            public UserIM() { }
            public UserIM(UserIM original)
            {
                FirstName = original.FirstName;
                LastName = original.LastName;
                Email = original.Email;
            }
            public UserIM(User model)
            {
                FirstName = model.FirstName;
                LastName = model.LastName;
                Email = model.Email;
            }
        }
        #endregion

    }
}

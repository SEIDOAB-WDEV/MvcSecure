using AppMvc.SeidoHelpers;

namespace AppMvc.Models
{
	public class ConfirmEmailViewModel
    {        
        public ModelValidationResult ValidationResult { get; set; } = new ModelValidationResult(false, null, null);
    }
}

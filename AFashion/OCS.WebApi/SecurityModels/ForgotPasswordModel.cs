using System.ComponentModel.DataAnnotations;

namespace OCS.WebApi.SecurityModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using PojistovnaFullAspPrzeczek.Resources;

namespace PojistovnaFullAspPrzeczek.ViewModels
{
    public class LoginViewModel
    {

        [Required(
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = "Required")]
        [EmailAddress(
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; } = "";

        [Required(
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = "PasswordLength")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = "";

        [Display(Name = "RememberMe", ResourceType=typeof(SharedResources))]
        public bool RememberMe { get; set; } = false;
    }
}

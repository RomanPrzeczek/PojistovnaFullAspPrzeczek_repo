using System.ComponentModel.DataAnnotations;
using PojistovnaFullAspPrzeczek.Resources;

namespace PojistovnaFullAspPrzeczek.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Name", ResourceType = typeof(SharedResources))]
        public string Name { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Surname", ResourceType = typeof(SharedResources))]
        public string Surname { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "City", ResourceType = typeof(SharedResources))]
        public string City { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Street", ResourceType = typeof(SharedResources))]
        public string Street { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Range(10000, 99999, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "ZIPInvalid")]
        [Display(Name = "ZIP", ResourceType = typeof(SharedResources))]
        public int ZIP { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Phone(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "PhoneInvalid")]
        [Display(Name = "Phone", ResourceType = typeof(SharedResources))]
        public string Phone { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(SharedResources))]
        public string Password { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        [Display(Name = "RepeatPassword", ResourceType = typeof(SharedResources))]
        public string ConfirmPassword { get; set; } = "";
    }
}

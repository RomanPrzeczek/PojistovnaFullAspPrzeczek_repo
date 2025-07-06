using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.InsuredPersons
{
    public class InsuredPersonEditDto
    {
        [Required]
        public int IdInsuredPerson { get; set; } // Důležité pro update

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Name", ResourceType = typeof(SharedResources))]
        public string Name { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Surname", ResourceType = typeof(SharedResources))]
        public string Surname { get; set; } = "";

        [Display(Name = "City", ResourceType = typeof(SharedResources))]
        public string City { get; set; } = "";

        [Display(Name = "Street", ResourceType = typeof(SharedResources))]
        public string Street { get; set; } = "";

        [Range(10000, 99999, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "ZIPInvalid")]
        [Display(Name = "ZIP", ResourceType = typeof(SharedResources))]
        public int ZIP { get; set; }

        [Phone(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "PhoneInvalid")]
        [Display(Name = "Phone", ResourceType = typeof(SharedResources))]
        public string Phone { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "InvalidEmail")]
        [StringLength(100, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "TooLongEmail")]
        [Display(Name = "Email", ResourceType = typeof(SharedResources))]
        public string Email { get; set; } = "";

        public override string ToString()
        {
            return $"Zdroj: InsuredPersonEditDto";
        }

    }
}

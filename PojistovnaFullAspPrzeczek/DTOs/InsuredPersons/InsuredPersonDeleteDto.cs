using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.InsuredPersons
{
    public class InsuredPersonDeleteDto
    {
        public int IdInsuredPerson { get; set; }

        [Display(Name = "Name", ResourceType = typeof(SharedResources))]
        public string Name { get; set; } = "";

        [Display(Name = "Surname", ResourceType = typeof(SharedResources))]
        public string Surname { get; set; } = "";

        [Display(Name = "City", ResourceType = typeof(SharedResources))]
        public string City { get; set; } = "";

        [Display(Name = "Street", ResourceType = typeof(SharedResources))]
        public string Street { get; set; } = "";

        [Display(Name = "ZIP", ResourceType = typeof(SharedResources))]
        public int ZIP { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(SharedResources))]
        public string Phone { get; set; } = "";

        [Display(Name = "Email", ResourceType = typeof(SharedResources))]
        public string Email { get; set; } = "";
    }
}

using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;
using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.InsuredPersons
{
    public class InsuredPersonDetailDto
    {
        public int IdInsuredPerson { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(SharedResources))]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "City", ResourceType = typeof(SharedResources))]
        public string City { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone", ResourceType = typeof(SharedResources))]
        public string Phone{ get; set; } = string.Empty;

        public List<PersonInsuranceDto> PersonInsurances { get; set; } = new();

        /// <summary>
        /// For debugging only.
        /// In related view returns DTO spec output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DetailDTO: {FullName}, {Email}";
        }
    }
}

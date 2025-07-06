using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.InsuredPersons
{
    public class InsuredPersonDto
    {
        public int IdInsuredPerson { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(SharedResources))]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "City", ResourceType = typeof(SharedResources))]
        public string City { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// For debugging only.
        /// In related view returns DTO spec output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DTO: {FullName}, {Email}";
        }
    }
}

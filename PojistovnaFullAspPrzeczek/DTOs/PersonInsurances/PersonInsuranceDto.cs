using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.PersonInsurances
{
    public class PersonInsuranceDto
    {
        public int IdPersonInsurance { get; set; }

        public int IdInsuredPerson { get; set; }

        public int IdInsuranceForPerson { get; set; }

        [Display(Name = "InsuranceSubject", ResourceType = typeof(SharedResources))]
        public string SubjectOfInsurance { get; set; } = string.Empty;

        [Display(Name = "InsurancePrice", ResourceType = typeof(SharedResources))]
        public decimal InsurancePrice { get; set; }

        [Display(Name = "InsuranceValidFrom", ResourceType = typeof(SharedResources))]
        public DateTime StartDate { get; set; }

        [Display(Name = "InsuranceValidTo", ResourceType = typeof(SharedResources))]
        public DateTime EndDate { get; set; }

        public InsuranceDto? Insurance { get; set; }
    }
}

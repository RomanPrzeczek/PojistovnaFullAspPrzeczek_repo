using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.PersonInsurances
{
    public class PersonInsuranceCreateDto
    {
        [Required]
        public int IdInsuredPerson { get; set; }

        public int IdPersonInsurance { get; set; }

        [Required(ErrorMessage = "Pojištění musí být vybráno.")]
        public int IdInsuranceForPerson { get; set; }

        [Required(ErrorMessage = "Předmět pojištění je povinný.")]
        [StringLength(100)]
        [Display(Name = "InsuranceSubject", ResourceType = typeof(SharedResources))]
        public string SubjectOfInsurance { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena pojištění je povinná.")]
        [Range(0.01, 1000000, ErrorMessage = "Cena musí být větší než 0.")]
        [Display(Name = "InsurancePrice", ResourceType = typeof(SharedResources))]
        public decimal InsurancePrice { get; set; }

        [Required(ErrorMessage = "Datum začátku platnosti je povinné.")]
        [DataType(DataType.Date)]
        [Display(Name = "InsuranceValidFrom", ResourceType = typeof(SharedResources))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Datum konce platnosti je povinné.")]
        [DataType(DataType.Date)]
        [Display(Name = "InsuranceValidTo", ResourceType = typeof(SharedResources))]
        public DateTime EndDate { get; set; }
    }
}

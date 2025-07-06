using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace PojistovnaFullAspPrzeczek.Models
{
    public class PersonInsurance
    {
        [Key]
        public int IdPersonInsurance { get; set; }

        [Required(ErrorMessage = "Cena pojištění je povinná.")]
        [Column(TypeName ="decimal(18,2)")]
        public decimal InsurancePrice { get; set; }
        public string SubjectOfInsurance { get; set; } = "";

        [DataType(DataType.Date)]
        [Display(Name = "Datum začátku")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum ukončení")]
        public DateTime EndDate { get; set; }

        // Cizí klíč na výběr pojištění
        [Display(Name = "Pojištění")]
        public int IdInsuranceForPerson { get; set; }

        // Navigační vlastnost výběru pojištění
        [ForeignKey("IdInsuranceForPerson")]
        public Insurance? Insurance { get; set; }

        // Cizí klíč na pojištěnce
        [Display(Name = "Pojištěnec")]
        public int IdInsuredPerson { get; set; }

        [ForeignKey("IdInsuredPerson")]
        public InsuredPerson? InsuredPerson { get; set; }

        /// For debugging only.
        /// In related view returns DTO spec output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"PersonInsuranceModel";
        }
    }
}

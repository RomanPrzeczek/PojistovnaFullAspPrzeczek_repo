using Microsoft.AspNetCore.Mvc.Rendering;
using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;


namespace PojistovnaFullAspPrzeczek.DTOs.InsuredPersons
{
    public class InsuredPersonChangeInsuranceDto
    {
        public int IdInsuredPerson { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(SharedResources))]
        public string FullName { get; set; } = "";
        public List<SelectListItem> PersonInsurances { get; set; } = new();

        public override string ToString()
        {
            return $"Zdroj dat: InsuredPersonChangeInsuranceDto";
        }
    }
}

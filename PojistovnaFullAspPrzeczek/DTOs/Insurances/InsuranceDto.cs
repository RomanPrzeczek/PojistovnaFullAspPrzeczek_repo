using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PojistovnaFullAspPrzeczek.Resources;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.DTOs.Insurances
{
    public class InsuranceDto
    {
        public int IdInsurance { get; set; }

        [Display(Name = "InsuranceTitle", ResourceType = typeof(SharedResources))]
        public string Title { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"DTO";
        }
    }
}

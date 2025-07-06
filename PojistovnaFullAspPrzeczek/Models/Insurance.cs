using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PojistovnaFullAspPrzeczek.Models
{
    public class Insurance
    {
        [Key]
        public int IdInsurance { get; set; }
        public string Title { get; set; } = "";
        [ForeignKey("InsuredPerson")]
        public int? IdInsuredPerson { get; set; }
        // Navigační vlastnost
        public InsuredPerson? InsuredPerson { get; set; }

        public override string ToString()
        {
            return $"DTO";
        }
    }
}

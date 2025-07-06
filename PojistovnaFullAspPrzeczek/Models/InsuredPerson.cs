using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PojistovnaFullAspPrzeczek.Models
{
    public class InsuredPerson
    {
        [Key]
        public int IdInsuredPerson { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public int ZIP { get; set; }
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public ICollection<PersonInsurance> PersonInsurances { get; set; } = new List<PersonInsurance>();
    }
}

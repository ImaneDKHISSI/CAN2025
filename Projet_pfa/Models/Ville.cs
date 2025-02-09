using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Ville")]
    public class Ville
    {
        public int Id { get; set; }
        public String Nom { get; set; }
        public IList<Stade>Stades { get; set; }
    }
}

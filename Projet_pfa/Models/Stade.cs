using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Stade")]
    public class Stade
    {
        public int Id { get; set; }
        public String Nom { get; set; } 
        public  int nbSiegeCat1 {  get; set; }
        public int nbSiegeCat2 { get; set; }
        public int nbSiegeCat3 { get; set; }
        public string Localisation { get; set; }
        public IList<Match>Matches { get; set; }
        public Ville Ville { get; set; }
        public int VilleId { get; set; }
    }
}

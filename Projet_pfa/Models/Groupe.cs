using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    public enum GroupeEnum
    {
          A,
          B,
          C,
          D,
          E,
          F
    }
    [Table("Groupe")]
    public class Groupe
    {
        
        public int Id { get; set; }
        //public string Libelle { get; set; }
        public IList<Equipe> Equipes { get; set; }
        public GroupeEnum Nom { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Nationalite")]
    public class Nationalite
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public IList<Utilisateur> Utilisateurs { get; set; }
    }
}

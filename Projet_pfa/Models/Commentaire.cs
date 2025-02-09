using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Commentaire")]
    public class Commentaire
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Publication Publication { get; set; }
        public int PublicationId { get; set; }
        public Personne Personne { get; set; }
        public int PersonneId { get; set; }

    }
}

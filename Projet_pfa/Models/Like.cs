using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Like")]
    public class Like
    {
        public int Id { get; set; }
        public Personne Personne { get; set; }
        public int PersonneId { get; set; }
        public Publication Publication { get; set; }
        public int PublicationId { get; set; }
    }
}

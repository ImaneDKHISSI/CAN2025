using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net.Sockets;

namespace Projet_pfa.Models
{
    
    //[Table("Utilisateur")]
    public class Utilisateur:Personne
    {
        
        public Nationalite? Nationalite { get; set; }
        public int? NationaliteId { get; set; }
        public IList<Publication>? publications { get; set; }
        public IList<Ticket>? Tickets { get; set; }
        public IList<Commentaire>?Commentaires { get; set;}
        public IList<Like>? Likes { get; set; }

    }
}

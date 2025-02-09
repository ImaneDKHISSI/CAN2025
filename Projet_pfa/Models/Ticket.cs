using Projet_pfa.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    [Table("Ticket")]

    public class Ticket
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }
       
        public string NomCompletBeneficiaire { get; set; }

        public Categorie CategorieSiege { get; set; }
        public Utilisateur Utilisateur { get; set; }
        public int UtilisateurId { get; set; }

        public Match Match { get; set; }
        public int MatchId { get; set; }

        public Paiement Paiement { get; set; }
        public int PaiementId { get; set; }
    }
}

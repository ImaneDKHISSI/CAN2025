using Projet_pfa.Models;
using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public enum Categorie
    {
        A,
        B, 
        C
    };
   
    public class MatchInformations
    {
        //possible que cette property soit une enumeration ! 
        [Display(Name = "Catégorie de Siège")]
        [Required(ErrorMessage ="Veuillez choisir une categorie")]
        public Categorie CategorieSiege { get; set; }

        [Display(Name ="Nombre de Ticket")]
        [Required(ErrorMessage ="Veuillez choisir un nombre de tickets")]
        public int NbrTicket{ get; set;}

        public Equipe? equipe1 { get; set; }
        public Equipe? equipe2{ get; set; }

        public string? date { get; set; }

        public Stade? stade { get; set; }

        public double? PrixSiegeCat1 { get; set; }
        public double? PrixSiegeCat2 { get; set; }
        public double? PrixSiegeCat3 { get; set; }

    }
}

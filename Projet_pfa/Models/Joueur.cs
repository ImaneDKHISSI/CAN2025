using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
    
    public enum PosteEnum
    {
        Gardien,
        Défenseur,
        Milieu,
        Attaquant
    }
    [Table("Joueur")]
    public class Joueur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int NumeroMaillot { get; set; }
        public DateTime DateNaissance { get; set; }
        //public byte[] Photo {  get; set; }
        public string Photo { get; set; }
        public PosteEnum? Role { get; set; }
        public Equipe Equipe { get; set; }

        public int EquipeId { get; set; }
    }
}

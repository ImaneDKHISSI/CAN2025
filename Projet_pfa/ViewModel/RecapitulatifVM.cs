using Projet_pfa.Models;

namespace Projet_pfa.ViewModel
{
    public class RecapitulatifVM
    {
        public Categorie categorie {  get; set; }
        public double prixCategorie {  get; set; }

        public List<string> nomcomplets { get; set; }
        public Equipe Equipe1 { get; set; }
        public Equipe Equipe2 { get; set; }
        public string Date { get; set; }
        public Stade stade { get; set; }
        public double? Taxe { get; set; }
        public int nbrTicket { get; set; }

        
    }
}

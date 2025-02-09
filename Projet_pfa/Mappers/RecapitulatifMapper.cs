using Projet_pfa.ViewModel;
using Projet_pfa.Models;

namespace Projet_pfa.Mappers
{
    public class RecapitulatifMapper
    {
        public static RecapitulatifVM ToRecapitulatif(Match m ,Categorie categ, double prix ,int nbrticket,List<string> nomcomplets)
        {
            RecapitulatifVM recapitulatifVM;
           
                recapitulatifVM = new RecapitulatifVM
                {
                    nomcomplets=nomcomplets,
                    Equipe1 = m.Equipe1,
                    Equipe2 = m.Equipe2,
                    Date = m.Date.ToString("dddd dd MMMM yyyy").ToUpper(),
                    stade =  m.Stade,
                    categorie = categ,
                    nbrTicket = nbrticket,
                    prixCategorie = prix
                };
            
            
            return recapitulatifVM;
        }
    }
}

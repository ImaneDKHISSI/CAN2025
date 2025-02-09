using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mappers
{
    public class TicketMapper
    {
        public static Ticket ToTicket(DateTime dateAchat,int idMatch,List<string> nomcomplets,int i,int UtilisateurId,int PaiementId,Categorie categorie)
        {
            Ticket ticket = new Ticket()
            {
                DateAchat = dateAchat,
                MatchId = idMatch,
                UtilisateurId = UtilisateurId,
                NomCompletBeneficiaire = nomcomplets[i].ToString(),
                PaiementId = PaiementId,
                CategorieSiege = categorie

            };
            return ticket;
        }
    }
}

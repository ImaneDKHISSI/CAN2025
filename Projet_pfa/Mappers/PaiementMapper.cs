using Projet_pfa.Models;

namespace Projet_pfa.Mappers
{
    public class PaiementMapper
    {
        public static Paiement ToPaiement(double prix, int nbrTickets, string transactionId, DateTime datePaiment)
        {
            Paiement paiement = new Paiement()
            {
                DatePaiement = datePaiment,
                Total=prix*nbrTickets,
                transactionId = transactionId,
            };
            return paiement;
        }
    }
}

using Projet_pfa.Models;

namespace Projet_pfa.Services
{
    public class PaiementService
    {
        MyContext db;
        public PaiementService(MyContext db)
        {
            this.db = db;
        }
        public void AddPayment(Paiement paiement)
        {
            db.Paiements.Add(paiement);
        }
    }
}

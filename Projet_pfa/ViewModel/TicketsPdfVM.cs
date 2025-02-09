using Projet_pfa.Models;

namespace Projet_pfa.ViewModel
{
    public class TicketsPdfVM
    {
        public List<Ticket> BuyedTickets { get; set; }
        
        public List<string> QrcodeImg { get; set; }

    }
}

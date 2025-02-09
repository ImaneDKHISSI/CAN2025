using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mappers
{
    public class ToTicketAsPdf
    {
        public static TicketsPdfVM ToTicketPdfVM(List<Ticket> tickets ,List<string> Qrcode)
        {

            TicketsPdfVM vm = new TicketsPdfVM()
            {
                BuyedTickets = tickets,
                QrcodeImg = Qrcode
            };
            return vm;
        }
    }
}

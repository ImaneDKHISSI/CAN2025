using Projet_pfa.Models;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Projet_pfa.ViewModel;
using Microsoft.EntityFrameworkCore;
using Projet_pfa.Exceptions;

namespace Projet_pfa.Services
{
    
    public class TicketService
    {
        MyContext db;
        public TicketService(MyContext db)
        {

            this.db = db;

        }
        public  List<string> GenerateQrCodeForTickets(List<Ticket> Tickets)
        {
            List<string> qrCodeImages = new List<string>();

            foreach (Ticket ticket in Tickets)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(ticket.Id.ToString(), QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    using (Bitmap bitmap = qrCode.GetGraphic(20))
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        string qrCodeBase64 = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        qrCodeImages.Add(qrCodeBase64);
                    }
                }
            }

            return qrCodeImages;
        }
    
        public double GetPriceFromCategory(Categorie categorie,Match match)
        {
            if (categorie == Categorie.A)
            {
                return match.PrixSiegeCat1;
            }
            else if (categorie == Categorie.B)
            {
                 return match.PrixSiegeCat2;
            }
            else
            {
                return match.PrixSiegeCat1;
            }
        }
    
        
        public void AddTicket(Ticket ticket)
        {
            db.Tickets.Add(ticket);
            
        }

        public Match FindMatch(int IdMatch)
        {
            Match matchSelected = db.Matches.Include(m => m.Stade.Ville).Include(m => m.Equipe1).Include(m => m.Equipe2).AsTracking().FirstOrDefault(m => m.Id == IdMatch);
            return matchSelected;
        }
        public void IncrementerNbrTicketReserve(Match matchSelected,int nbrTickets,double Prix)
        {


            if (Prix == matchSelected.PrixSiegeCat1)
            {
                 
                 matchSelected.nbrsiegeReserveCat1 += nbrTickets;
            }
            else if (Prix == matchSelected.PrixSiegeCat2)
            {
                matchSelected.nbrsiegeReserveCat2 += nbrTickets;
            }
            else
            {
                matchSelected.nbrsiegeReserveCat3 += nbrTickets;
            }
        }

        public int GetNbrTicketReserved(Match matchSelected, double Prix)
        {

            if (Prix == matchSelected.PrixSiegeCat1)
            {
                return matchSelected.nbrsiegeReserveCat1;
            }
            else if (Prix == matchSelected.PrixSiegeCat2)
            {
                return matchSelected.nbrsiegeReserveCat2;
            }
            else
            {
                return matchSelected.nbrsiegeReserveCat3;
            }

        }
        public int GetNbrAvailableTicket(Match matchSelected, double Prix)
        {
           
            if(Prix == matchSelected.PrixSiegeCat1)
            {
                return matchSelected.Stade.nbSiegeCat1;
            }else if (Prix == matchSelected.PrixSiegeCat2)
            {
                return matchSelected.Stade.nbSiegeCat2;
            }
            else
            {
                return matchSelected.Stade.nbSiegeCat3;
            }
           
        }
    
        public bool TicketStillAvailable(Match Match, double Prix,int nbrTickets)
        {
            // recuperation de nombre de tickets available
            int nbrAvailableTickets = GetNbrAvailableTicket(Match, Prix);
            int nbrTicketReserve = GetNbrTicketReserved(Match, Prix);
                if (nbrAvailableTickets >= (nbrTickets + nbrTicketReserve))
                {
                    return true;
                }
                else
                {
                    throw new TicketAvailabilityException("An error occurred while checking ticket availability.");
                }
           
        }
           
        
    }
}

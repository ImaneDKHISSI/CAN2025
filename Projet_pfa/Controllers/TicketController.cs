using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_pfa.Mappers;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Checkout;
using System.Text.Json;
using Projet_pfa.Services;
using Projet_pfa.Filters;
using Projet_pfa.Exceptions;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Projet_pfa.Controllers
{   
    [AuthentificationFilter("User")]
    [MaxTicket]
    public class TicketController : Controller
    {
       
        private MyContext db;
        private TicketService ticketService;
        private PaiementService paiementService;
        private readonly ILogger<TicketController> _logger;
        private readonly IRazorViewEngine _viewRenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompositeViewEngine _viewEngine;

        public TicketController(
            MyContext db,
            TicketService ticketService,
            PaiementService paiementService,
            ILogger<TicketController> logger,
            ICompositeViewEngine viewEngine)
        {
            this.db = db;
            this.ticketService = ticketService;
            this.paiementService= paiementService;
            _logger = logger;
            _viewEngine = viewEngine;
            //_viewRenderService = viewRenderService;
            //_httpContextAccessor = httpContextAccessor;
        }

        
        public IActionResult MatchInfo(int id)
        {
            Match match = ticketService.FindMatch(id);
            if (match != null)
            {
                int nbrTicketMadeByUser = int.Parse(HttpContext.Session.GetString("NbrTicketMadeByUser"));
                ViewBag.nbrTicketMadeByUser = nbrTicketMadeByUser;
                HttpContext.Session.SetInt32("matchChoisiId",id);
                MatchInformations matchinfor = MatchInformationMapper.GetMatchInfoFromMatch(match);
                return View(matchinfor);
            }

            return View();
        }

        [HttpPost]
        public IActionResult MatchInfo(MatchInformations model) 
        {
            Int32 MatchId = (int)HttpContext.Session.GetInt32("matchChoisiId");
            Match match = ticketService.FindMatch(MatchId);
            if (!ModelState.IsValid)
            {
                MatchInformations matchinfor = MatchInformationMapper.GetMatchInfoFromMatch(match);
                return View(matchinfor);
            }
            HttpContext.Session.SetString("nbrtickets", model.NbrTicket.ToString());
            HttpContext.Session.SetString("categorie", model.CategorieSiege.ToString());

            double prix =ticketService.GetPriceFromCategory(model.CategorieSiege,match);
            HttpContext.Session.SetString("prix", prix.ToString());

            return RedirectToAction(nameof(ticketInfo));
        }
        
        public IActionResult ticketInfo()
        {
            ViewBag.Erreur = false;
            string str_nbrTickets = HttpContext.Session.GetString("nbrtickets");
            ViewBag.nbrtickets =int.Parse(str_nbrTickets);
            return View();
        }
        [HttpPost]
        public IActionResult ticketInfo(TicketInformation model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.NomComplets[0] != null)
                {
                    ViewBag.Erreur = false;
                    List<string> nomcomplets = model.NomComplets;
                    string jsonResultat = JsonSerializer.Serialize(nomcomplets);
                    HttpContext.Session.SetString("nomcomplets", jsonResultat);
                    //ViewBag.nomcompletsCount = nomcomplets.Count;
                    //ViewBag.nomcomplets = nomcomplets;
                    return RedirectToAction(nameof(Recapitulatif));
                }
                else
                {
                    
                    ViewBag.nbrtickets = int.Parse(HttpContext.Session.GetString("nbrtickets"));
                    ViewBag.Erreur = true;
                    return View();
                }
               
            }
            
            return View();
            
        }


        public IActionResult Recapitulatif()
        {   
            List<string> nomcomplets ;
            if (HttpContext.Session.GetString("nomcomplets") != null)
            {
                nomcomplets = JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("nomcomplets"));
            }
            else
            {
                nomcomplets = new List<string>();
            }
              

            int idMatch = (int)HttpContext.Session.GetInt32("matchChoisiId");
            Match match = db.Matches.Include(m => m.Stade.Ville).Include(m => m.Equipe1).Include(m => m.Equipe2).FirstOrDefault(m => m.Id == idMatch);

            Categorie categorie = Enum.Parse<Categorie>(HttpContext.Session.GetString("categorie"));
            int nbrTicket =int.Parse(HttpContext.Session.GetString("nbrtickets"));
            double prix = double.Parse(HttpContext.Session.GetString("prix"));

            RecapitulatifVM  recap= RecapitulatifMapper.ToRecapitulatif(match,categorie,prix,nbrTicket,nomcomplets);

            return View(recap);
        }

        public IActionResult Checkout() 
        {
           
            int idMatch = (int)HttpContext.Session.GetInt32("matchChoisiId");
            Match match = db.Matches.Include(m => m.Stade.Ville).Include(m => m.Equipe1).Include(m => m.Equipe2).FirstOrDefault(m => m.Id == idMatch);
            double prix = double.Parse(HttpContext.Session.GetString("prix"));
            int nbrTickets = int.Parse(HttpContext.Session.GetString("nbrtickets"));
            
            

            var domain = "https://localhost:44303/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                SuccessUrl = domain + $"Ticket/OrderConfirmation",
                CancelUrl = domain + "Ticket/Erreur",
                LineItems = new List<SessionLineItemOptions>(),
                Mode="payment"
            };

                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount =(long)(prix *100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = match.Equipe1.Nom + " Vs " + match.Equipe2.Nom,
                            Description = "Ticket Pour le Match du " + match.Equipe1.Nom + " Contre " + match.Equipe2.Nom,
                            
                        },
                        
                        
                    },
                    Quantity = nbrTickets,
                    
                    
                    //Price = HttpContext.Session.GetString("prix")
                };
                

            options.LineItems.Add(sessionListItem);
            var service = new SessionService();
            Session session = service.Create(options);
            
            TempData["TransactionId"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["TransactionId"].ToString());
            if(session.PaymentStatus == "paid")
            {
                //récuperation des informations nécessaire pour l'ajout d'un payement
                double prix = double.Parse(HttpContext.Session.GetString("prix"));
                int nbrTickets = int.Parse(HttpContext.Session.GetString("nbrtickets"));
                string transactionId = session.PaymentIntentId.ToString();
                DateTime datePaiment = DateTime.Now;
                

                // l'ajout d'un paiement avec les informations précedentes
                Paiement paiement = PaiementMapper.ToPaiement(prix, nbrTickets, transactionId, datePaiment);
                paiementService.AddPayment(paiement);
                db.SaveChanges();

                //Paiement RecentPaiement = db.Paiements.FirstOrDefault(p=>p.transactionId == transactionId);
                //HttpContext.Session.SetString("PaiementId", RecentPaiement.Id.ToString());
                TempData["PaiementId"] = paiement.Id;
                //Redirection vers la page de success

                return RedirectToAction(nameof(Success));
                
             }
            return View();
        }
        public IActionResult Success()
        {
            try
            {
                int nbrTickets = int.Parse(HttpContext.Session.GetString("nbrtickets"));
                int idMatch = (int)HttpContext.Session.GetInt32("matchChoisiId");
                double prix = double.Parse(HttpContext.Session.GetString("prix"));
                Categorie categorie = Enum.Parse<Categorie>(HttpContext.Session.GetString("categorie"));
                Match match = ticketService.FindMatch(idMatch);
                if (match != null)
                {
                    if (ticketService.TicketStillAvailable(match, prix, nbrTickets))
                    {
                        List<string> nomcomplets;
                        if (HttpContext.Session.GetString("nomcomplets") != null)
                        {
                            nomcomplets = JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("nomcomplets"));
                        }
                        else
                        {
                            return RedirectToAction(nameof(MatchInfo));
                        }

                        DateTime dateAchat = DateTime.Now;
                        int UtilisateurId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
                        int paiementId =(int) TempData["PaiementId"];
                        
                        //Ajout de chaque Ticket dans la bd 
                        for (int i = 0; i < nbrTickets; i++)
                        {
                            Ticket ticket = TicketMapper.ToTicket(dateAchat, idMatch, nomcomplets, i, UtilisateurId,paiementId,categorie);
                            ticketService.AddTicket(ticket);
                            

                        }
                        ticketService.IncrementerNbrTicketReserve(match,nbrTickets,prix);
                        //db.Entry(match).Property(m => m.RowVersion).IsModified = true;
                        try
                        {
                            db.SaveChanges(); 
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                            TempData["ExConcurrency"] = ex.Message;
                            return View("ConcurrencyException");
                        }

                        int OldNbrTicketMadeByUser = int.Parse(HttpContext.Session.GetString("NbrTicketMadeByUser"));
                        int NewNbrTicketMadeByUser = OldNbrTicketMadeByUser + nbrTickets;
                        HttpContext.Session.SetString("NbrTicketMadeByUser", NewNbrTicketMadeByUser.ToString());

                        //Recherche des Ticket d'un utilisateur par son id stocké dans la session

                        List<Ticket> list = db.Tickets.Where(m => m.UtilisateurId == UtilisateurId).ToList();

                        List<string> qrImages = ticketService.GenerateQrCodeForTickets(list);
                        // enregistrement de qr code src pour l'afficher
                        List<string> qrcodes = qrImages;
                        string jsonRe = JsonSerializer.Serialize(qrcodes);
                        
                        HttpContext.Session.SetString("QrCode", jsonRe);
                        
                    }
                }
                else
                {
                    return RedirectToAction(nameof(MatchInfo));
                }
                

            }
            catch (TicketAvailabilityException e)
            {
                ModelState.AddModelError("", e.Message); // Display the specific error message
                TempData["ExTicketUnavailable"] = e.Message;
                return View("TicketUnavailable");

            }
            

            return View();

            
        }


        public IActionResult TicketUnavailable()
        {
            return View();
        }

        public IActionResult BuyLimitExceeded()
        {
            return View();
            
        }
    
        public IActionResult ConcurrencyException()
        {
            return View();
        }

        public IActionResult GenerateTickets()
        {
            TicketsPdfVM t = getTicketAspdf();
            return View(t);
        }
       

        public async Task<IActionResult> IndexPdf()
        {
            License.LicenseKey = "IRONSUITE.HABIBELMIR8.GMAIL.COM.6588-74A8DC7CE3-BWYRQMESROEDK3WB-HZSIBMH6KI3E-YN6LSEV44JD5-BQ6JRHHRE223-337Y72UUTXUJ-FWAK5ZREORO6-BUHBZC-TLLOJLM3EQKMUA-DEPLOYMENT.TRIAL-TTNGDV.TRIAL.EXPIRES.20.JUN.2024";

            using StringWriter sw = new StringWriter();
            IView view = _viewEngine.FindView(ControllerContext, "GenerateTickets", true).View ?? default!;
            TicketsPdfVM ticketPdfVm = getTicketAspdf();
            ViewData.Model = ticketPdfVm;
            ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());
            await view.RenderAsync(viewContext);
            var content = sw.GetStringBuilder().ToString();
            
            ChromePdfRenderer renderer = new ChromePdfRenderer();

            IronPdf.PdfDocument pdf = renderer.RenderHtmlAsPdf(content);
            
            return File(pdf.Stream, "application/pdf", "Tickets"+ ticketPdfVm.BuyedTickets.Count()+ ".pdf");


        }
        public TicketsPdfVM getTicketAspdf()
        {
            TicketsPdfVM ticketsPdf;
            int UtilisateurId;
            int matchId;
            if (HttpContext.Session.GetString("IdUtilisateur") != null && HttpContext.Session.GetInt32("matchChoisiId") != null)
            {
                 UtilisateurId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
                 matchId = (int)HttpContext.Session.GetInt32("matchChoisiId");
            }
            else
            {
                UtilisateurId = 0;
                matchId = 0;
            }
            
            List<Ticket> listTicketsMadeByUser = db.Tickets
                .Include(t => t.Match.Stade)
                .Include(t => t.Match.Equipe1)
                .Include(t => t.Match.Equipe2)
                .Where(t => t.UtilisateurId == UtilisateurId && t.MatchId==matchId)
                .ToList();

            if (listTicketsMadeByUser.Count != 0)
            {
                string jsonR = HttpContext.Session.GetString("QrCode") ?? "";
                if (jsonR != null)
                {
                    List<string> qrcodes = JsonSerializer.Deserialize<List<string>>(jsonR);
                    ticketsPdf = ToTicketAsPdf.ToTicketPdfVM(listTicketsMadeByUser, qrcodes);
                }
                else
                {
                    ticketsPdf = null;
                }
                
            }
            else
            {
                ticketsPdf = null;
               
            }
            return ticketsPdf;
        }

    }
}

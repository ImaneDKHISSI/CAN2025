using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projet_pfa.Filters;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;


namespace Projet_pfa.Controllers
{
    public class MatchController : Controller
    {
        private readonly MyContext _context;

        public MatchController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                ViewBag.IsAdmin = true;
            }
            var matches = _context.Matches.Include(m => m.Equipe1).Include(m => m.Equipe2)
                                  .OrderByDescending(m => m.Date)
                                  .ToList();
            ViewBag.Matches = matches;
            var stades = _context.Stades.Include(m => m.Ville)
                                  .ToList();
            ViewBag.Stades = stades;
            return View();
        }
        public IActionResult FiltrerMatchs(DateDebutDateFin viewModel)
        {
            //if(!ModelState.IsValid)
            //{

            //}
            var matchsFiltres = _context.Matches
                .Where(m => (!viewModel.DateDebut.HasValue || m.Date >= viewModel.DateDebut) &&
                            (!viewModel.DateFin.HasValue || m.Date <= viewModel.DateFin) &&
                            (!viewModel.IdStade.HasValue || m.StadeId == viewModel.IdStade))
                .Include(m => m.Equipe1).Include(m => m.Equipe2)
                .OrderByDescending(m => m.Date)
                .ToList();
            var stades = _context.Stades.Include(m => m.Ville)
                                  .ToList();
            ViewBag.Matches = matchsFiltres;
            ViewBag.Stades = stades;
            return View("Index");
        }

        [AuthentificationFilter("Admin")]
        public IActionResult GererMatchs()
        {
            var matches = _context.Matches
                            .Include(m => m.Equipe1)
                            .Include(m => m.Equipe2)
                            .Include(m => m.Stade)
                            .ToList();
            ViewBag.Matches = matches;
			var stades = _context.Stades.Include(m => m.Ville)
								  .ToList();
			ViewBag.Stades = stades;
			return View();
        }
        [AuthentificationFilter("Admin")]
        public IActionResult FormAjouter()
        {
            var matches = _context.Matches
                            .Include(m => m.Equipe1)
                            .Include(m => m.Equipe2)
                            .Include(m => m.Stade)
                            .ToList();
            ViewBag.Matches = matches;
            var stades = _context.Stades.Include(m => m.Ville)
                                  .ToList();
            ViewBag.Stades = stades;
            var equipes = _context.Equipes
                                  .ToList();
            ViewBag.Equipes = equipes;
            return View();
        }
        [AuthentificationFilter("Admin")]
        public IActionResult Ajouter(MatchViewModel viewModel)
        {
            
            Match match = new Match();
            match.Date = viewModel.Date;
            match.Equipe1Id = viewModel.IdEquipe1;
            match.Equipe2Id = viewModel.IdEquipe2;
            match.StadeId = viewModel.IdStade;
            match.PrixSiegeCat1 = viewModel.PrixSiegeCat1;
            match.PrixSiegeCat2 = viewModel.PrixSiegeCat2;
            match.PrixSiegeCat3 = viewModel.PrixSiegeCat3;
            
            _context.Matches.Add(match);
            try
            {
                _context.SaveChanges();
            }catch(DbUpdateException e)
            {
                TempData["SameEquipe"] = true;
                return RedirectToAction(nameof(FormAjouter));
            }
            
            return RedirectToAction("GererMatchs");
        }

        [AuthentificationFilter("Admin")]
        public IActionResult FormModifier(int id)
        {
            List<Equipe> equipes = _context.Equipes
                .Take(24)
                .ToList();
            Match match = _context.Matches
                .Include(e=>e.Equipe1)
                .Include(e=>e.Equipe2)
                .Include(e=>e.Stade)
                .FirstOrDefault(e=>e.Id == id);
            List<Stade> stades = _context.Stades.Include(m => m.Ville)
                                 .ToList();
            //var matches = _context.Matches
            //                .Include(m => m.Equipe1)
            //                .Include(m => m.Equipe2)
            //                .Include(m => m.Stade)
            //                .ToList();
            //ViewBag.Matches = matches;
            MatchViewModel vm = new MatchViewModel()
            {
                IdEquipe1 = match.Equipe1Id,
                IdEquipe2 = match.Equipe2Id,
                IdStade = match.StadeId,
                PrixSiegeCat1 = match.PrixSiegeCat1,
                Date = match.Date,
                PrixSiegeCat2 = match.PrixSiegeCat2,
                PrixSiegeCat3 = match.PrixSiegeCat3,
                Resultat = match.Resultat,
                equipes1 =equipes,
                equipes2=equipes,
                stades = stades,
                match= match
            };
           
            
            
            //ViewBag.Stades = stades;
            //ViewBag.Match = match;
            //ViewBag.Equipes = equipes;
            return View(vm);
        }
        [HttpPost]
        [AuthentificationFilter("Admin")]
        public IActionResult Modifier(int id, MatchViewModel viewModel)
        {
            Match match = _context.Matches.FirstOrDefault(e=>e.Id == id);

            match.Date = viewModel.Date;
            match.Equipe1Id = viewModel.IdEquipe1;
            match.Equipe2Id = viewModel.IdEquipe2;
            match.StadeId = viewModel.IdStade;
            match.PrixSiegeCat1 = viewModel.PrixSiegeCat1;
            match.PrixSiegeCat2 = viewModel.PrixSiegeCat2;
            match.PrixSiegeCat3 = viewModel.PrixSiegeCat3;
            match.Resultat = viewModel.Resultat;
            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                TempData["SameEquipe"] = true;
                return RedirectToAction(nameof(FormModifier),"Match",new {id = match.Id});
            }
           
            return RedirectToAction("GererMatchs");
        }
        [AuthentificationFilter("Admin")]
        public IActionResult Supprimer(int id)
        {
			Match match = _context.Matches.FirstOrDefault(e => e.Id == id);
            _context.Matches.Remove(match);
			_context.SaveChanges();
			return RedirectToAction("GererMatchs");
		}
       
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Projet_pfa.Mappers;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Controllers
{
    public class UtilisateursController : Controller
    {
        MyContext db;
        private readonly PasswordHasher<Utilisateur> passwordHasher;
        public UtilisateursController(MyContext db)
        {
            this.db = db;
            this.passwordHasher = new PasswordHasher<Utilisateur>();
        }
        //Formulaire d'authentification
        
        
        [HttpGet]
        [Route("/Authentification")]
        public IActionResult FormAuthentification()
        {
            return View();
        }
        
        
        
        [HttpPost]
        [Route("/Authentification")]
        public IActionResult FormAuthentification(FormulaireAuthentification model)
        {
            if (ModelState.IsValid)
            {
                
                Personne p= db.Personnes.FirstOrDefault(m => m.Login == model.NomUtilisateur);
                if(p != null)
                {
                   
                   
                        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, p.Password, model.MotDePasse);
                        if (passwordVerificationResult == PasswordVerificationResult.Success)
                        {
                            if (p is Administrateur)
                            {
                                HttpContext.Session.SetString("role", "Admin");
                                
                            }
                            else if (p is Utilisateur utilisateur)
                            {
                                int NbrTicketMadeByUser = db.Tickets.Where(m => m.UtilisateurId == utilisateur.Id).Count();
                                HttpContext.Session.SetString("NbrTicketMadeByUser", NbrTicketMadeByUser.ToString());
                                HttpContext.Session.SetString("role", "User");
                            }
                            HttpContext.Session.SetString("IdUtilisateur", p.Id.ToString());
                            return RedirectToAction("Index", "Accueil");
                        }
                        else
                        {
                            ViewBag.Wrong = false;
                        }
                }
                else
                {
                    ViewBag.Exist = false;
                }
            }
            return View();
        }


        public IActionResult FormInscription()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FormInscription(UtilisateurAddVM vm)
        {

            if (ModelState.IsValid)
            {
                int count = db.Utilisateurs.Where(u => u.Login == vm.Login).Count();
                if (count == 0)
                {
                    Utilisateur user = UtilisateurMapper.GetUtilisateurFromUtilisateurAddVM(vm, passwordHasher);
                    db.Personnes.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("FormAuthentification");
                }
                ModelState.AddModelError("Login", "Login existe deja");

            }
            return View();

        }
    
        public IActionResult SeDeconnecter()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(FormAuthentification));
        }
    }
}   

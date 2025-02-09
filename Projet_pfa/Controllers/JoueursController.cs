using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Projet_pfa.Filters;
using Projet_pfa.Mapper;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Controllers
{
    public class JoueursController : Controller
    {
        MyContext db;
        public JoueursController(MyContext db)
        {
            this.db = db;
        }
        private int CalculateAge(DateTime date)
        {
            var today = DateTime.Today; //La variable today est définie pour stocker la date actuelle sans l'heure 
            var age = today.Year - date.Year;
            if (date > today.AddYears(-age)) age--;
            return age;
        }
        public IActionResult AffichageJoueurs(int Id)
        {
            if(HttpContext.Session.GetString("role") == "Admin")
            {
                ViewBag.IsAdmin = true;
            }
            Equipe equipe = null;
            equipe= db.Equipes.Include(e => e.Joueurs).FirstOrDefault(e => e.Id == Id);
            if (equipe == null)
            {
                return NotFound();
            }

            ViewBag.NombreDeJoueurs = equipe.Joueurs.Count();
            ViewBag.Ages = equipe.Joueurs.ToDictionary(j => j.Id, j => CalculateAge(j.DateNaissance));

            // Grouper les joueurs par rôle, puis trier chaque groupe par numéro de maillot
            var groupedJoueurs = equipe.Joueurs
                .GroupBy(j => j.Role)
                .OrderBy(g => g.Key)
                .Select(g => g.OrderBy(j => j.NumeroMaillot).ToList())
                .ToList();
            TempData["Id"] = equipe.Id;
            return View(groupedJoueurs);

            // Passer les groupes de joueurs à la vue
        }

        [AuthentificationFilter("Admin")]
        public IActionResult AddJoueurs()
        {
            TempData["Id"] = (int)TempData["Id"];
            return View();
        }


        /* public IActionResult AddJoueurs(JoueurAddVM vm)
         {
             if (ModelState.IsValid)
             {
                 Joueur joueur = JoueurMapper.GetJoueurFromJoueurAddVM(vm);
                 db.Joueurs.Add(joueur);
                 try
                 {
                     db.SaveChanges();
                     return RedirectToAction("AffichageJoueurs", "Joueurs", new { Id = joueur.Id });
                 }
                 catch (DbUpdateException ex)
                 {
                     // Log l'erreur ou affichez un message d'erreur spécifique
                     // Vous pouvez aussi examiner l'inner exception pour plus de détails
                     ModelState.AddModelError("", "Une erreur est survenue lors de l'enregistrement : " + ex.InnerException?.Message);
                 }
             }
             return View(vm);
         }*/
        [HttpPost]
        [AuthentificationFilter("Admin")]
        public IActionResult AddJoueurs(JoueurAddVM vm)
        {
            int id = (int)TempData["Id"];
            Joueur j = null;
            string[] allowedExt = { ".jpg", ".png", ".jpeg" };
            //la variable model est le view model passé en paramêtre contenant la photo de type IFormFile
            string FileExt = Path.GetExtension(vm.Photo.FileName);
            if (allowedExt.Contains(FileExt.ToLower()))
            {
                //generation d'un nouveau nom unique 
                string NewName = Guid.NewGuid() + vm.Photo.FileName;
                //remplaçe UploadedImages par un nom de dossier
                string PathFile = Path.Combine("wwwroot/images/Joueur", NewName);
                //remplacement des \ par /
                PathFile = PathFile.Replace('\\', '/');

                using (FileStream stream = System.IO.File.Create(PathFile))
                {
                    //pour enregistrer dans la bd comme 'images/UploadedImages/nomImage' et pas comme 'wwwroot/images/UploadedImages/nonImage'
                    PathFile = PathFile.Substring(8);
                    vm.Photo.CopyTo(stream);
                    //mapper PublicationAddVM To publication
                    //publication = PublicationMapper.PublicationAddVMToPublication(vm, PathFile);
                    j = JoueurMapper.GetJoueurFromJoueurAddVM(vm, PathFile,id);
                    db.Joueurs.Add(j);
                }
                db.SaveChanges();
               
            }
            return RedirectToAction("AffichageJoueurs", "Joueurs" , new {Id = id});
        }
        [AuthentificationFilter("Admin")]
        public IActionResult Delete(int id)
        {
            Joueur joueur = db.Joueurs.Include(j => j.Equipe).FirstOrDefault(j => j.Id == id);
            if (joueur != null)
            {
                int equipeId = joueur.Equipe.Id; // Assurez-vous que le joueur a une équipe associée
                db.Joueurs.Remove(joueur);
                db.SaveChanges();
                return RedirectToAction("AffichageJoueurs", "Joueurs", new { Id = equipeId });
            }
            return RedirectToAction("AffichageJoueurs", "Joueurs"); // Ou vers une autre vue appropriée si le joueur n'existe pas
        }


        /*public IActionResult Update(JoueurUpdateVM vm)
        {
            Joueur joueur = db.Joueurs.Where(j => j.Id == vm.Id).FirstOrDefault();
            if (joueur == null)
            {
                return RedirectToAction("AffichageJoueurs", "Joueurs");
            }
            return View(joueur);
        }*/
        [AuthentificationFilter("Admin")]
        public IActionResult Update(int id)
        {
            Joueur joueur = db.Joueurs.Include(j => j.Equipe).FirstOrDefault(j => j.Id == id);
            if (joueur == null)
            {
                return NotFound(); // Renvoie une réponse 404 si le joueur n'est pas trouvé
            }

            JoueurUpdateVM model = new JoueurUpdateVM
            {
                
                Nom = joueur.Nom,
                Prenom = joueur.Prenom,
                DateNaissance = joueur.DateNaissance,
                NumeroMaillot = joueur.NumeroMaillot,
                // Assurez-vous d'inclure tous les autres champs pertinents que votre vue attend
            };

            return View(model); // Renvoie le ViewModel à la vue
        }

        [HttpPost]
        [AuthentificationFilter("Admin")]
        public IActionResult Update(int id,JoueurUpdateVM vm)
        {
            if (ModelState.IsValid)
            {
                Joueur joueur = db.Joueurs.Where(jo => jo.Id == id).Include(e => e.Equipe).FirstOrDefault();

                if (joueur == null)
                {
                    return RedirectToAction("AjoutEquipes", "Equipes");              
                }   
                joueur.Nom = vm.Nom;
                joueur.Prenom = vm.Prenom;
                joueur.DateNaissance = vm.DateNaissance;
                joueur.NumeroMaillot = vm.NumeroMaillot;
                db.SaveChanges();
                 
               // Console.WriteLine("Joueur mis à jour avec succès. Équipe ID: " + joueur?.Equipe?.Id);
                return RedirectToAction("AffichageJoueurs", "Joueurs", new { Id = joueur.EquipeId });
            }
            return View(vm);
        }
    }
}

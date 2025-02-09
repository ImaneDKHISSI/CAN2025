using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_pfa.Filters;
using Projet_pfa.Mappers;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Controllers
{
    public class PublicationController : Controller
    {
        MyContext db;
        public PublicationController(MyContext db)
        {
            this.db = db;
        }
        
        public IActionResult AddPublication()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPublication(PublicationAddVM model)
        {
            if (ModelState.IsValid)
            {
                Publication publication = null;
                if (model.Photo != null)
                {
                    string[] allowedExt = { ".jpg", ".png", "jpeg" };
                    //la variable model est le view model passé en paramêtre 
                    string FileExt = Path.GetExtension(model.Photo.FileName);
                    if (allowedExt.Contains(FileExt.ToLower()))
                    {
                        //generation d'un nouveau nom unique 
                        string NewName = Guid.NewGuid() + model.Photo.FileName;
                        //remplaçe UploadedImages par un nom de dossier
                        string PathFile = Path.Combine("wwwroot/images/UploadedImages", NewName);
                        //remplacement des \ par /
                        PathFile = PathFile.Replace('\\', '/');

                        using (FileStream stream = System.IO.File.Create(PathFile))
                        {
                            //pour enregistrer dans la bd comme 'images/UploadedImages/nomImage' et pas comme 'wwwroot/images/UploadedImages/nonImage'
                            PathFile = PathFile.Substring(8);
                            model.Photo.CopyTo(stream);
                            //mapper PublicationAddVM To publication
                            publication = PublicationMapper.PublicationAddVMToPublication(model, PathFile);


                        }
                    }
                }
                else
                {
                    publication = PublicationMapper.PublicationAddVMToPublication(model);
                }

                publication.PersonneId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
                db.Publications.Add(publication);
                db.SaveChanges();
                return RedirectToAction("GetPublication");


            }
            return View();
        }


        public IActionResult Getpublications()
        {
            List<Publication> publication = db.Publications
                .Include(p => p.Commentaires)
                .ThenInclude(c => c.Personne)
                .Include(p => p.Likes).ToList();

            if (HttpContext.Session.GetString("role") != null && HttpContext.Session.GetString("role") == "Admin")
            {
                ViewBag.role = "Admin";
            }
            else
            {
                ViewBag.role = "user";
            }
            return View(publication);
        }


        [AuthentificationFilter("User","Admin")]
        public IActionResult Like(int Id)
        {
            Publication publication = db.Publications.FirstOrDefault(p => p.Id == Id);
            if (publication == null)
            {
                return NotFound();
            }
            int CurrentPersonId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
            Personne CurrentPerson = db.Personnes.FirstOrDefault(u=>u.Id== CurrentPersonId);
            // Vérifiez si l'utilisateur a déjà aimé cette publication
            Like isLiked = db.Likes.FirstOrDefault(l => l.PersonneId == CurrentPersonId && l.PublicationId == Id);
            if (isLiked != null)
            {
                db.Likes.Remove(isLiked);
            }
            else
            {
                Like like = new Like()
                {
                    //Utilisateur = Currentuser,
                    PersonneId = CurrentPersonId,
                    Publication = publication,
                    PublicationId = Id
                };
                db.Likes.Add(like);
            }
            
            db.SaveChanges();


            return RedirectToAction("Getpublications");
        }

        [HttpPost]

        [AuthentificationFilter("User", "Admin")]
        public IActionResult AjouterCommentaire(int PublicationId, string Texte)
        {
            int CurrentPersonId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
            Personne CurrentPerson = db.Personnes.FirstOrDefault(u => u.Id == CurrentPersonId);
            //Publication publication = db.Publications.FirstOrDefault(p => p.Id == PublicationId);
            Publication publication = db.Publications.Include(p => p.Commentaires).Include(p => p.Likes).FirstOrDefault(p => p.Id == PublicationId);
            if (publication == null)
            {
                return NotFound();
            }
            Commentaire NouveauCommentaire = new Commentaire
            {
                Text = Texte,
                Date = DateTime.UtcNow,
                Publication = publication,
                PublicationId = PublicationId,
                PersonneId = CurrentPersonId,
                Personne = CurrentPerson

            };
            db.Commentaires.Add(NouveauCommentaire);
            db.SaveChanges();


            return RedirectToAction("Getpublications");
        }
        [AuthentificationFilter("Admin")]
        public IActionResult DeletePublication(int Id)
        {
            Publication publication = db.Publications.Include(l => l.Likes).Include(c => c.Commentaires).FirstOrDefault(p => p.Id == Id);
            if (publication != null)
            {
                //suppression des commentaire 
                foreach (Commentaire cm in publication.Commentaires)
                {
                    db.Commentaires.Remove(cm);
                }
                //suppression des likes
                foreach (Like l in publication.Likes)
                {
                    db.Likes.Remove(l);
                }

                //suppression des publications
                db.Publications.Remove(publication);
                db.SaveChanges();
                return RedirectToAction("Getpublications");
            }
            //en cascade !!!!!!important
            return View("Getpublications");

        }
        //on a enlever httppost
        [AuthentificationFilter("Admin")]
        public IActionResult DeleteComment(int Id)
        {
            Commentaire comment = db.Commentaires.FirstOrDefault(c => c.Id == Id);
            if (comment != null)
            {
                db.Commentaires.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Getpublications");
            }

            return View("Getpublications");
        }

        //[AuthentificationFilter("Admin")]
        [AuthentificationFilter("User","Admin")]
        public IActionResult IsPublicationLiked(int publicationId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("IdUtilisateur"));
            Personne p = db.Personnes.FirstOrDefault(p => p.Id == userId);
            bool isliked;
            if (p != null)
            {
                isliked = db.Likes.Any(l => l.PersonneId == userId && l.PublicationId == publicationId);
                return Json(new {Liked = isliked });
            }
            isliked = false;
            return Json(new { Liked = isliked });
            
        }

    }
}

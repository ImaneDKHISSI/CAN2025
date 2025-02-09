using Microsoft.AspNetCore.Mvc;
using Projet_pfa.Filters;
using Projet_pfa.Mappers;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Controllers
{
    
    public class NewsController : Controller
    {
        private readonly MyContext _context;

        public NewsController(MyContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            var newsList = _context.News.OrderByDescending(n => n.Date).ToList();
            ViewBag.NewsList = newsList;
            
            return View();
        }
        [AuthentificationFilter("Admin")]
        public IActionResult GererNews()
        {
            var newsList = _context.News;
            ViewBag.NewsList = newsList;
            return View();
        }
        [AuthentificationFilter("Admin")]
        public IActionResult FormAjouter()
        {
            return View();
        }
        [AuthentificationFilter("Admin")]
        public IActionResult Ajouter(AddNewVM model)
        {
            News news = null;
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
                        PathFile = PathFile.Substring(15);
                        model.Photo.CopyTo(stream);

                        //mapper PublicationAddVM To publication
                        news = NewsMapper.NewsVmToNews(9, PathFile, model.Texte, model.Titre);


                    }
                    
                }
            }
            _context.News.Add(news);
            _context.SaveChanges();
            return RedirectToAction("GererNews");
        }
        [AuthentificationFilter("Admin")]
        public IActionResult FormModifier(int id)
        {
			var news = _context.News.FirstOrDefault(e => e.Id == id);
			ViewBag.News = news;
            return View();
		}
        [AuthentificationFilter("Admin")]
        public IActionResult Modifier(int id, UpdateNewsVM model,string texte)
        {
            News news = null;
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
                        PathFile = PathFile.Substring(15);
                        model.Photo.CopyTo(stream);

                        //mapper PublicationAddVM To publication
                         news = _context.News.FirstOrDefault(e => e.Id == id);
                        news.Titre = model.Titre;
                        news.Texte = texte;
                        news.Photo = PathFile;


                    }

                }
            }
            _context.SaveChanges();
            return RedirectToAction("GererNews");
        }
        [AuthentificationFilter("Admin")]
        public IActionResult Supprimer(int id)
        {
            News news = _context.News.FirstOrDefault(e => e.Id == id);
            _context.News.Remove(news);
            _context.SaveChanges();
            return RedirectToAction("GererNews");
        }
    }
}

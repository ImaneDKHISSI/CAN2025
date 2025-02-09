using Microsoft.AspNetCore.Mvc;
using Projet_pfa.Models;

namespace Projet_pfa.Controllers
{
    public class AccueilController : Controller
    {
        private readonly MyContext _context;

        public AccueilController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }
            var newsList = _context.News.OrderByDescending(n => n.Date).Take(10).ToList();
            ViewBag.NewsList = newsList;
            return View();
        }
    }
}

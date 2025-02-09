using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_pfa.Models;

namespace Projet_pfa.Controllers
{
    public class EquipesController : Controller
    {
        MyContext db;
        public EquipesController(MyContext db)
        {
            this.db = db;
        }

        public IActionResult AjoutEquipes()
        {
            List<Groupe> Groupes = db.Groupes.Distinct().Include(g => g.Equipes).AsNoTracking().ToList();

            return View(Groupes);
        }

       


    }
}

using Projet_pfa.Models;

namespace Projet_pfa.Mappers
{
    public class NewsMapper
    {
        public static News NewsVmToNews(int adminId ,string pathfile , string text , string titre)
        {
           
            News news = new News()
            {
                AdministrateurId = adminId,
                Date = DateTime.Now,
                Photo = pathfile,
                Texte = text,
                Titre = titre,
            };
            return news;
        }
    }
}

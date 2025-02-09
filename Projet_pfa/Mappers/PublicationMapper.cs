using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mappers
{
    public class PublicationMapper
    {
        public static Publication PublicationAddVMToPublication(PublicationAddVM PublicationAddViewModel,string path)
        {
            Publication publication = new Publication()
            {
                Photo = path,
                Date=DateTime.Now,
                Texte = PublicationAddViewModel.Texte
                
            };
            return publication;
        }
        public static Publication PublicationAddVMToPublication(PublicationAddVM PublicationAddViewModel)
        {
            Publication publication = new Publication()
            {
                
                Date = DateTime.Now,
                Texte = PublicationAddViewModel.Texte

            };
            return publication;
        }
    }
}

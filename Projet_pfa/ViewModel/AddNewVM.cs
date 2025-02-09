using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class AddNewVM
    {
        [Required(ErrorMessage ="Obligatoire")]
        public string Titre { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        public string Texte { get; set; }
    }
}

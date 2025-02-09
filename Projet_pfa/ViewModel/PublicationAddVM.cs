using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class PublicationAddVM
    {

        [Required(ErrorMessage ="Champs obligatoire")]   
       // [Display(Name = "Texte Du Publication")]
        public string Texte { get; set; }
      //  [Display(Name ="Ajouter une image")]
        public IFormFile? Photo { get; set; }
    }
}

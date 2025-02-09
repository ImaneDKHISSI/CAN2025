using Projet_pfa.Models;
using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class UtilisateurAddVM
    {
        [Required(ErrorMessage = "Le Nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le Prenom est obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La Login est obligatoire")]
        public string Login { get; set; }
        [MinLength(6, ErrorMessage = "Le password>6 caractères")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La Confirmation du Mot De Passe est obligatoire")]
        [Compare("Password", ErrorMessage = "les 2 password doivent être equivalent")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation de Password")]
        public string ConfirmationMotDePasse { get; set; }
        

    }
}

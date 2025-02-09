using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class FormulaireAuthentification
    {
        [Required(ErrorMessage = "Le Nom d'utlisateur est obligatoire")]
        [Display(Name ="Nom D'utilisateur")]
        public string NomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le Password est obligatoire")]
        [DataType(DataType.Password)]
        [Display(Name ="Mot de passe")]
        public string MotDePasse { get; set; }
    }
}

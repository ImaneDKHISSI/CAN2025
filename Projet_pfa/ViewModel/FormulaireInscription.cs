using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class FormulaireInscription
    {
        [Required(ErrorMessage = "Le Nom est obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le Prenom est obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La Nationalite est obligatoire")]
        public string Nationalite { get; set; }

        [Required(ErrorMessage = "Le Nom D'Utilisateur est obligatoire")]
        public string NomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le Mot de Passe est obligatoire")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "La Confirmation du Mot De Passe est obligatoire")]
        [DataType(DataType.Password)]
        [Compare("MotDePasse", ErrorMessage = "vous devez donner le même mot de passe !")]
        public string ConfirmationMotDePasse { get; set; }
    }
}

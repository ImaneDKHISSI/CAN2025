using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class JoueurUpdateVM
    {
        
        [Required(ErrorMessage = "Le nom est obligatoire")]

        public string Nom { get; set; }
        [Required(ErrorMessage = "Le prenom est obligatoire")]

        public string Prenom { get; set; }
        [Required(ErrorMessage = "Le numero de Maillot est obligatoire")]

        public int NumeroMaillot { get; set; }
        [Required(ErrorMessage = "L'age est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        //public byte[] Photo {  get; set; }
        //[Required(ErrorMessage = "La photo est obligatoire")]

        //public IFormFile Photo { get; set; }
    }
}

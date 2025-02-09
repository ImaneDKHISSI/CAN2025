using System.ComponentModel.DataAnnotations;

namespace Projet_pfa.ViewModel
{
    public class TicketInformation
    {
        [Required(ErrorMessage ="Veuillez saisir Votre nom complet")]
        
        public List<string> NomComplets { get; set; }
    }
}

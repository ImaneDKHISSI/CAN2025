using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_pfa.Models
{
	[Table("Equipe")]
	public class Equipe
	{
		public int Id { get; set; }
		public string Nom { get; set; }
		public string Drapeau {  get; set; }
		
		public IList<Match> MatchesEquipe1 { get; set; }
		public IList<Match> MatchesEquipe2 { get; set; }
		public IList<Joueur> Joueurs { get; set; }
		public Groupe Groupe { get; set; }
		public int GroupeId { get; set; }
		
	}
}

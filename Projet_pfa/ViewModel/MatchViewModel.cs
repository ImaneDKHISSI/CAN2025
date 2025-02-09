using Projet_pfa.Models;

namespace Projet_pfa.ViewModel
{
	public class MatchViewModel
	{
		public DateTime Date { get; set; }
		public int IdEquipe1 { get; set; }
		public int IdEquipe2 { get; set; }
		public int IdStade { get; set; }
		public double PrixSiegeCat1 { get; set; }
		public double PrixSiegeCat2 { get; set; }
		public double PrixSiegeCat3 { get; set; }
		public string? Resultat { get; set; }
        public List<Equipe>? equipes1 { get; set; }
        public List<Equipe>? equipes2 { get; set; }
        public List<Stade>? stades { get; set; }
        public Match? match { get; set; }

    }
}

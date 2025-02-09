namespace Projet_pfa.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public DateTime Date { get; set; }
        public string Photo { get; set; }
        public string Texte { get; set; }
        public Administrateur Admin { get; set; }
        public int AdministrateurId { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Projet_pfa.Models
{
    public class MyContext : DbContext
    {
        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Administrateur> Administrateurs { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Joueur> Joueurs { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Nationalite> Nationalites { get; set; }
        public DbSet<Ville> Villes { get; set; }
        public DbSet<Stade> Stades { get; set; }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<News> News { get; set; }

        public MyContext(DbContextOptions<MyContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Equipe1)
                .WithMany(e => e.MatchesEquipe1)
                .HasForeignKey(m => m.Equipe1Id);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Equipe2)
                .WithMany(e => e.MatchesEquipe2)
                .HasForeignKey(m => m.Equipe2Id);

            modelBuilder.Entity<Personne>()
            .HasDiscriminator<string>("PersonneType")
            .HasValue<Administrateur>("Administrateur")
            .HasValue<Utilisateur>("Utilisateur");

            modelBuilder.Entity<Match>().Property(m => m.RowVersion).IsConcurrencyToken();
            

            modelBuilder.Entity<Stade>().HasData(
                new Stade { Id = 2, Nom = "Stade d'honneur", nbSiegeCat1 = 100, nbSiegeCat2 = 300, nbSiegeCat3 = 3000, VilleId = 1, Localisation = "https://maps.app.goo.gl/hjaBBckWqvtf8XQ78" },
                new Stade { Id = 3, Nom = "Stade municipal", nbSiegeCat1 = 50, nbSiegeCat2 = 100, nbSiegeCat3 = 1500, VilleId = 1, Localisation = "https://maps.app.goo.gl/UA3UjupwsANjP6Ko6" }
            );
            modelBuilder.Entity<Match>().HasData(
                new Match { Id = 6, Equipe1Id = 51, Equipe2Id = 46, Resultat = "2 - 0", Date = new DateTime(2024, 03, 15), StadeId = 2, PrixSiegeCat1 = 500, PrixSiegeCat2 = 200, PrixSiegeCat3 = 100 },
                new Match { Id = 7, Equipe1Id = 51, Equipe2Id = 50, Resultat = "2 - 1", Date = new DateTime(2024, 04, 1), StadeId = 3, PrixSiegeCat1 = 500, PrixSiegeCat2 = 200, PrixSiegeCat3 = 100 },
                new Match { Id = 8, Equipe1Id = 46, Equipe2Id = 50, Resultat = "1 - 1", Date = new DateTime(2024, 06, 15), StadeId = 2, PrixSiegeCat1 = 500, PrixSiegeCat2 = 200, PrixSiegeCat3 = 100 }
            );
            
            
            
            modelBuilder.Entity<News>().HasData(
                new News {Id = 1,  AdministrateurId = 9, Titre = "Les Lions de l'Atlas en finale", Date = new DateTime(2024, 05, 23), Photo = "slide1.jpg", Texte = "Dans une demi-finale mémorable de la CAN 2025, les Lions de l'Atlas démontrent leur détermination et leur talent exceptionnel sur le terrain. Cette victoire historique propulse le Maroc en finale, où ils auront l'occasion de poursuivre leur quête du titre continental et d'écrire une nouvelle page glorieuse dans l'histoire du football marocain." },
                new News { Id = 2, AdministrateurId = 9, Titre = "Les Préparatifs de la Finale : Maroc vs Côte d'Ivoire", Date = new DateTime(2024, 05, 23), Photo = "slide2.jpg", Texte = "Alors que la tension monte et que l'excitation atteint son paroxysme, les préparatifs battent leur plein pour la finale tant attendue de la Coupe d'Afrique des Nations (CAN) 2025. Avec une route jonchée de victoires impressionnantes et de performances exceptionnelles, les équipes finalistes, le Maroc et la Côte d'Ivoire, se préparent à s'affronter dans un duel épique pour décrocher le prestigieux titre continental." },
                new News { Id = 3, AdministrateurId = 9, Titre = "Titre 3", Date = new DateTime(2024, 05, 23), Photo = "slide3.jpg", Texte = "Le football, sport emblématique, passionne des foules à travers le monde. Sur les terrains, les joueurs s'affrontent avec ferveur, démontrant leur habileté et leur stratégie. Les stades résonnent des chants des supporters, créant une ambiance électrique. Chaque match est un spectacle unique, mêlant adrénaline, compétition et émotion intense." }
            );


        }
    } 
}
    


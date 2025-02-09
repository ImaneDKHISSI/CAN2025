using Microsoft.AspNetCore.Identity;
using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mappers
{
    public class UtilisateurMapper
    {
        public static Utilisateur GetUtilisateurFromUtilisateurAddVM(UtilisateurAddVM vm, PasswordHasher<Utilisateur> passwordHasher)
        {
            Utilisateur user = new Utilisateur();
            user.Nom = vm.Nom;
            user.Prenom = vm.Prenom;
            user.Login = vm.Login;
            user.Password = vm.Password;
            user.NationaliteId = 1;
            user.Password = passwordHasher.HashPassword(null, vm.Password);  // Hashage du mot de passe

            return user;
        }

    }
}

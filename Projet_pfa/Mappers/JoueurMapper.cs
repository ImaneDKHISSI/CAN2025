using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mapper
{
    public class JoueurMapper
    {
        public static Joueur GetJoueurFromJoueurAddVM(JoueurAddVM vm,string PathFile,int EquipeId)
        {
            Joueur j=new Joueur();
            j.Nom = vm.Nom;
            j.Prenom = vm.Prenom;
            j.NumeroMaillot= vm.NumeroMaillot;
            j.DateNaissance = vm.DateNaissance;
            j.Photo = PathFile;
            j.Role= vm.Role;
            j.EquipeId = EquipeId;
            return j;
        }
    }
}

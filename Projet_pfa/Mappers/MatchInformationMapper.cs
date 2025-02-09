using Projet_pfa.Models;
using Projet_pfa.ViewModel;

namespace Projet_pfa.Mappers
{
    public class MatchInformationMapper
    {
        public static MatchInformations GetMatchInfoFromMatch(Match m)
        {

            string dateOnly = m.Date.ToString("dddd dd MMMM yyyy").ToUpper();
            MatchInformations matchinfo = new MatchInformations
            {
                PrixSiegeCat1=m.PrixSiegeCat1,
                PrixSiegeCat2=m.PrixSiegeCat2,
                PrixSiegeCat3=m.PrixSiegeCat3,
                equipe1 = m.Equipe1,
                equipe2 = m.Equipe2,
                stade = m.Stade,
                date = dateOnly
            };


            return matchinfo;
        }
    }
}

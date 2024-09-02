using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Model;

namespace MyApp.Game
{
    public class KnockoutStage
    {
        public List<GroupTeam> QualifiedTeams { get; set; }
        public List<Match> QuarterFinalMatches { get; private set; }
        public List<Match> SemiFinalMatches { get; private set; }
        public Match ThirdPlaceMatch { get; private set; }
        public Match FinalMatch { get; private set; }

        public KnockoutStage(List<GroupTeam> qualifiedTeams)
        {
            QualifiedTeams = qualifiedTeams;
            QuarterFinalMatches = new List<Match>();
            SemiFinalMatches = new List<Match>();

            DrawQuarterFinals();
            DrawSemiFinals();
            ThirdPlaceMatch = DrawAndSimulateThirdPlaceMatch();
            FinalMatch = SimulateFinal(); 
        }

        public void DrawQuarterFinals()
        {
            var hatD = QualifiedTeams.Take(2).ToList(); // 1 i 2
            var hatE = QualifiedTeams.Skip(2).Take(2).ToList(); // 3 i 4
            var hatF = QualifiedTeams.Skip(4).Take(2).ToList(); // 5 i 6
            var hatG = QualifiedTeams.Skip(6).Take(2).ToList(); // 7 i 8

            printHats(hatD, hatE, hatF, hatG);

            // D i G
            var random = new Random();
            var dgPairings = CreatePairings(hatD, hatG, random);

            // E i F
            var efPairings = CreatePairings(hatE, hatF, random);

            QuarterFinalMatches.AddRange(dgPairings);
            QuarterFinalMatches.AddRange(efPairings);
        }

        private List<Match> CreatePairings(List<GroupTeam> pot1, List<GroupTeam> pot2, Random random)
        {
            var pairings = new List<Match>();
            var pot2Available = new List<GroupTeam>(pot2);

            foreach (var team1 in pot1)
            {
                GroupTeam? opponent = null;
                var candidates = pot2Available
                    .Where(op => !HavePlayedEachOther(team1, op))
                    .ToList();

                if (candidates.Count > 0)
                {
                    int index = random.Next(candidates.Count);
                    opponent = candidates[index];
                    pot2Available.Remove(opponent);
                }
                else
                {
                    opponent = pot2Available[random.Next(pot2Available.Count)];
                    Console.WriteLine($"Mora doci do ponavljanja {team1.Team} i {opponent.Team}");
                    pot2Available.Remove(opponent);
                }

                pairings.Add(new Match(team1, opponent));
            }

            return pairings;
        }


        private bool HavePlayedEachOther(GroupTeam teamA, GroupTeam teamB)
        {
            return teamA.MatchesPlayed.Any(m => (m.TeamA == teamA && m.TeamB == teamB) || (m.TeamA == teamB && m.TeamB == teamA));
        }

        public void DrawSemiFinals()
        {
            var random = new Random();
            var quarterFinalWinners = QuarterFinalMatches.Select(m => m.Winner).ToList();

            var shuffledWinners = quarterFinalWinners.OrderBy(_ => random.Next()).ToList();

            if (quarterFinalWinners.Count != 4 || shuffledWinners.Count % 2 != 0)
            {
                throw new InvalidOperationException("Greška. Ne možemo formirati polufinalne parove.");
            }

            for (int i = 0; i < shuffledWinners.Count; i += 2)
            {
                if (i + 1 >= shuffledWinners.Count)
                {
                    throw new InvalidOperationException("Nedostaje tim za formiranje para u polufinalu.");
                }
                SemiFinalMatches.Add(new Match(shuffledWinners[i], shuffledWinners[i + 1])); 
            }
        }

        private Match DrawAndSimulateThirdPlaceMatch()
        {
            var semiFinalLosers = SemiFinalMatches
                .Select(m => m.Loser)
                .Where(loser => loser != null)
                .ToList();

            if (semiFinalLosers.Count < 2)
            {
                throw new InvalidOperationException("Nema dovoljno gubitnika iz polufinala za utakmicu za treće mesto.");
            }

            var thirdPlaceMatch = new Match(semiFinalLosers[0], semiFinalLosers[1]);
            return thirdPlaceMatch;
        }

      
         private Match SimulateFinal(){
            // Simulacija finala
            var finalTeams = SemiFinalMatches.Select(m => m.Winner).ToList();
            var finalMatch = new Match(finalTeams[0], finalTeams[1]);
            return finalMatch;
        }

        public void PrintKnockoutResults()
        {
            Console.WriteLine("\nEliminaciona faza:");
            foreach (var match in QuarterFinalMatches)
            {
                Console.WriteLine($"    {match.TeamA.Team} - {match.TeamB.Team}");
            }
            Console.WriteLine("\nRezultati četvrtfinala:");
            foreach (var match in QuarterFinalMatches)
            {
                Console.WriteLine($"    {match}");
            }

            Console.WriteLine("\nRezultati polufinala:");
            foreach (var match in SemiFinalMatches)
            {
                Console.WriteLine($"    {match}");
            }
            
            Console.WriteLine("\nUtakmica za treće mesto:");
            Console.WriteLine($"    {ThirdPlaceMatch}");

            Console.WriteLine("\nFinale:");
            Console.WriteLine($"    {FinalMatch}");

            PrintMedalResults();
        }

        public void PrintMedalResults()
        {
            string gold = FinalMatch.Winner != null ? FinalMatch.Winner.Team : "N/A";
            string silver = FinalMatch.Loser != null ? FinalMatch.Loser.Team : "N/A";
            string bronze = ThirdPlaceMatch.Winner != null ? ThirdPlaceMatch.Winner.Team : "N/A";

            Console.WriteLine("\nMedalje:");
            Console.WriteLine($"    1. {gold}");
            Console.WriteLine($"    2. {silver}");
            Console.WriteLine($"    3. {bronze}");
        }

        public void printHats(List<GroupTeam> hatD, List<GroupTeam> hatE, List<GroupTeam> hatF, List<GroupTeam> hatG){
            Console.WriteLine("\nŠeširi:");
            Console.WriteLine("    Šešir D");
            foreach (var team in hatD)
            {
                Console.WriteLine($"        {team.Team}");
            }

            Console.WriteLine("    Šešir E");
            foreach (var team in hatE)
            {
                Console.WriteLine($"        {team.Team}");
            }

            Console.WriteLine("    Šešir F");
            foreach (var team in hatF)
            {
                Console.WriteLine($"        {team.Team}");
            }

            Console.WriteLine("    Šešir G");
            foreach (var team in hatG)
            {
                Console.WriteLine($"        {team.Team}");
            }
        }
    }
}

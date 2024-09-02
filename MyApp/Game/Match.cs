using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Model;

namespace MyApp.Game
{

    public class Match
    {
        public GroupTeam TeamA { get; set; }
        public GroupTeam TeamB { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public GroupTeam Winner { get; private set; }
        public GroupTeam Loser { get; private set; }
        private static Random rand = new Random();

        public Match(GroupTeam teamA, GroupTeam teamB)
        {
            TeamA = teamA;
            TeamB = teamB;
            Winner = teamA;
            Loser = teamB;
            SimulateMatch();
        }

    private void SimulateMatch()
    {
       
        double probabilityAWin = 1.0 / (1.0 + Math.Pow(10, (TeamB.FIBARanking - TeamA.FIBARanking) / 400.0)); // Verovatnoća pobede na osnovu FIBA rankinga
       
        double formProbabilityAWin = probabilityAWin + (TeamA.Form - TeamB.Form) / 100.0; // Verovatnoća pobede na osnovu forme tima
        formProbabilityAWin = Math.Clamp(formProbabilityAWin, 0.0, 1.0);
        
        double randomValue = rand.NextDouble(); // Generiše broj između 0 i 1
        
        // Ako je formProbabilityAWin = 0.7 random broj koji je manji od 0.7 je više verovatan rezultat
        // što znači da će tim A pobediti u 70% slučajeva.
        if (randomValue < formProbabilityAWin)
        {
            // Tim A pobedi
            ScoreA = rand.Next(71, 100);
            ScoreB = rand.Next(70, ScoreA);
            Winner = TeamA;
            Loser = TeamB;
        }
        else
        {
            // Tim B pobedi
            ScoreB = rand.Next(71, 100);
            ScoreA = rand.Next(70, ScoreB);
            Winner = TeamB;
            Loser = TeamA;
        }
        TeamA.UpdateStatsAndForm(TeamB, ScoreA, ScoreB, Winner == TeamA);
        TeamB.UpdateStatsAndForm(TeamA, ScoreB, ScoreA, Winner == TeamB);
    }

        public override string ToString()
        {
            return $"{TeamA.Team} - {TeamB.Team} ({ScoreA}:{ScoreB})";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Model;

namespace MyApp.Game
{
    public class GroupPhase
    {
        private List<GroupTeam> Teams{ get; set; }

        public GroupPhase()
        {
            Teams = [];
        }

        
        public List<Match> SimulateGroupMatches(List<GroupTeam> teams)
        {
            var matches = new List<Match>();

            // Svaka ekipa igra sa svakom u svojoj grupi
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = i + 1; j < teams.Count; j++)
                {
                    Match match = new Match(teams[i], teams[j]);
                    matches.Add(match);
                    teams[i].MatchesPlayed.Add(match); 
                    teams[j].MatchesPlayed.Add(match);
                }
            }

            return matches;
        }

        public List<GroupTeam> GroupResults(string groupName, List<GroupTeam> teams, List<Match> matches)
        {
            Console.WriteLine($"\nGrupa {groupName}:");
            foreach (var match in matches)
            {
                Console.WriteLine($"    {match}");
            }

            Console.WriteLine($"\nKonačan plasman u grupi {groupName}:");

            // Rangiranje po bodovima
            teams = teams.OrderByDescending(t => t.Points).ToList();

            // Dodatna logika za rangiranje po međusobnim susretima
            for (int i = 0; i < teams.Count - 1; i++)
            {
                var currentTeam = teams[i];
                var nextTeam = teams[i + 1];

                if (currentTeam.Points == nextTeam.Points)
                {
                    var match = matches.FirstOrDefault(m =>
                        (m.TeamA == currentTeam && m.TeamB == nextTeam) ||
                        (m.TeamA == nextTeam && m.TeamB == currentTeam));

                    if (match != null)
                    {
                        // Ako je trenutni tim pobedio ostaje na istom indeksu
                        if ((match.TeamA == currentTeam && match.ScoreA > match.ScoreB) ||
                            (match.TeamB == currentTeam && match.ScoreB > match.ScoreA))
                        {
                            continue;
                        }
                        else
                        {
                            // Ako je trenutni tim izgubio, menjamo im mesta
                            teams[i] = nextTeam;
                            teams[i + 1] = currentTeam;
                        }
                    }
                }
            }

            // Logika za formiranje kruga
            for (int i = 0; i < teams.Count - 2; i++)
            {
                if (teams[i].Points == teams[i + 1].Points && teams[i + 1].Points == teams[i + 2].Points)
                {
                    //Pravimo novu listu od ta 3 tima
                    var subset = new List<GroupTeam> { teams[i], teams[i + 1], teams[i + 2] };
                    subset = subset.OrderByDescending(t => GetSubsetPointDifference(t, subset, matches)) //Timovi se prvo porede po ukupnoj razlici poena u međusobnim utakmicama.
                                .ThenByDescending(t => GetSubsetScoredPoints(t, subset, matches)) //Ako su i dalje izjednačeni, onda se porede po ukupnom broju postignutih poena u međusobnim utakmicama.
                                .ToList();

                    teams[i] = subset[0];
                    teams[i + 1] = subset[1];
                    teams[i + 2] = subset[2];
                }
            }
            // Teams = new List<GroupTeam>(teams);

            for (int i = 0; i < teams.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {teams[i]}");
            }
            return teams;
        }

        private int GetSubsetPointDifference(GroupTeam team, List<GroupTeam> subset, List<Match> matches)
        {
            int pointDifference = 0;
            foreach (var match in matches)
            {
                if (subset.Contains(match.TeamA) && subset.Contains(match.TeamB))
                {
                    if (match.TeamA == team)
                    {
                        pointDifference += match.ScoreA - match.ScoreB;
                    }
                    else if (match.TeamB == team)
                    {
                        pointDifference += match.ScoreB - match.ScoreA;
                    }
                }
            }
            return pointDifference;
        }

        private int GetSubsetScoredPoints(GroupTeam team, List<GroupTeam> subset, List<Match> matches)
        {
            int scoredPoints = 0;
            foreach (var match in matches)
            {
                if (subset.Contains(match.TeamA) && subset.Contains(match.TeamB))
                {
                    if (match.TeamA == team)
                    {
                        scoredPoints += match.ScoreA;
                    }
                    else if (match.TeamB == team)
                    {
                        scoredPoints += match.ScoreB;
                    }
                }
            }
            return scoredPoints;
        }

        public List<GroupTeam> RankTeamsAfterGroupPhase(List<Group> groups)
        {
            var firstPlaceTeams = new List<GroupTeam>();
            var secondPlaceTeams = new List<GroupTeam>();
            var thirdPlaceTeams = new List<GroupTeam>();

            foreach (var group in groups)
            {           
                firstPlaceTeams.Add(group.Teams[0]);
                secondPlaceTeams.Add(group.Teams[1]);
                thirdPlaceTeams.Add(group.Teams[2]);
            }

            var rankedTeams = new List<GroupTeam>();

            // Rangiranje prvoplasiranih
            rankedTeams.AddRange(RankSubsetTeams(firstPlaceTeams));

            // Rangiranje drugoplasiranih
            rankedTeams.AddRange(RankSubsetTeams(secondPlaceTeams));

            // Rangiranje trećeplasiranih
            rankedTeams.AddRange(RankSubsetTeams(thirdPlaceTeams));

            return rankedTeams;
        }

        private List<GroupTeam> RankSubsetTeams(List<GroupTeam> teams)
        {
            return teams.OrderByDescending(t => t.Points)
                        .ThenByDescending(t => t.GetPointDifference())
                        .ThenByDescending(t => t.ScoredPoints)
                        .ToList();
        }

        public List<GroupTeam> PrintFinalStandingsAndQualifiedTeams(List<Group> groups)
        {
            var rankedTeams = RankTeamsAfterGroupPhase(groups);

            Console.WriteLine("\nKonačan plasman u grupnoj fazi:");
            for (int i = 0; i < rankedTeams.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {rankedTeams[i]}");
            }

            Console.WriteLine($"\nEkipa koja ispada: {rankedTeams[8]}");
            rankedTeams.Remove(rankedTeams[8]);
            return rankedTeams;
        }

    }
}
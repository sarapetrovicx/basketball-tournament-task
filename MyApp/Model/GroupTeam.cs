using MyApp.Game;

namespace MyApp.Model{
    public class GroupTeam{
        public string Team { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Points { get; private set; }
        public int ScoredPoints { get; private set; }
        public int ConcededPoints { get; private set; }
        public List<Match> MatchesPlayed { get; set; }
        public double Form { get; set; }

        public GroupTeam(string team, string isoCode, int fibaRanking)
        {
            Team = team;
            ISOCode = isoCode;
            FIBARanking = fibaRanking;
            Wins = 0;
            Losses = 0;
            Points = 0;
            ScoredPoints = 0;
            ConcededPoints = 0;
            MatchesPlayed = new List<Match>();
            Form = 0.0;
        }

        public void UpdateStatsAndForm(GroupTeam opponent, int scoreFor, int scoreAgainst, bool won)
        {
            UpdateStats(scoreFor, scoreAgainst, won);
            UpdateTeamForm(scoreFor, scoreAgainst, opponent);
        }


        public void UpdateStats(int scored, int conceded, bool won)
        {
            ScoredPoints += scored;
            ConcededPoints += conceded;
            if (won)
            {
                Wins++;
                Points += 2;
            }
            else
            {
                Losses++;
                Points += 1;
            }
        }

        //   "CAN": [
        //       {
        //         "Date": "11/07/24",
        //         "Opponent": "USA",
        //         "Result": "72-86"
        //       },
        //       {
        //         "Date": "21/07/24",
        //         "Opponent": "PRI",
        //         "Result": "103-93"
        //       }
        //     ],
        public void CalculateTeamForm(TeamExhibitions teamExhibition, HashSet<GroupTeam> uniqueTeams)
        {
            foreach (var match in teamExhibition.Matches)
            {
                var scores = match.Result.Split('-').Select(int.Parse).ToArray();
                int scoredPoints = scores[0];
                int concededPoints = scores[1];
                int pointDifference = scoredPoints - concededPoints; 

                var opponent = uniqueTeams.FirstOrDefault(t => t.ISOCode == match.Opponent);
                var opponentRanking = opponent != null ? opponent.FIBARanking : 10; 

                // Što je veća razlika u korist tima, to je veća forma
                // Što je manji ranking (jači protivnik), to je veća forma
                Form += pointDifference * (1.0 / opponentRanking); //-14+0.62=-13.38
            }
            Form = Math.Round(Form, 2);

        }

        public void UpdateTeamForm(int scoreFor, int scoreAgainst, GroupTeam opponent)
        {
            int resultDifference = scoreFor - scoreAgainst;
            double opponentRankingFactor = 1.0 / opponent.FIBARanking;
            
            Form += resultDifference * opponentRankingFactor;
            Form = Math.Round(Form, 2);
        }

        public int GetPointDifference()
        {
            return ScoredPoints - ConcededPoints;
        }

        public override string ToString()
        {
            return $"{Team} - Wins: {Wins}, Losses: {Losses}, Points: {Points}, Scored: {ScoredPoints}, Conceded: {ConcededPoints}, Form: {Form}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GroupTeam other = (GroupTeam)obj;
            return ISOCode == other.ISOCode; 
        }

        public override int GetHashCode()
        {
            return ISOCode.GetHashCode();
        }
    }

}
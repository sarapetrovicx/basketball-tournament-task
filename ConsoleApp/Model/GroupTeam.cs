namespace ConsoleApp.Model{
    public class GroupTeam
    {
        public string Team { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }

        public GroupTeam(string team, string isoCode, int fibaRanking){
            Team = team;
            ISOCode = isoCode;
            FIBARanking = fibaRanking;
        }

        public override string ToString()
        {
            return $"Team: {Team}, ISO Code: {ISOCode}, FIBA Ranking: {FIBARanking}";
        }
    }
}
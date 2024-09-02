namespace MyApp.Model{
    public class TeamExhibitions
    {
        public string TeamCode { get; set; }
        public List<ExhibitionMatch> Matches { get; set; }

        public TeamExhibitions(string teamCode, List<ExhibitionMatch> matches)
        {
            TeamCode = teamCode;
            Matches = matches ?? new List<ExhibitionMatch>();
        }
    }
}

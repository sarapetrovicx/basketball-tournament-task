using MyApp.Game;

namespace MyApp.Model{
    public class Group{
        public string Name { get; set; }
        public List<GroupTeam> Teams { get; set; }
        public List<Match> Matches { get; set; }


        public Group(string name, List<GroupTeam> teams){
            Name = name;
            Teams = teams;
            Matches = new List<Match>();
        }


        public void SimulateAndPrintGroupResults(GroupPhase groupPhase){
            Matches = groupPhase.SimulateGroupMatches(Teams);
            Teams = groupPhase.GroupResults(Name, Teams, Matches);
        }

        public override string ToString()
        {
            var result = $"Group {Name}:\n";
            foreach (var team in Teams)
            {
                result += $"{team}\n";
            }
            return result;
        }
    }
}

namespace ConsoleApp.Model{
    public class Group
    {
        public string Name { get; set; }
        public List<GroupTeam> Teams { get; set; }

        public Group(string name, List<GroupTeam> teams)
        {
            Name = name;
            Teams = teams ?? new List<GroupTeam>();
        }

        
    }
}

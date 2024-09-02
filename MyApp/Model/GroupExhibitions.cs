namespace MyApp.Model{
  public class GroupExhibitions
  {
        public List<Group> Groups { get; set; }
        public List<TeamExhibitions> Exhibitions { get; set; }

        public GroupExhibitions(List<Group> groups, List<TeamExhibitions> exhibitions)
      {
          Groups = groups ?? new List<Group>();
          Exhibitions = exhibitions ?? new List<TeamExhibitions>();
      }
      
  }
}

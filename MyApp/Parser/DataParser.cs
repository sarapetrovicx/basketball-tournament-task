using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using MyApp.Model;

namespace MyApp.Parser
{
    public class DataParser(string groupsFilePath, string exhibitionsFilePath)
    {
        private readonly string GroupsFilePath = groupsFilePath;
        private readonly string ExhibitionsFilePath = exhibitionsFilePath;

        public List<Group> ParseGroups()
        {
            string groupsJson = File.ReadAllText(GroupsFilePath, Encoding.UTF8);
            var groupsDict = JsonSerializer.Deserialize<Dictionary<string, List<GroupTeam>>>(groupsJson);
            var groupsList = new List<Group>();

            if (groupsDict != null)
            {
                foreach (var entry in groupsDict)
                {
                    groupsList.Add(new Group(entry.Key, entry.Value));
                }
            }
            return groupsList;
        }

        public List<TeamExhibitions> ParseExhibitions()
        {
            string exhibitionsJson = File.ReadAllText(ExhibitionsFilePath, Encoding.UTF8);
            var exhibitionsDict = JsonSerializer.Deserialize<Dictionary<string, List<ExhibitionMatch>>>(exhibitionsJson);
            var exhibitionsList = new List<TeamExhibitions>();

            if (exhibitionsDict != null)
            {
                foreach (var entry in exhibitionsDict)
                {
                    exhibitionsList.Add(new TeamExhibitions(entry.Key, entry.Value));
                }
            }
            return exhibitionsList;
        }
    }
}
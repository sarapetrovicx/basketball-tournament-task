using System;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.IO;
using ConsoleApp.Model;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main()
        {

            string exhibitionsFilePath = "/Users/sarapetrovic/Desktop/basketball-tournament-task/exibitions.json";
            string groupsFilePath = "/Users/sarapetrovic/Desktop/basketball-tournament-task/groups.json";

            string exhibitionsJson = File.ReadAllText(exhibitionsFilePath, Encoding.UTF8);
            string groupsJson = File.ReadAllText(groupsFilePath, Encoding.UTF8);


            // Parsiranje exhibitions JSON-a
            var exhibitionsDict = JsonSerializer.Deserialize<Dictionary<string, List<ExhibitionMatch>>>(exhibitionsJson);
            var exhibitionsList = new List<TeamExhibitions>();

            if (exhibitionsDict != null)
            {
                foreach (var entry in exhibitionsDict)
                {
                    exhibitionsList.Add(new TeamExhibitions(entry.Key, entry.Value));
                }
            }

            // Parsiranje groups JSON-a
            var groupsDict = JsonSerializer.Deserialize<Dictionary<string, List<GroupTeam>>>(groupsJson);
            var groupsList = new List<Group>();

            if (groupsDict != null)
            {
                foreach (var entry in groupsDict)
                {
                    groupsList.Add(new Group(entry.Key, entry.Value));
                }
            }
            // Kombinovanje u GroupExhibitions objekat
            var groupExhibitions = new GroupExhibitions(groupsList, exhibitionsList);

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };

            string output = JsonSerializer.Serialize(groupExhibitions, jsonOptions);
            Console.WriteLine(output);
        }
    }
}
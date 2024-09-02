using System;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.IO;
using MyApp.Model;
using MyApp.Game;
using MyApp.Parser;


namespace MyApp
{
    public class Program
    {
        public static void Main()
        {
            string groupsFilePath = "../groups.json";
            string exhibitionsFilePath = "../exibitions.json";

            var parser = new DataParser(groupsFilePath, exhibitionsFilePath);

            var groupsList = parser.ParseGroups();
            var exhibitionsList = parser.ParseExhibitions();

            HashSet<GroupTeam> uniqueTeams = new();

            foreach (var group in groupsList)
            {
                foreach (var team in group.Teams)
                {
                    uniqueTeams.Add(team);
                }
            }

            foreach (var teamExhibition in exhibitionsList)
            {
                GroupTeam? groupTeam = uniqueTeams.FirstOrDefault(t => t.ISOCode == teamExhibition.TeamCode);
                groupTeam?.CalculateTeamForm(teamExhibition, uniqueTeams);
            }

            foreach (var team in uniqueTeams)
            {
                Console.WriteLine(team);

            }

            GroupPhase groupPhase = new();

            foreach (var group in groupsList)
            {
                group.SimulateAndPrintGroupResults(groupPhase);
            }
            List<GroupTeam> qualifiedTeams = groupPhase.PrintFinalStandingsAndQualifiedTeams(groupsList);

            var knockoutStage = new KnockoutStage(qualifiedTeams);
            knockoutStage.PrintKnockoutResults();
        }
    }
}

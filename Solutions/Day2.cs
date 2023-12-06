using Repository;
using System.Text.RegularExpressions;

namespace Solutions
{
    public class Day2 : AdventSolutionBase
    {
        private const string Filename = "day2.txt";

        public Day2(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename).ToList();
            var sumAllPossibleGames = 0;
            var criterias = new Dictionary<string, int>
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 },
            };
            for (var i = 0; i < allLines.Count; i++)
            {
                var samples = allLines[i].Split(":").ToList()[1];
                string pattern = @"\;|,";
                var rgx = new Regex(pattern);
                var regexResult = rgx.Split(samples).ToList();

                var colorValueSamples = new Dictionary<string, List<int>>()
                {
                    { "red", new List<int>() },
                    { "green", new List<int>() },
                    { "blue", new List<int>() },
                };
                foreach (var entry in regexResult)
                {
                    var valueColorPair = entry.Trim().Split(" ");
                    colorValueSamples[valueColorPair[1]].Add(int.Parse(valueColorPair[0]));
                }
                var gameNumber = i + 1;
                sumAllPossibleGames += IsGamePossible(colorValueSamples, criterias) ? gameNumber : 0;
            }
            return sumAllPossibleGames;
        }

        private bool IsGamePossible(Dictionary<string, List<int>> samples, Dictionary<string, int> criterias)
        {
            var isPossible = true;
            foreach (var criteria in criterias)
            {
                if (samples[criteria.Key].Max() > criteria.Value)
                {
                    isPossible = false;
                }
            }
            return isPossible;
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename).ToList();
            var sumOfPowers = 0;

            for (var i = 0; i < allLines.Count; i++)
            {
                var samples = allLines[i].Split(":").ToList()[1];
                string pattern = @"\;|,";
                var rgx = new Regex(pattern);
                var regexResult = rgx.Split(samples).ToList();

                var colorValueSamples = new Dictionary<string, List<int>>()
                {
                    { "red", new List<int>() },
                    { "green", new List<int>() },
                    { "blue", new List<int>() },
                };
                foreach (var entry in regexResult)
                {
                    var valueColorPair = entry.Trim().Split(" ");
                    colorValueSamples[valueColorPair[1]].Add(int.Parse(valueColorPair[0]));
                }
                sumOfPowers += FewestPossibleCubesPower(colorValueSamples);
            }
            return sumOfPowers;
        }

        private int FewestPossibleCubesPower(Dictionary<string, List<int>> samples) => samples["green"].Max() * samples["red"].Max() * samples["blue"].Max();
    }
}

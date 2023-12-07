using Repository;
using System.Text.RegularExpressions;

namespace Solutions
{
    public class Day4 : AdventSolutionBase
    {
        private const string Filename = "day4.txt";

        public Day4(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var totalPoints = 0;
            foreach (var line in allLines)
            {
                var rgx = new Regex(@"\:|\|"); // Match ':' or '|'
                var regexResult = rgx.Split(line).ToList();

                var winningNumbers = regexResult[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var myNumbers = regexResult[2].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var wins = 0;
                foreach (var myNumber in myNumbers)
                {
                    if (winningNumbers.Contains(myNumber)) wins++;
                }
                totalPoints += wins != 0 ? (int)Math.Pow(2, wins - 1) : 0;
            }
            return totalPoints;
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            return -1;
        }
    }
}

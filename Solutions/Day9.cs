using Repository;

namespace Solutions
{
    public class Day9 : AdventSolutionBase
    {
        private const string Filename = "day9.txt";

        public Day9(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var nextValues = new List<int>();
            foreach (var line in allLines)
            {
                var values = line.Split(" ").Select(x => int.Parse(x)).ToList();

                var sequences = new List<List<int>>() { values };
                var j = 0;
                while (sequences[j].Any(x => x != 0))
                {
                    var nextSequence = new List<int>();
                    for (var i = 0; i < sequences[j].Count - 1; i++)
                    {
                        nextSequence.Add(sequences[j][i + 1] - sequences[j][i]);
                    }
                    sequences.Add(nextSequence);
                    j++;
                }

                nextValues.Add(CalculateNextValue(sequences));
            }

            return nextValues.Sum();
        }

        public int CalculateNextValue(List<List<int>> sequences)
        {
            for (var i = sequences.Count - 1; i > 0; i--)
            {
                if (i == sequences.Count - 1) sequences[i].Add(0);
                var j = sequences[i - 1].Count - 1;
                var nextValue = sequences[i][j] + sequences[i - 1][j];
                sequences[i - 1].Add(nextValue);
            }

            return sequences.First().Last();
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

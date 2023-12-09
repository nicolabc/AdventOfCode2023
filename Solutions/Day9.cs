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
            var question = 1;
            return GeneralSolution(filename, question);
        }

        private int GeneralSolution(string filename, int question)
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

                nextValues.Add(CalculateValue(sequences, question));
            }

            return nextValues.Sum();
        }

        public int CalculateValue(List<List<int>> sequences, int question) => question == 1 ? CalculateNextValue(sequences) : CalculatePreviousValue(sequences);

        public int CalculateNextValue(List<List<int>> sequences)
        {
            for (var i = sequences.Count - 1; i > 0; i--)
            {
                if (i == sequences.Count - 1) sequences[i].Add(0);

                var nextValue = sequences[i].Last() + sequences[i - 1].Last();
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
            var question = 2;
            return GeneralSolution(filename, question);
        }

        public int CalculatePreviousValue(List<List<int>> sequences)
        {
            for (var i = sequences.Count - 1; i > 0; i--)
            {
                if (i == sequences.Count - 1) sequences[i] = sequences[i].Prepend(0).ToList();

                var nextValue = sequences[i - 1].First() - sequences[i].First();
                sequences[i - 1] = sequences[i - 1].Prepend(nextValue).ToList();
            }

            return sequences.First().First();
        }
    }
}

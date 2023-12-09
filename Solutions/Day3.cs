using Repository;

namespace Solutions
{
    public class Day3 : AdventSolutionBase
    {
        private string Filename => $"{GetType().Name.ToLower()}.txt";

        public Day3(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename).ToList();
            var sum = 0;
            var isAdded = false;
            for (int i = 0; i < allLines.Count; i++)
            {
                var concatNumberUpToJ = string.Empty;
                for (int j = 0; j < allLines[i].Length; j++)
                {
                    var element = allLines[i][j];
                    if (!char.IsDigit(element))
                    {
                        isAdded = false;
                        concatNumberUpToJ = string.Empty;
                        continue;
                    }

                    concatNumberUpToJ += element;
                    if (!isAdded && HasAdjacentSymbol(i, j, allLines).Item1)
                    {
                        isAdded = true;
                        sum += GetFullNumberAfterJ(concatNumberUpToJ, j, allLines[i]);
                    }
                }
            }

            return sum;
        }

        private int GetFullNumberAfterJ(string concatNumberUpToJ, int j, string line)
        {
            for (int k = j + 1; k < line.Length; k++)
            {
                if (!char.IsDigit(line[k])) return int.Parse(concatNumberUpToJ);
                concatNumberUpToJ += line[k];
            }

            return int.Parse(concatNumberUpToJ);
        }

        private (bool, int, int) HasAdjacentSymbol(int i, int j, List<string> allLines, char? onlyAllowedSymbol = null)
        {
            var x = onlyAllowedSymbol;

            // Check above
            if (i > 0)
            {
                if (IsSymbol(allLines[i - 1][j], x)) return (true, i - 1, j);
                if (j > 0 && IsSymbol(allLines[i - 1][j - 1], x)) return (true, i - 1, j - 1);
                if (j + 1 < allLines[i - 1].Length && IsSymbol(allLines[i - 1][j + 1], x)) return (true, i - 1, j + 1);
            }

            // Check left and right
            if (j > 0 && IsSymbol(allLines[i][j - 1], x)) return (true, i, j - 1);
            if (j + 1 < allLines[i].Length && IsSymbol(allLines[i][j + 1], x)) return (true, i, j + 1);

            // Check below
            if (i + 1 < allLines.Count)
            {
                if (IsSymbol(allLines[i + 1][j], x)) return (true, i + 1, j);
                if (j > 0 && IsSymbol(allLines[i + 1][j - 1], x)) return (true, i + 1, j - 1);
                if (j + 1 < allLines[i + 1].Length && IsSymbol(allLines[i + 1][j + 1], x)) return (true, i + 1, j + 1);
            }

            return (false, int.MinValue, int.MinValue);
        }

        private bool IsSymbol(char v, char? onlyAllowedSymbol)
        {
            if (v == '.') return false;
            if (onlyAllowedSymbol is not null && v == onlyAllowedSymbol) return true;
            return !char.IsDigit(v);
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename).ToList();
            var gearsWithPartNumbers = new Dictionary<(int i, int j), List<int>>();
            var isAdded = false;
            for (int i = 0; i < allLines.Count; i++)
            {
                var concatNumberUpToJ = string.Empty;
                for (int j = 0; j < allLines[i].Length; j++)
                {
                    var element = allLines[i][j];
                    if (!char.IsDigit(element))
                    {
                        isAdded = false;
                        concatNumberUpToJ = string.Empty;
                        continue;
                    }

                    concatNumberUpToJ += element;
                    var adjacentTuple = HasAdjacentSymbol(i, j, allLines, '*');
                    if (!isAdded && adjacentTuple.Item1)
                    {
                        isAdded = true;
                        var gearI = adjacentTuple.Item2;
                        var gearJ = adjacentTuple.Item3;
                        if (gearsWithPartNumbers.ContainsKey((gearI, gearJ))) gearsWithPartNumbers[(gearI, gearJ)].Add(GetFullNumberAfterJ(concatNumberUpToJ, j, allLines[i]));
                        else
                        {
                            gearsWithPartNumbers.Add((gearI, gearJ), new List<int> { GetFullNumberAfterJ(concatNumberUpToJ, j, allLines[i]) });
                        }
                    }
                }
            }

            return GetSumOfGearRatios(gearsWithPartNumbers);
        }

        private static int GetSumOfGearRatios(Dictionary<(int i, int j), List<int>> gearsWithPartNumbers)
        {
            var sumOfGearRatios = 0;
            foreach (var partNumbers in gearsWithPartNumbers.Values)
            {
                if (partNumbers.Count != 2) continue;
                sumOfGearRatios += partNumbers.First() * partNumbers.Last();
            }

            return sumOfGearRatios;
        }
    }
}

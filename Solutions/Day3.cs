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
                    if (!isAdded && HasAdjacentSymbol(i, j, allLines))
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

        private bool HasAdjacentSymbol(int i, int j, List<string> allLines)
        {
            // Check above
            if (i > 0)
            {
                if (IsSymbol(allLines[i - 1][j])) return true;
                if (j > 0 && IsSymbol(allLines[i - 1][j - 1])) return true;
                if (j + 1 < allLines[i - 1].Length && IsSymbol(allLines[i - 1][j + 1])) return true;
            }

            // Check left and right
            if (j > 0 && IsSymbol(allLines[i][j - 1])) return true;
            if (j + 1 < allLines[i].Length && IsSymbol(allLines[i][j + 1])) return true;

            // Check below
            if (i + 1 < allLines.Count)
            {
                if (IsSymbol(allLines[i + 1][j])) return true;
                if (j > 0 && IsSymbol(allLines[i + 1][j - 1])) return true;
                if (j + 1 < allLines[i + 1].Length && IsSymbol(allLines[i + 1][j + 1])) return true;
            }

            return false;
        }

        private bool IsSymbol(char v)
        {
            if (v == '.') return false;
            return !char.IsDigit(v);
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

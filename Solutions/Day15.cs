using Repository;

namespace Solutions
{
    public class Day15 : AdventSolutionBase
    {
        private string Filename => $"{GetType().Name.ToLower()}.txt";

        public Day15(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var line = GetAllLines(filename).First();
            var allStrings = line.Split(',');
            var sum = 0;
            foreach (var stringToHash in allStrings)
            {
                sum += HashString(stringToHash);
            }
            return sum;
        }

        public int HashString(string stringToHash)
        {
            var currentValue = 0;
            foreach (var c in stringToHash)
            {
                currentValue += (int)c;
                currentValue *= 17;
                currentValue %= 256;
            }
            return currentValue;
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

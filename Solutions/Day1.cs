using Repository;
using System.Linq;
using System.Threading;

namespace Solutions
{
    public class Day1 : AdventSolutionBase
    {
        private const string Filename = "day1.txt";
        private readonly List<string> digitsAsStrings = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        public Day1(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion() => FirstQuestion(Filename);

        public override int FirstQuestion(string filename)
        {
            var firstDigit = default(char);
            var secondDigit = default(char);
            var sum = 0;
            var allLines = GetAllLines(filename);
            foreach (var line in allLines)
            {
                foreach (var c in line)
                {
                    if (char.IsNumber(c))
                    {
                        if (firstDigit == default)
                        {
                            firstDigit = c;
                            secondDigit = c;
                        }
                        else
                        {
                            secondDigit = c;
                        }
                    }
                }

                sum += GetTwoDigitNumber(firstDigit, secondDigit);
                firstDigit = default;
                secondDigit = default;
            }

            return sum;
        }

        private int GetTwoDigitNumber(char firstDigit, char secondDigit) => GetTwoDigitNumber((int)char.GetNumericValue(firstDigit), (int)char.GetNumericValue(secondDigit));

        private int GetTwoDigitNumber(int firstDigit, int secondDigit) => (firstDigit * 10) + secondDigit;

        public override int SecondQuestion() => SecondQuestion(Filename);

        public override int SecondQuestion(string filename)
        {
            var firstDigit = -1;
            var secondDigit = -1;
            var sum = 0.0;
            var allLines = GetAllLines(filename);
            var possibleDigit = "";
            foreach (var line in allLines)
            {
                foreach (var c in line)
                {
                    possibleDigit += c;
                    if (IsNumber(c, possibleDigit))
                    {
                        if (firstDigit == -1)
                        {
                            firstDigit = GetNumber(c, possibleDigit);
                            secondDigit = GetNumber(c, possibleDigit);
                        }
                        else
                        {
                            secondDigit = GetNumber(c, possibleDigit);
                        }
                        // Need to reset possible digit to the last character of previous string digit. Example input: oneight => one + eight
                        possibleDigit = possibleDigit.Substring(possibleDigit.Length - 1);
                    }
                }

                sum += GetTwoDigitNumber(firstDigit, secondDigit);
                firstDigit = -1;
                secondDigit = -1;
            }

            return (int)sum;
        }

        private int GetNumber(char c, string possibleDigit)
        {
            if (char.IsDigit(c)) return (int)char.GetNumericValue(c);
            return GetNumber(possibleDigit);
        }

        private bool IsNumber(char c, string possibleDigit)
        {
            if (char.IsDigit(c)) return true;
            if (GetNumber(possibleDigit) != -1) return true;
            return false;
        }

        private int GetNumber(string possibleDigit)
        {
            var length = possibleDigit.Length;

            for (int i = 0; i < length;  i++)
            {
                if (digitsAsStrings.Contains(possibleDigit.Substring(i)))
                {
                    return digitsAsStrings.FindIndex(x => x.Contains(possibleDigit.Substring(i))) + 1; //Adding one for zero indexed list;
                }
            }
            return -1;
        }
    }
}
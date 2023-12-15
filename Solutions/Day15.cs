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
            var line = GetAllLines(filename).First();
            var allStrings = line.Split(',');
            var boxes = new List<KeyValuePair<string, int>>[256];

            foreach (var rawStep in allStrings)
            {
                if (rawStep.Contains('='))
                {
                    HandleEqualSignInstruction(boxes, rawStep);
                }
                else if (rawStep.Contains('-'))
                {
                    HandleDashInstruction(boxes, rawStep);
                }
            }

            return GetSumOfFocusingPower(boxes);
        }

        private void HandleDashInstruction(List<KeyValuePair<string, int>>[] boxes, string rawStep)
        {
            var step = rawStep.Split('-');
            var label = step.First();
            var boxIndex = HashString(label);

            if (boxes[boxIndex] == null)
            {
                boxes[boxIndex] = new();
                return;
            }

            var orderWithinBox = boxes[boxIndex].FindIndex(x => x.Key == label);
            if (orderWithinBox != -1)
            {
                boxes[boxIndex].RemoveAt(orderWithinBox);
            }
        }

        private void HandleEqualSignInstruction(List<KeyValuePair<string, int>>[] boxes, string rawStep)
        {
            var step = rawStep.Split('=');
            var label = step.First();
            var focalLength = int.Parse(step.Last());
            var boxIndex = HashString(label);

            if (boxes[boxIndex] == null)
            {
                boxes[boxIndex] = new();
            }

            var orderWithinBox = boxes[boxIndex].FindIndex(x => x.Key == label);
            if (orderWithinBox != -1)
            {
                boxes[boxIndex][orderWithinBox] = new(label, focalLength);
            }
            else
            {
                boxes[boxIndex].Add(new(label, focalLength));
            }
        }

        private static int GetSumOfFocusingPower(List<KeyValuePair<string, int>>[] boxes)
        {
            var sumFocusingPower = 0;
            for (var boxIndex = 0; boxIndex < boxes.Length; boxIndex++)
            {
                var box = boxes[boxIndex];
                if (box is null) continue;

                for (var lensIndex = 0; lensIndex < box.Count; lensIndex++)
                {
                    sumFocusingPower += (boxIndex + 1) * (lensIndex + 1) * box[lensIndex].Value;
                }
            }
            return sumFocusingPower;
        }
    }
}

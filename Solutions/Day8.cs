using Repository;

namespace Solutions
{
    public class Day8 : AdventSolutionBase
    {
        private const string Filename = "day8.txt";

        public Day8(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var map = new Dictionary<string, Node>();

            BuildMap(allLines, map);

            var instructions = allLines.First();
            int steps = GetStepsTraversingMap(map, instructions, "AAA");
            return steps;
        }

        private void BuildMap(IEnumerable<string> allLines, Dictionary<string, Node> map)
        {
            foreach (var line in allLines.Skip(2))
            {
                var nodes = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var element = line.Substring(0, 3);
                map.Add(element, new Node(nodes[2].Substring(1, 3), nodes[3].Substring(0, 3))); // Substring gets the node letters without '(,' and ')'
            }
        }

        private int GetStepsTraversingMap(Dictionary<string, Node> map, string instructions, string startingNode)
        {
            var stepCount = 0;
            var currentNode = startingNode;
            while (currentNode != "ZZZ")
            {
                var currentStep = stepCount % instructions.Length;
                currentNode = instructions[currentStep] == 'L' ? map[currentNode].Left : map[currentNode].Right;
                stepCount++;
            }

            return stepCount;
        }

        public class Node
        {
            public string Left;
            public string Right;

            public Node(string left, string right)
            {
                Left = left;
                Right = right;
            }
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var map = new Dictionary<string, Node>();
            BuildMap(allLines, map, out List<string> startingNodes);

            var instructions = allLines.First();
            long steps = GetStepsTraversingMap(map, instructions, startingNodes);
            Console.WriteLine($"Solutions in long format is {steps}");
            return (int)steps;
        }

        private long GetStepsTraversingMap(Dictionary<string, Node> map, string instructions, List<string> startingNodes)
        {
            var stepCount = (long)0;
            var currentNodes = startingNodes.ToList();
            while (currentNodes.Any(x => x[2] != 'Z'))
            {
                var currentStep = (int)stepCount % instructions.Length;
                for (var i = 0; i < currentNodes.Count; i++)
                {
                    currentNodes[i] = instructions[currentStep] == 'L' ? map[currentNodes[i]].Left : map[currentNodes[i]].Right;
                }
                stepCount++;
            }

            return stepCount;
        }

        private void BuildMap(IEnumerable<string> allLines, Dictionary<string, Node> map, out List<string> startingNodes)
        {
            startingNodes = new List<string>();
            foreach (var line in allLines.Skip(2))
            {
                var nodes = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var element = line.Substring(0, 3);
                map.Add(element, new Node(nodes[2].Substring(1, 3), nodes[3].Substring(0, 3))); // Substring gets the node letters without '(,' and ')'
                if (element[2] == 'A') startingNodes.Add(element);
            }
        }
    }
}

using Repository;

namespace Solutions.AlternativeSolutions
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
            var instructions = allLines.First();

            BuildMap(allLines, map);

            var steps = 0;
            TraverseMap(instructions, map, "AAA", ref steps);
            return steps;
        }

        // Literally getting stackOverflowException using this as the solution :(
        private void TraverseMap(string instructions, Dictionary<string, Node> map, string currentNode, ref int steps)
        {
            var recurringSteps = steps % instructions.Length;
            if (currentNode == "ZZZ") return;
            TraverseMap(instructions, map, instructions[recurringSteps] == 'L' ? map[currentNode].Left : map[currentNode].Right, ref steps);
            steps++;
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

        private void BuildMap(IEnumerable<string> allLines, Dictionary<string, Node> map)
        {
            foreach (var line in allLines.Skip(2))
            {
                var nodes = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var element = line.Substring(0, 3);
                map.Add(element, new Node(nodes[2].Substring(1, 3), nodes[3].Substring(0, 3))); // Substring gets the node letters without '(,' and ')'
            }
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

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
            var instructions = allLines.First();

            foreach (var line in allLines.Skip(2))
            {
                var nodes = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var element = line.Substring(0, 3);
                map.Add(element, new Node(nodes[2].Substring(1, 3), nodes[3].Substring(0, 3))); //substring gets the node letters without '(,' and ')'
            }

            var steps = 0;
            TraverseMap(instructions, map, "AAA", ref steps);
            return steps;
        }

        private void TraverseMap(string instructions, Dictionary<string, Node> map, string currentNode, ref int steps)
        {
            var recurringSteps = steps % instructions.Length;
            if (currentNode == "ZZZ") return;
            else if (instructions[recurringSteps] == 'L')
            {
                steps++;
                TraverseMap(instructions, map, map[currentNode].Left, ref steps); //TraverseMap(instructions, map, instructions[steps] == 'L' ? map[currentNode].Left : map[currentNode].Right, ref steps);
                return;
            }
            else if (instructions[recurringSteps] == 'R')
            {
                steps++;
                TraverseMap(instructions, map, map[currentNode].Right, ref steps);
                return;
            }
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
            return -1;
        }
    }
}

using Repository;
using System.Data;

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
            BuildMap(allLines, map, out List<Ghost> startingNodes);

            var instructions = allLines.First();
            var stepsToZForEachGhost = GetStepsToZForEachGhost(map, instructions, startingNodes);

            var steps = GetLeastCommonMultiplier(stepsToZForEachGhost);
            Console.WriteLine($"Solutions in long format is {steps}");
            return (int)steps;
        }

        private long GetLeastCommonMultiplier(List<long> integers)
        {
            long leastCommonMuliple = integers.First();

            for (var i = 1; i < integers.Count(); i++)
            {
                leastCommonMuliple = GetLeastCommonMultiple(leastCommonMuliple, integers[i]);
            }

            return leastCommonMuliple;
        }

        public long GetLeastCommonMultiple(long a, long b)
        {
            return Math.Abs(a) * Math.Abs(b) / GetGreatestCommonDivsor(a, b);
        }

        public long GetGreatestCommonDivsor(long a, long b)
        {
            while (b != 0)
            {
                var bPrevious = b;
                b = a % b;

                a = bPrevious;
            }
            return Math.Abs(a);
        }

        private List<long> GetStepsToZForEachGhost(Dictionary<string, Node> map, string instructions, List<Ghost> ghosts)
        {
            var stepCount = (long)0;
            while (ghosts.Any(x => x.CurrentNode[2] != 'Z'))
            {
                var currentStep = (int)stepCount % instructions.Length;
                // Traverse each ghost that has not yet reached Z
                for (var i = 0; i < ghosts.Count; i++)
                {
                    if (ghosts[i].StepsToZ != null) continue;

                    var currentNode = instructions[currentStep] == 'L' ? map[ghosts[i].CurrentNode].Left : map[ghosts[i].CurrentNode].Right;
                    ghosts[i].CurrentNode = currentNode;

                    if (currentNode[2] == 'Z') ghosts[i].StepsToZ = stepCount + 1;
                }
                stepCount++;
            }
            return ghosts.Select(x => x.StepsToZ ?? throw new NoNullAllowedException()).ToList();
        }

        private void BuildMap(IEnumerable<string> allLines, Dictionary<string, Node> map, out List<Ghost> initializedGhosts)
        {
            initializedGhosts = new List<Ghost>();
            foreach (var line in allLines.Skip(2))
            {
                var nodes = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var element = line.Substring(0, 3);
                map.Add(element, new Node(nodes[2].Substring(1, 3), nodes[3].Substring(0, 3))); // Substring gets the node letters without '(,' and ')'
                if (element[2] == 'A') initializedGhosts.Add(new Ghost(element));
            }
        }

        public class Ghost
        {
            public string CurrentNode;
            public long? StepsToZ;

            public Ghost(string currentNode)
            {
                CurrentNode = currentNode;
            }
        }
    }
}

using Repository;
using static Solutions.Day10;

namespace Solutions
{
    public class Day10 : AdventSolutionBase
    {
        private string Filename => $"{GetType().Name.ToLower()}.txt";

        public Day10(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename).ToList();
            var (i, j) = GetStartingPoint(allLines);

            var initializer = new Initializer(allLines);
            var startingDirections = initializer.GetStartingPipeDirections(i, j);

            var agents = new List<Agent>();
            var pipeMap = new PipeMap(allLines);
            foreach (var dir in startingDirections)
            {
                agents.Add(new Agent(i, j, dir, pipeMap));
            }

            var reachedFarthestPoint = false;
            while (!reachedFarthestPoint)
            {
                foreach (var agent in agents)
                {
                    agent.Move();
                }

                if (agents[0].i == agents[1].i && agents[0].j == agents[1].j) reachedFarthestPoint = true;
                if (agents[0].NextPosition.Item1 == agents[1].i && agents[0].NextPosition.Item2 == agents[1].j) reachedFarthestPoint = true;
            }

            return agents[0].stepCount;
        }

        public class Initializer
        {
            public List<string> map;
            public string SouthFacingPipes => "|7F";
            public string WestFacingPipes => "-J7";
            public string NorthFacingPipes => "|LJ";
            public string EastFacingPipes => "-LF";

            public Initializer(List<string> map)
            {
                this.map = map;
            }

            public List<Direction> GetStartingPipeDirections(int i, int j)
            {
                var startingDirections = new List<Direction>();
                if (i > 0 && IsPipeConnectedWhenEnteringWithDirection(Direction.North, map[i - 1][j])) startingDirections.Add(Direction.North);
                if (j < map[i].Length - 1 && IsPipeConnectedWhenEnteringWithDirection(Direction.East, map[i][j + 1])) startingDirections.Add(Direction.East);
                if (i < map.Count - 1 && IsPipeConnectedWhenEnteringWithDirection(Direction.South, map[i + 1][j])) startingDirections.Add(Direction.South);
                if (j > 0 && IsPipeConnectedWhenEnteringWithDirection(Direction.West, map[i][j - 1])) startingDirections.Add(Direction.West);

                if (startingDirections.Count != 2) throw new Exception("Incorrect number of starting directions");
                return startingDirections;
            }

            private bool IsPipeConnectedWhenEnteringWithDirection(Direction direction, char potentialPipe)
            {
                return direction switch
                {
                    Direction.North => SouthFacingPipes.Contains(potentialPipe) ? true : false,
                    Direction.East => WestFacingPipes.Contains(potentialPipe) ? true : false,
                    Direction.South => NorthFacingPipes.Contains(potentialPipe) ? true : false,
                    Direction.West => EastFacingPipes.Contains(potentialPipe) ? true : false,
                    _ => false,
                };
            }
        }
       


        public class PipeMap
        {
            public List<string> map;
            public string SouthFacingPipes => "|7F";
            public string WestFacingPipes => "-J7";
            public string NorthFacingPipes => "|LJ";
            public string EastFacingPipes => "-LF";

            public PipeMap(List<string> map)
            {
                this.map = map;
            }

            public char GetPipe(int i, int j)
            {
                return map[i][j];
            }
        }

        public class Agent
        {
            public int i;
            public int j;
            public int stepCount;
            public Direction currentDirection;
            public PipeMap map;

            public Agent(int startingIndexI, int startingIndexJ, Direction startingDirection, PipeMap map)
            {
                i = startingIndexI;
                j = startingIndexJ;
                stepCount = 0;
                currentDirection = startingDirection;
                this.map = map;
            }

            public void Move()
            {
                (i,j) = NextPosition;
                currentDirection = GetNextDirection(currentDirection, map.GetPipe(i, j));
                stepCount++;
            }

            public (int, int) NextPosition => currentDirection switch
            {
                Direction.North => (i - 1, j),
                Direction.East => (i, j + 1),
                Direction.South => (i + 1, j),
                Direction.West => (i, j - 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            private Direction GetNextDirection(Direction currentDirection, char pipe)
            {
                var dir = currentDirection;
                return pipe switch
                {
                    '|' => dir == Direction.North || dir == Direction.South ? dir : throw new Exception($"Unexpected current direction {dir}"),
                    '-' => dir == Direction.East || dir == Direction.West ? dir : throw new Exception($"Unexpected current direction {dir}"),
                    'L' => dir == Direction.South ? Direction.East : dir == Direction.West ? Direction.North : throw new Exception($"Unexpected current direction {dir}"),
                    'J' => dir == Direction.South ? Direction.West : dir == Direction.East ? Direction.North : throw new Exception($"Unexpected current direction {dir}"),
                    '7' => dir == Direction.North ? Direction.West : dir == Direction.East ? Direction.South : throw new Exception($"Unexpected current direction {dir}"),
                    'F' => dir == Direction.North ? Direction.East : dir == Direction.West ? Direction.South : throw new Exception($"Unexpected current direction {dir}"),
                    _ => throw new Exception($"Unknown pipe {pipe} reached with {dir}")
                };
            }
        }

        private (int i, int j) GetStartingPoint(List<string> allLines)
        {
            var i = -1;
            foreach (var line in allLines)
            {
                i++;
                if (line.Contains('S')) return (i, line.IndexOf('S'));
            }

            throw new Exception("No starting point found");
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

    public enum Direction
    {
        North, East, South, West,
    }
}

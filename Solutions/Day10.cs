using Repository;
using System.Linq;
using System.Text;
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

            return agents[0].StepCount;
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

            public PipeMap(List<string> map)
            {
                this.map = map;
            }

            public char GetMapAt(int i, int j)
            {
                return map[i][j];
            }

            public void UpdateMap(int i, int j, char c)
            {
                // Strings are immutable
                StringBuilder sb = new StringBuilder(map[i]);
                sb[j] = c;
                map[i] = sb.ToString();
            }

            //(i,j) is the midpoint from which the update is performed
            public bool UpdateMapInCardinalDirections(int i, int j, char c)
            {
                var updated = false;
                if (i > 0 && GetMapAt(i - 1, j) == '.')
                {
                    UpdateMap(i - 1, j, c); 
                    updated = true;
                }
                if (j > 0 && GetMapAt(i, j - 1) == '.')
                {
                    UpdateMap(i, j - 1, c); 
                    updated = true;
                }
                if (i + 1 < map.Count && GetMapAt(i + 1, j) == '.')
                {
                    UpdateMap(i + 1, j, c); 
                    updated = true;
                }
                if (j + 1 < map[i].Length && GetMapAt(i, j + 1) == '.')
                {
                    UpdateMap(i, j + 1, c); 
                    updated = true;
                }
                return updated;
            }

            public bool UpdateMapInCardinalDirectionsForJunkPipes(int i, int j, char c, List<Position> mainLoop)
            {
                var updated = false;
                if (i > 0 && !mainLoop.Contains(new Position { i = i - 1, j = j}) && GetMapAt(i - 1, j) != c)
                {
                    UpdateMap(i - 1, j, c);
                    updated = true;
                }
                if (j > 0 && !mainLoop.Contains(new Position { i = i, j = j - 1 }) && GetMapAt(i, j - 1) != c)
                {
                    UpdateMap(i, j - 1, c);
                    updated = true;
                }
                if (i + 1 < map.Count && !mainLoop.Contains(new Position { i = i + 1, j = j }) && GetMapAt(i + 1, j) != c)
                {
                    UpdateMap(i + 1, j, c);
                    updated = true;
                }
                if (j + 1 < map[i].Length && !mainLoop.Contains(new Position { i = i, j = j + 1}) && GetMapAt(i, j + 1) != c)
                {
                    UpdateMap(i, j + 1, c);
                    updated = true;
                }
                return updated;
            }

            public int GetCountOfChar(char charInsideLoop)
            {
                var count = 0;
                foreach(var line in map)
                {
                    count += line.Count(x => x == charInsideLoop);
                }
                return count;
            }
        }

        public class Agent
        {
            public int i;
            public int j;
            public int StepCount;
            public Direction CurrentDirection;
            public Direction DirectionBeforeTurning;
            public PipeMap Map;

            public Agent(int startingIndexI, int startingIndexJ, Direction startingDirection, PipeMap map)
            {
                i = startingIndexI;
                j = startingIndexJ;
                StepCount = 0;
                CurrentDirection = startingDirection;
                DirectionBeforeTurning = startingDirection;
                this.Map = map;
            }

            public virtual void Move()
            {
                (i, j) = NextPosition;
                CurrentDirection = GetNextDirection(CurrentDirection, Map.GetMapAt(i, j));
                StepCount++;
            }

            public (int, int) NextPosition => CurrentDirection switch
            {
                Direction.North => (i - 1, j),
                Direction.East => (i, j + 1),
                Direction.South => (i + 1, j),
                Direction.West => (i, j - 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            protected Direction GetNextDirection(Direction currentDirection, char pipe)
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
                    'S' => Direction.South, // Does not matter
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
            var allLines = GetAllLines(filename).ToList();
            var (i, j) = GetStartingPoint(allLines);

            var initializer = new Initializer(allLines);
            var startingDirections = initializer.GetStartingPipeDirections(i, j);

            var pipeMap = new PipeMap(allLines);
            var agent = new AgentWithLRMarking(i, j, startingDirections.First(), pipeMap);

            var loopedOneRound = false;
            while (!loopedOneRound)
            {
                agent.Move();
                if (agent.HasReachedStart())
                {
                    loopedOneRound = true;
                }
            }

            var charInsideLoop = agent.GetNextDirectionOfTopLeftPipe();

            var loopedSecondRound = false;
            agent.SetStartingConditions(i, j, startingDirections.First());
            while (!loopedSecondRound)
            {
                agent.MoveAndMarkLeftRight();
                if (agent.HasReachedStart())
                {
                    loopedSecondRound = true;
                }
            }

            var updatedInsideLoop = true;
            while (updatedInsideLoop)
            {
                updatedInsideLoop = false;
                var positions = GetPositionOfMarkings(agent.Map.map, charInsideLoop);
                foreach (var position in positions)
                {
                    if(agent.UpdateMapInCardinalDirectionsForJunkPipes(position.i, position.j, charInsideLoop))
                    {
                        updatedInsideLoop = true;
                    };
                }
            }
            return agent.Map.GetCountOfChar(charInsideLoop);
        }

        public struct Position
        {
            public int i;
            public int j;
        }

        private List<Position> GetPositionOfMarkings(List<string> allLines, char marking)
        {
            var i = 0;
            var markingsPositions = new List<Position>();
            foreach (var line in allLines)
            {
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == marking)
                    {
                        markingsPositions.Add(new Position() { i = i, j = j });
                    }
                }

                i++;
            }
            return markingsPositions;
        }

        public class AgentWithLRMarking : Agent
        {
            private Direction nextDirectionOfTopLeftPipe;
            private int topLeftI;
            private int topLeftJ;
            private List<Position> loopPositions;

            public AgentWithLRMarking(int startingIndexI, int startingIndexJ, Direction startingDirection, PipeMap map) : base(startingIndexI, startingIndexJ, startingDirection, map)
            {
                // Initialize values in case starting point is top left
                nextDirectionOfTopLeftPipe = startingDirection;
                topLeftI = startingIndexI;
                topLeftJ = startingIndexJ;
                loopPositions = new();
            }

            public bool HasReachedStart()
            {
                return Map.GetMapAt(i, j) == 'S';
            }

            public override void Move()
            {
                base.Move();
                loopPositions.Add(new Position { i = i, j = j });
                if (i < topLeftI || (i == topLeftI && j < topLeftJ))
                {
                    topLeftI = i;
                    topLeftJ = j;
                    nextDirectionOfTopLeftPipe = CurrentDirection;
                }
            }

            public void MoveAndMarkLeftRight()
            {
                DirectionBeforeTurning = CurrentDirection;
                base.Move();
                MarkLeftAndRight();
            }

            public char GetNextDirectionOfTopLeftPipe()
            {
                return nextDirectionOfTopLeftPipe == Direction.East ? 'R' : nextDirectionOfTopLeftPipe == Direction.South ? 'B' : throw new Exception("Unexpected next direction");
            }

            private void MarkLeftAndRight()
            {
                MarkLeftAndRight(DirectionBeforeTurning);
                MarkLeftAndRight(CurrentDirection);
            }

            private bool MarkLeftAndRight(Direction direction) => direction switch
            {
                Direction.North => MarkLeftAndRightWhenFacingNorth(),
                Direction.East => MarkLeftAndRightWhenFacingEast(),
                Direction.South => MarkLeftAndRightWhenFacingSouth(),
                Direction.West => MarkLeftAndRightWhenFacingWest(),
                _ => throw new NotImplementedException(),
            };

            private bool MarkLeftAndRightWhenFacingWest()
            {
                // Check range if they are outside the map and set 'B' and 'R' if not part of main loop
                if (i + 1 < Map.map.Count && !loopPositions.Contains(new Position { i = i + 1, j = j})) Map.UpdateMap(i + 1, j, 'B');
                if (i > 0 && !loopPositions.Contains(new Position { i = i - 1, j = j })) Map.UpdateMap(i - 1, j, 'R');
                return true;
            }

            private bool MarkLeftAndRightWhenFacingSouth()
            {
                // Check range if they are outside the map and set 'B' and 'R' if not part of main loop
                if (j + 1 < Map.map[i].Length && !loopPositions.Contains(new Position { i = i, j = j+1 })) Map.UpdateMap(i, j + 1, 'B');
                if (j > 1 && !loopPositions.Contains(new Position { i = i, j = j - 1 })) Map.UpdateMap(i, j - 1, 'R');
                return true;
            }

            private bool MarkLeftAndRightWhenFacingEast()
            {
                // Check range if they are outside the map and set 'B' and 'R' if not part of main loop
                if (i + 1 < Map.map.Count && !loopPositions.Contains(new Position { i = i + 1, j = j })) Map.UpdateMap(i + 1, j, 'R');
                if (i > 0 && !loopPositions.Contains(new Position { i = i - 1, j = j })) Map.UpdateMap(i - 1, j, 'B');
                return true;
            }

            private bool MarkLeftAndRightWhenFacingNorth()
            {
                // Check range if they are outside the map and set 'B' and 'R' if not part of main loop
                if (j + 1 < Map.map[i].Length && !loopPositions.Contains(new Position { i = i, j = j + 1 })) Map.UpdateMap(i, j + 1, 'R');
                if (j > 1 && !loopPositions.Contains(new Position { i = i, j = j - 1 })) Map.UpdateMap(i, j - 1, 'B');
                return true;
            }

            internal void SetStartingConditions(int i, int j, Direction direction)
            {
                this.i = i;
                this.j = j;
                CurrentDirection = direction;
            }

            public bool UpdateMapInCardinalDirectionsForJunkPipes(int i, int j, char c) => Map.UpdateMapInCardinalDirectionsForJunkPipes(i, j, c, loopPositions);
        }
    }

    public enum Direction
    {
        North, East, South, West,
    }
}

using Repository;
using static Solutions.Day10;

namespace Solutions
{
    public class Day16 : AdventSolutionBase
    {
        private string Filename => $"{GetType().Name.ToLower()}.txt";

        public Day16(IDataRetriever dataRetriever)
            : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);

            var contraptionMap = new ContraptionMapHelper(allLines);
            var beamsStateMachine = new BeamsStateMachine(Heading.Right, 0, 0, contraptionMap);
            while (!beamsStateMachine.HasAllBeamsFinished())
            {
                beamsStateMachine.IterateAllBeamsOneStep();
            }

            return beamsStateMachine.CountAllEnergizedVisitedTiles();
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var maxLengthY = allLines.Count() - 1;
            var maxLengthX = allLines.First().Count() - 1;

            var maxNumberOfEnergizedTiles = 0;
            var contraptionMap = new ContraptionMapHelper(allLines);

            for (var x = 0; x <= maxLengthX; x++)
            {
                for (var y = 0; y <= maxLengthY; y++)
                {
                    if (x != 0 && x != maxLengthX && y != 0 && y != maxLengthY) continue;
                    var startingHeading = GetStartingHeading(x, y, maxLengthX, maxLengthY);

                    var beamsStateMachine = new BeamsStateMachine(startingHeading, x, y, contraptionMap);
                    while (!beamsStateMachine.HasAllBeamsFinished())
                    {
                        beamsStateMachine.IterateAllBeamsOneStep();
                    }

                    var energizedTiles = beamsStateMachine.CountAllEnergizedVisitedTiles();
                    if (energizedTiles > maxNumberOfEnergizedTiles) maxNumberOfEnergizedTiles = energizedTiles;
                }
            }

            return maxNumberOfEnergizedTiles;
        }

        private Heading GetStartingHeading(int x, int y, int maxLengthX, int maxLengthY)
        {
            if (x == 0) return Heading.Right;
            if (x == maxLengthX) return Heading.Left;

            if (y == 0) return Heading.Down;
            if (y == maxLengthY) return Heading.Up;
            throw new ArgumentOutOfRangeException("Outside legal starting points!");
        }
    }
    public class ContraptionMapHelper
    {
        private int minX = 0;
        private int minY = 0;
        private int maxX;
        private int maxY;
        private List<string> contraptionMap { get;set; }
        public ContraptionMapHelper(IEnumerable<string> contraptionMap)
        {
            maxY = contraptionMap.Count() - 1;
            maxX = contraptionMap.First().Count() - 1;
            this.contraptionMap = contraptionMap.ToList();
        }

        public bool IsOutsideOfMap(int x, int y) => x < minX || x > maxX || y < minY || y > maxY;
        public char GetTileAtPosition(int x, int y) => contraptionMap[y][x];
    }

    public class BeamsStateMachine
    {
        private List<Beam> allBeams { get; set; }
        private Dictionary<(int, int), List<Heading>> allVisitedTiles { get; set; }
        private ContraptionMapHelper map { get; set; }

        public BeamsStateMachine(Heading heading, int positionX, int positionY, ContraptionMapHelper map)
        {
            this.map = map;
            allBeams = new() { new(heading, positionX, positionY, map, isSplit: false) };
            allVisitedTiles = new()
            {
                [(positionX, positionY)] = new() { heading },
            };

        }

        public void IterateAllBeamsOneStep()
        {
            for (var i = 0; i < allBeams.Count; i++)
            {
                var currentBeam = allBeams[i];
                if (currentBeam.HasFinished()) continue;

                var visitedTile = currentBeam.IterateOneTimeStep();
                if (currentBeam.HasSplitToTwoBeams)
                {
                    allBeams.Add(new Beam(currentBeam.GetInverseHeading(), currentBeam.GetPositionX(), currentBeam.GetPositionY(), map));
                    currentBeam.HasSplitToTwoBeams = false;
                }

                if (currentBeam.HasHitWall) continue;
                AddVisitedTileToAllVisitedTiles(currentBeam, visitedTile);
            }
        }

        private void AddVisitedTileToAllVisitedTiles(Beam currentBeam, KeyValuePair<(int X, int Y), Heading> visitedTile)
        {
            if (!allVisitedTiles.ContainsKey(visitedTile.Key)) allVisitedTiles[visitedTile.Key] = new() { visitedTile.Value };
            else if (!allVisitedTiles[visitedTile.Key].Contains(visitedTile.Value)) allVisitedTiles[visitedTile.Key].Add(visitedTile.Value);
            else currentBeam.HasEnteredCycle = true;
        }

        public bool HasAllBeamsFinished() => allBeams.TrueForAll(beam => beam.HasFinished());

        public int CountAllEnergizedVisitedTiles()
        {
            return allVisitedTiles.Keys.Count;
        }
    }

    public class Beam
    {
        public Beam(ContraptionMapHelper map)
        {
            Heading = Heading.Right;
            positionX = 0;
            positionY = 0;
            this.map = map;
        }

        public Beam(Heading heading, int positionX, int positionY, ContraptionMapHelper map, bool isSplit = true)
        {
            Heading = heading;
            this.positionX = positionX;
            this.positionY = positionY;
            this.map = map;
            if (isSplit) SetNextPosition(); // The position will be off by one iteration due to the splitted beam initializing another beam.
                                            // Need to iterate one step to get correct initialized position for second beam.
        }

        private ContraptionMapHelper map;
        private int positionX { get; set; }
        private int positionY { get; set; }
        public Heading Heading { get; set; }
        public bool HasHitWall { get; set; }
        public bool HasEnteredCycle { get; set; }
        public bool HasSplitToTwoBeams { get; set; }
        public int GetPositionX() => positionX;
        public int GetPositionY() => positionY;
        public bool HasFinished() => HasHitWall || HasEnteredCycle;
        public Heading GetInverseHeading()
        {
            return Heading switch
            {
                Heading.Up => Heading.Down,
                Heading.Right => Heading.Left,
                Heading.Down => Heading.Up,
                Heading.Left => Heading.Right,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public KeyValuePair<(int X, int Y), Heading> IterateOneTimeStep()
        {
            var tile = map.GetTileAtPosition(positionX, positionY);
            SetNextHeadingAndPosition(tile);
            if (map.IsOutsideOfMap(positionX, positionY)) HasHitWall = true;
            return new((positionX, positionY), Heading);
        }

        private void SetNextHeadingAndPosition(char tileAtCurrentPosition)
        {
            switch (tileAtCurrentPosition)
            {
                case '.':
                    break;
                case '/':
                    Heading = GetNextHeadingForForwardSlash();
                    break;
                case '|':
                    Heading = GetNextHeadingForPipe();
                    break;
                case '\\':
                    Heading = GetNextHeadingForBackSlash();
                    break;
                case '-':
                    Heading = GetNextHeadingForDash();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SetNextPosition();
        }


        private Heading GetNextHeadingForForwardSlash()
        {
            return Heading switch
            {
                Heading.Up => Heading.Right,
                Heading.Right => Heading.Up,
                Heading.Down => Heading.Left,
                Heading.Left => Heading.Down,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private Heading GetNextHeadingForPipe()
        {
            if (Heading == Heading.Left || Heading == Heading.Right) HasSplitToTwoBeams = true;
            return Heading switch
            {
                Heading.Up => Heading.Up,
                Heading.Right => Heading.Up,
                Heading.Down => Heading.Down,
                Heading.Left => Heading.Down,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private Heading GetNextHeadingForBackSlash()
        {
            return Heading switch
            {
                Heading.Up => Heading.Left,
                Heading.Right => Heading.Down,
                Heading.Down => Heading.Right,
                Heading.Left => Heading.Up,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private Heading GetNextHeadingForDash()
        {
            if (Heading == Heading.Up || Heading == Heading.Down) HasSplitToTwoBeams = true;
            return Heading switch
            {
                Heading.Up => Heading.Right,
                Heading.Right => Heading.Right,
                Heading.Down => Heading.Left,
                Heading.Left => Heading.Left,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private void SetNextPosition()
        {
            switch (Heading)
            {
                case Heading.Up:
                    positionY--;
                    break;
                case Heading.Right:
                    positionX++;
                    break;
                case Heading.Down:
                    positionY++;
                    break;
                case Heading.Left:
                    positionX--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum Heading
    {
        Up, Right, Down, Left
    }
}
using Repository;

namespace Solutions
{
    public class Day6 : AdventSolutionBase
    {
        private const string Filename = "day6.txt";

        public Day6(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var time = GetListOfNumbers(allLines.First());
            var distanceRecords = GetListOfNumbers(allLines.Last());

            return GetMultipliedWaysToBeatRecord(time, distanceRecords);
        }

        private int GetMultipliedWaysToBeatRecord(List<long> time, List<long> distanceRecords)
        {
            var numberOfWaysToBeatTheRecord = new List<int>();
            var multipliedWaysToBeatRecord = 1;
            for (int i = 0; i < time.Count; i++)
            {
                var waysToBeatRecord = GetNumberOfWaysToBeatTheRecord(time[i], distanceRecords[i]);
                numberOfWaysToBeatTheRecord.Add(waysToBeatRecord);
                multipliedWaysToBeatRecord *= waysToBeatRecord;
            }

            return multipliedWaysToBeatRecord;
        }

        private int GetNumberOfWaysToBeatTheRecord(long timeInTheRace, long currentRecord)
        {
            // Distance travelled as a function of t where t is the time (variable) to hold the button
            // and where T is Time in the race :
            // D(t) = 1t(T-t) = -t^2+Tt
            // Setting D(t) > L, where L is the current record and solving for t yields:
            // -t^2+Tt-L > 0
            // Solving this using quadratic formula:

            // Transforming constants to math syntax
            var T = timeInTheRace;
            var L = currentRecord;

            var lowest_t_valueToEqualRecord = (-T + Math.Sqrt((T * T) - (4 * L))) / (-2);
            var highest_t_ValueToEqualRecord = (-T - Math.Sqrt((T * T) - (4 * L))) / (-2);

            var t_lower = (int)Math.Floor(lowest_t_valueToEqualRecord + 1);
            var t_higher = (int)Math.Ceiling(highest_t_ValueToEqualRecord - 1);

            return t_higher - t_lower + 1;
        }

        private List<long> GetListOfNumbers(string firstLine)
        {
            var cleanedList = firstLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var numbersAsStrings = cleanedList.Where(x => !x.Contains(':'));
            return numbersAsStrings.Select(x => long.Parse(x)).ToList();
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public override int SecondQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var time = GetNumberWithoutSpaces(allLines.First());
            var distanceRecords = GetNumberWithoutSpaces(allLines.Last());

            return GetMultipliedWaysToBeatRecord(time, distanceRecords);
        }

        private List<long> GetNumberWithoutSpaces(string firstLine)
        {
            var test = firstLine.Replace(" ", "");
            var cleanedList = test.Split(":")[1];
            return new List<long>() { long.Parse(cleanedList) };
        }
    }
}

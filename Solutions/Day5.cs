using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    public class Day5 : AdventSolutionBase
    {
        private const string Filename = "day5.txt";

        public Day5(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            var allLines = GetAllLines(filename);
            var almanac = new Almanac();
            almanac.AllMaps.AddRange(
            new List<List<Map>> { 
                almanac.SeedToSoilMaps, 
                almanac.SoilToFertilizerMaps, 
                almanac.FertilizerToWaterMaps, 
                almanac.WaterToLightMaps, 
                almanac.LightToTemperatureMaps, 
                almanac.TemperatureToHumidityMaps,
                almanac.HumidityToLocationMaps,
            });
            var addCurrentLineToMap = false;
            var CurrentAllMapsIndex = -1; //Start at -1 due to a gap between seeds and seeds-to-soil map
            foreach (var line in allLines)
            {
                if (line.StartsWith("seeds: "))
                {
                    var splitted = line.Split("seeds: ")[1];
                    var seedsAsStrings = splitted.Split(" ");
                    almanac.Seeds = seedsAsStrings.Select(x => long.Parse(x)).ToList();
                }

                if (line.Contains("map:"))
                {
                    addCurrentLineToMap = true;
                    continue;
                }

                if (line == "")
                {
                    CurrentAllMapsIndex++;
                    continue;
                }

                if (addCurrentLineToMap)
                {
                    almanac.AddLineToMap(line, almanac.AllMaps[CurrentAllMapsIndex]);
                }
            }

            var lowestDestination = almanac.GetLowestDestination();
            Console.WriteLine(lowestDestination);
            return (int)lowestDestination;
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

    public class Almanac
    {
        public List<long> Seeds { get; set; }
        public List<Map> SeedToSoilMaps { get; set; } = new();
        public List<Map> SoilToFertilizerMaps { get; set; } = new();
        public List<Map> FertilizerToWaterMaps { get; set; } = new();
        public List<Map> WaterToLightMaps { get; set; } = new();
        public List<Map> LightToTemperatureMaps { get; set; } = new();
        public List<Map> TemperatureToHumidityMaps { get; set; } = new();
        public List<Map> HumidityToLocationMaps { get; set; } = new();

        public List<List<Map>> AllMaps { get; set; } = new() { };

        public void AddLineToMap(string? line, List<Map> maps)
        {
            if (line == null) throw new ArgumentNullException();
            var splitted = line.Split(" ");
            maps.Add(new Map
            {
                DestinationRangeStart = long.Parse(splitted[0]),
                SourceRangeStart = long.Parse(splitted[1]),
                RangeLength = long.Parse(splitted[2]),
            });
        }

        public long GetLowestDestination()
        {
            var allLocations = new List<long>();
            foreach (var seed in Seeds)
            {
                allLocations.Add(GetLocation(seed, AllMaps));
            }

            return allLocations.Min();
        }

        public long GetLocation(long seed, List<List<Map>> allMapsInOrder)
        {
            var currentPosition = seed;
            foreach (var maps in allMapsInOrder)
            {
                currentPosition = GetDestination(currentPosition, maps);
            }

            return currentPosition;
        }

        public long GetDestination(long input, List<Map> maps)
        {
            foreach (var map in maps)
            {
                if (input >= map.SourceRangeStart && input < map.SourceRangeStart + map.RangeLength)
                {
                    var transformationLength = input - map.SourceRangeStart;
                    return map.DestinationRangeStart + transformationLength;
                }
            }

            return input;
        }
    }

    public class Map
    {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }
    }
}

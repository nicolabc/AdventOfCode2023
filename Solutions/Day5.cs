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
            List<string> seedsRawAsStrings = new();
            foreach (var line in allLines)
            {
                if (line.StartsWith("seeds: "))
                {
                    var splitted = line.Split("seeds: ")[1];
                    seedsRawAsStrings = splitted.Split(" ").ToList();
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

            // Optimizing solution
            var possibleLowestLocations = new List<long>();
            var skippableLength = (long)0;
            var lastSeedNotSkipped = (long)0;
            for (var i = 0; i < seedsRawAsStrings.Count; i += 2)
            {
                var count = 0;
                var rangeForSeed = long.Parse(seedsRawAsStrings[i + 1]);
                var seedStart = long.Parse(seedsRawAsStrings[i]);
                while (count < rangeForSeed)
                {
                    almanac.Seeds.Add(seedStart + count);
                    count++;

                    var currentSeed = seedStart + count;
                    if (currentSeed < lastSeedNotSkipped + skippableLength && currentSeed > lastSeedNotSkipped) continue;
                    (var location, skippableLength) = almanac.GetLocation(currentSeed, almanac.AllMaps);
                    possibleLowestLocations.Add(location);
                    lastSeedNotSkipped = currentSeed;
                }
            }

            var lowestDestination = possibleLowestLocations.Min();
            Console.WriteLine(lowestDestination);
            return (int)lowestDestination;
        }
    }

    public class Almanac
    {
        public List<long> Seeds { get; set; } = new();
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
            var skippableLength = (long)0;
            var lastSeedNotSkipped = (long)0;
            for (var i = 0; i < Seeds.Count; i++)
            {
                var currentSeed = Seeds[i];
                if (currentSeed < lastSeedNotSkipped + skippableLength && currentSeed > lastSeedNotSkipped)
                {
                    Console.WriteLine("Skipping!");
                    continue;
                };
                (var location, skippableLength) = GetLocation(currentSeed, AllMaps);
                allLocations.Add(location);
                lastSeedNotSkipped = currentSeed;
            }

            return allLocations.Min();
        }

        public (long, long) GetLocation(long seed, List<List<Map>> allMapsInOrder)
        {
            var currentPosition = seed;
            long upperBoundSkippableDestination = long.MaxValue;
            foreach (var maps in allMapsInOrder)
            {
                (currentPosition, var skippableLength) = GetDestination(currentPosition, maps);
                if (skippableLength < upperBoundSkippableDestination) upperBoundSkippableDestination = skippableLength;
            }

            return (currentPosition, upperBoundSkippableDestination);
        }

        public (long, long) GetDestination(long input, List<Map> maps)
        {
            var lowestHigherRangeStart = long.MaxValue;
            foreach (var map in maps)
            {

                if (input >= map.SourceRangeStart && input < map.SourceRangeStart + map.RangeLength)
                {
                    var transformationLength = input - map.SourceRangeStart;
                    var skippableLength = map.SourceRangeStart + map.RangeLength - input;
                    return (map.DestinationRangeStart + transformationLength, skippableLength);
                }

                if (input < map.SourceRangeStart && map.SourceRangeStart < lowestHigherRangeStart) lowestHigherRangeStart = map.SourceRangeStart;
            }

            return (input, lowestHigherRangeStart-input);
        }
    }

    public class Map
    {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }
    }
}

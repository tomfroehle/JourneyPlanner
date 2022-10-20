using JourneyPlanner.Models;

namespace JourneyPlanner;

internal class Program
{
    public static void Main(string[] args)
    {
        var journeys = JourneySearcher.FindShortestJourneys(args[0], args[1]);
        foreach (var journey in journeys)
        {
            Print(journey);
        }
    }

    private static void Print(Journey journey)
    {
        Console.WriteLine($"{string.Join(',', journey.Connections.Select(PrintConnection))}, {journey.Duration}");

        static string PrintConnection(Connection s)
        {
            return $"{s.Source.Name}-{s.Source.Line} -> {s.Destination.Name}-{s.Destination.Line}";
        }
    }
}
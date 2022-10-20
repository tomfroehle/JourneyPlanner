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
        Console.WriteLine($"{string.Join(',', journey.Edges.Select(PrintEdge))}, {journey.Duration}");

        static string PrintEdge(Edge edge)
        {
            return $"{edge.Source.Name}-{edge.Source.Line} -> {edge.Destination.Name}-{edge.Destination.Line}";
        }
    }
}
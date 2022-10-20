using JourneyPlanner.Models;

namespace JourneyPlanner;

internal class Program
{
    public static void Main(string[] args)
    {
        var journeys = JourneySearcher.FindShortestJourneys(args[0], args[1]);
        Print(journeys);
    }

    private static void Print(IEnumerable<Journey> journeys)
    {
        foreach (var journey in journeys)
        {
            Print(journey);
        }
    }

    private static void Print(Journey journey)
    {
        var edges = string.Join(',', journey.Edges.Select(PrintEdge));
        Console.WriteLine($"{edges}, {journey.Duration}");

        static string PrintEdge(Edge edge)
        {
            return $"{edge.Source.Name}-{edge.Source.Line} -> {edge.Destination.Name}-{edge.Destination.Line}";
        }
    }
}